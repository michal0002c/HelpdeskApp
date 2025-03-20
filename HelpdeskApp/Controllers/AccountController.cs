using HelpdeskApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpdeskApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            if (user.UserName == "admin" && user.Password == "admin") 
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
