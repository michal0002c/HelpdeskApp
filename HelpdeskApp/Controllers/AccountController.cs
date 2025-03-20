using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Models;
using HelpdeskApp.Data;
using System.Linq;

namespace HelpdeskApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Incorrect username or password.";
                return View();
            }
        }
    }
}
