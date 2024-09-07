using System.ComponentModel.DataAnnotations;

namespace ChefizaApi.Dtos {
    public struct CreateUserDto {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [MaxLength(60), EmailAddress]
        public string Email { get; set; }
        [MinLength(6), MaxLength(16)]
        public string Password { get; set; }
    }
}