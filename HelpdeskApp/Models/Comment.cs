using HelpdeskApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Comment
{
    public int Id { get; set; }

    [Required]
    public int TicketId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; }

    [Required]
    public string Content { get; set; }

    public string Username { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
