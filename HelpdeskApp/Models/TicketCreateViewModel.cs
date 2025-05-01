using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    public class TicketCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
