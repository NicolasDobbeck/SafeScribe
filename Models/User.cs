using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}