using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        public readonly EcommerceContext _context;
        public HomeController(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id)
        {
            var products = id != 0
                ? await _context.Products.Where(p => p.CategoryId == id).ToListAsync()
                : await _context.Products.ToListAsync();

            if (products.Count <= 0)
            {
                ViewBag.Message = "No Products found";
            }

            var categories = await _context.Categories.ToListAsync();

            ViewBag.Products = products;
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult GetProductsByCategory()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
