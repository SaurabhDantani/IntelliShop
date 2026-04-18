using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IConfiguration _configuration;

        public PaymentController(EcommerceContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.CartItems = cart.CartItems;
            ViewData["PayPalClientId"] = _configuration["PayPal:ClientId"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder([FromBody] PaymentViewModel request)
        {
            if (string.IsNullOrEmpty(request?.id) || request?.status != "COMPLETED")
            {
                return BadRequest("Invalid payment");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return BadRequest(new { success = false, message = "Cart is empty" });

            decimal subtotal = (decimal)cart.CartItems.Sum(i => i.Product.Price * i.Quantity);
            decimal total = subtotal + (subtotal * 0.08m);

            // Create Order
            var order = new Orders
            {
                UserId = userId,
                TotalAmount = total
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); 

            // Create Payment
            var payment = new Payments
            {
                OrderId = order.Id,
                Amount = total,
                PaymentMethod = "PayPal",
                PaymentDate = DateTime.UtcNow,
                UserId = userId
            };
            _context.Payments.Add(payment);

            // Clear Cart
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return Json(new { success = true, orderId = order.Id });
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
