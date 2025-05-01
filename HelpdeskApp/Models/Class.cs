using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public int TicketId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
