using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly EcommerceContext _context;

        public UserController(UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager, EcommerceContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = "Please fill in all required fields.";
                TempData["ActiveTab"] = "login";
                return RedirectToAction("Index", "Home");
            }

            // Find user by email using Identity (handles normalization correctly)
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["AuthError"] = "Invalid email or password.";
                TempData["ActiveTab"] = "login";
                return RedirectToAction("Index", "Home");
            }

            // SignInManager handles password check + cookie sign-in in one call
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["AuthError"] = "Invalid email or password.";
            TempData["ActiveTab"] = "login";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userData)
        {
            if (!ModelState.IsValid)
            {
                TempData["AuthError"] = "Please fill in all required fields.";
                TempData["ActiveTab"] = "register";
                return RedirectToAction("Index", "Home");
            }

            var user = new AspNetUser
            {
                FirstName = userData.Name,
                LastName = userData.LastName ?? string.Empty,
                Email = userData.Email,
                UserName = userData.Email, // Use Email as UserName — avoids duplicate UserName issues
            };

            var result = await _userManager.CreateAsync(user, userData.Password);

            if (result.Succeeded)
            {
                // Auto sign-in after registration
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Surface Identity errors back to the view
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            TempData["AuthError"] = errors;
            TempData["ActiveTab"] = "register";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            
            var orders = await _context.Orders
                .Include(o => o.Payment)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            ViewBag.User = user;
            return View(orders);
        }
    }
}
