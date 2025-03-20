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

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
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

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Sprawdzenie, czy użytkownik o takiej nazwie już istnieje
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Użytkownik o tej nazwie już istnieje.");
                    return View(user);
                }

                // Dodanie użytkownika do bazy danych
                _context.Users.Add(user);
                _context.SaveChanges();

                // Przekierowanie po udanej rejestracji
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
