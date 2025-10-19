using SafeScribe.Models;

namespace SafeScribe.DTOs.Response

{
    public class NoteResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }

        public static NoteResponseDto FromNote(Note note)
        {
            return new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UserId = note.UserId
            };
        }
    }
}
