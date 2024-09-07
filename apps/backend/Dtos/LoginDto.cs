using System.ComponentModel.DataAnnotations;

namespace ChefizaApi.Dtos
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, MinLength(6), MaxLength(16)]
        public string? Password { get; set; }
    }
}