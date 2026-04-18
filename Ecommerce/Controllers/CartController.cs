using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly EcommerceContext _context; 
        
        public CartController(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUserCart = await _context.Carts.FirstOrDefaultAsync(u => u.UserId == userId);
            
            var getCartItems = new List<CartItem>();
            if (getUserCart != null)
            {
                getCartItems = await _context.CartItems
                    .Include(c => c.Product)
                        .ThenInclude(p => p.Category)
                    .Where(c => c.CartId == getUserCart.Id)
                    .ToListAsync();
            }

            ViewBag.CartItems = getCartItems;
            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            // 1. Get or create the Cart for the user
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity  = quantity,
                    CartId = cart.Id
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync(); 
            
            TempData["SuccessMessage"] = $"{product.Name} was added to your cart!";
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int change)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity += change;
                if (cartItem.Quantity <= 0)
                {
                    _context.CartItems.Remove(cartItem);
                    TempData["SuccessMessage"] = $"{cartItem.Product.Name} was removed from your cart.";
                }
                else
                {
                    TempData["SuccessMessage"] = $"Quantity for {cartItem.Product.Name} updated!";
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Product).FirstOrDefaultAsync(ci => ci.Id == cartItemId);
            if (cartItem != null)
            {
                var productName = cartItem.Product.Name;
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"{productName} was removed from your cart.";
            }
            return RedirectToAction("Index");
        }
    }
}
