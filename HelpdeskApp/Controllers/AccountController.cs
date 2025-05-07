using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Models;
using HelpdeskApp.Data;
using HelpdeskApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;
using HelpdeskApp.Helpers;

namespace HelpdeskApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AccountController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            var hashedPassword = PasswordHasher.Hash(model.Password);

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == model.Username && u.Password == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role.ToString());

                return RedirectToAction("Index", "Ticket");
            }
            else
            {
                ViewBag.Error = "Incorrect username or password.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u =>
                    u.Username == model.Username || u.Email == model.Email);

                if (existingUser != null)
                {
                    if (existingUser.Username == model.Username)
                    {
                        ModelState.AddModelError(nameof(model.Username), "Username already exists.");
                    }

                    if (existingUser.Email == model.Email)
                    {
                        ModelState.AddModelError(nameof(model.Email), "Email is already in use.");
                    }

                    return View(model); 
                }

                var hashedPassword = PasswordHasher.Hash(model.Password);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = hashedPassword,
                    Role = UserRole.User
                };

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var fileName = Path.GetFileName(model.ProfileImage.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ProfileImage.CopyTo(stream);
                    }

                    user.ProfileImagePath = "/images/" + fileName;
                }

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) return NotFound();

            var model = new EditProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                CurrentImagePath = user.ProfileImagePath
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditProfileViewModel model)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                user.Password = PasswordHasher.Hash(model.NewPassword);
            }

            if (ModelState.IsValid)
            {
                user.Username = model.Username;
                user.Email = model.Email;

                if (model.NewProfileImage != null && model.NewProfileImage.Length > 0)
                {
                    var fileExt = Path.GetExtension(model.NewProfileImage.FileName).ToLower();
                    if (fileExt == ".jpg" || fileExt == ".png")
                    {
                        var fileName = Guid.NewGuid().ToString() + fileExt;
                        var filePath = Path.Combine("wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.NewProfileImage.CopyTo(stream);
                        }

                        user.ProfileImagePath = "/images/" + fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("NewProfileImage", "Only JPG and PNG files are allowed.");
                        return View(model);
                    }
                }

                _context.SaveChanges();
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Edit");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
