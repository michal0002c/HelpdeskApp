using System.ComponentModel.DataAnnotations;
namespace HelpdeskApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; } = "New";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}