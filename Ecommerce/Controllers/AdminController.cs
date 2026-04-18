using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    // You can restrict this to Admin role later if you add roles.
    [Authorize]
    public class AdminController : Controller
    {
        private readonly EcommerceContext _context;

        public AdminController(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Products = await _context.Products.Include(p => p.Category).OrderByDescending(p => p.Id).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                TempData["ErrorMessage"] = "Category name cannot be empty.";
                return RedirectToAction("Index");
            }

            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == categoryName.ToLower());

            if (exists)
            {
                TempData["ErrorMessage"] = $"Category '{categoryName}' already exists.";
                return RedirectToAction("Index");
            }

            var newCategory = new Category { Name = categoryName };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Category '{categoryName}' added successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price == null || product.CategoryId == null)
            {
                TempData["ErrorMessage"] = "Product Name, Price, and Category are required.";
                return RedirectToAction("Index");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Product '{product.Name}' added successfully.";
            return RedirectToAction("Index");
        }
    }
}
