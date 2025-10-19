using System.ComponentModel.DataAnnotations;
using SafeScribe.Models;

namespace SafeScribe.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = AppRoles.Leitor;
    }
}