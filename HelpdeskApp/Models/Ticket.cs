using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpdeskApp.Models
{
    public enum TicketStatus
    {
        New,
        InProgress,
        Closed
    }

    public class Ticket
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.New;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public User AssignedTo { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
