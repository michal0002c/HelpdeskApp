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
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            ticket.Username = HttpContext.Session.GetString("Username");

            Console.WriteLine($"Session Username: {ticket.Username}");

            if (!ModelState.IsValid) 
            {
                Console.WriteLine("Errors:");

                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"Pole: {key}, Błąd: {error.ErrorMessage}");
                    }
                }

                return View(ticket);
            }

            _context.Tickets.Add(ticket);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
    }
}
