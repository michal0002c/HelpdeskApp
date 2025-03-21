using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Models;
using HelpdeskApp.Data;
using Microsoft.AspNetCore.Http;

namespace HelpdeskApp.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("Username") != null;
        }

        public IActionResult Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var tickets = _context.Tickets.ToList();
            return View(tickets);
        }

        public IActionResult Create()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }
    }
}
