using System.ComponentModel.DataAnnotations;

namespace SafeScribe.DTOs
{
    public class NoteUpdateDto
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "O conteúdo é obrigatório.")]
        public string Content { get; set; } = string.Empty;
    }
}