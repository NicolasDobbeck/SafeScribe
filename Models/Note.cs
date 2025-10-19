using SafeScribe.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeScribe.Models
{
    public class Note
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? Owner { get; set; }
    }
}