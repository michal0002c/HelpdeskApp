using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Models;
using HelpdeskApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HelpdeskApp.ViewModels;

//namespace
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

        private string GetLoggedInUsername()
        {
            return HttpContext.Session.GetString("Username");
        }

        private User GetLoggedInUser()
        {
            var username = GetLoggedInUsername();
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public IActionResult Index()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var tickets = _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                .ToList();

            var user = GetLoggedInUser();
            int myCount;

            if (user.Role == UserRole.Admin)
            {
                myCount = _context.Tickets.Count(t => t.AssignedToId == user.Id);
            }
            else
            {
                myCount = _context.Tickets.Count(t => t.Username == user.Username);
            }

            ViewBag.MyTicketsCount = myCount;
            ViewBag.AllTicketsCount = tickets.Count;

            return View("TicketList", tickets);
        }

        public IActionResult MyTickets()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var user = GetLoggedInUser();

            List<Ticket> tickets;

            if (user.Role == UserRole.Admin)
            {
                tickets = _context.Tickets
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments)
                    .Where(t => t.AssignedToId == user.Id)
                    .ToList();
            }
            else
            {
                tickets = _context.Tickets
                    .Include(t => t.AssignedTo)
                    .Include(t => t.Comments)
                    .Where(t => t.Username == user.Username)
                    .ToList();
            }

            ViewBag.MyTicketsCount = tickets.Count;
            ViewBag.AllTicketsCount = _context.Tickets.Count();

            return View("TicketList", tickets);
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
        public IActionResult Create(TicketCreateViewModel model)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ticket = new Ticket
            {
                Title = model.Title,
                Description = model.Description,
                Username = GetLoggedInUsername()
            };

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

            var ticket = _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                .FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        public IActionResult AssignToMe(int id)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            var user = GetLoggedInUser();

            if (ticket == null || user == null || user.Role != UserRole.Admin)
                return Unauthorized();

            ticket.AssignedToId = user.Id;
            ticket.Status = TicketStatus.InProgress;

            _context.SaveChanges();
            return RedirectToAction("Details", new { id });
        }

        private bool IsAdmin()
        {
            var user = GetLoggedInUser();
            return user != null && user.Role == UserRole.Admin;
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, TicketStatus status)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            var user = GetLoggedInUser();

            if (ticket == null || user == null || user.Role != UserRole.Admin)
                return Unauthorized();

            ticket.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public IActionResult AddComment(CommentViewModel model)
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return RedirectToAction("Details", new { id = model.TicketId });

            var comment = new Comment
            {
                TicketId = model.TicketId,
                Content = model.Content,
                Username = GetLoggedInUsername(),
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = model.TicketId });
        }

    }
}
