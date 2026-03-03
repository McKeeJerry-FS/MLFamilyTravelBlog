using Microsoft.AspNetCore.Mvc;
using MLFamilyTravelBlog.Data;
using MLFamilyTravelBlog.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MLFamilyTravelBlog.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;

        public SubscriptionController(ApplicationDbContext context, UserManager<BlogUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("Email", "Email is required.");
                return View();
            }

            if (_context.Subscribers.Any(s => s.Email == email))
            {
                ViewBag.Message = "You are already subscribed!";
                return View();
            }

            var subscriber = new Subscriber
            {
                Email = email,
                SubscribedOn = DateTime.UtcNow
            };
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Thank you for subscribing!";
            return RedirectToAction("Index", "BlogPosts");
        }

        [HttpGet]
        public IActionResult Unsubscribe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(string email)
        {
            var subscriber = _context.Subscribers.FirstOrDefault(s => s.Email == email);
            if (subscriber != null)
            {
                _context.Subscribers.Remove(subscriber);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "You have been unsubscribed.";
            }
            else
            {
                ViewBag.Message = "Email not found.";
                return View();
            }
            return RedirectToAction("Index", "BlogPosts");
        }

        // Helper method to check if user is subscribed
        private bool IsUserSubscribed(string email)
        {
            return _context.Subscribers.Any(s => s.Email == email);
        }
    }
}

