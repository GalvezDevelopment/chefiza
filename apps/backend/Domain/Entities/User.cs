using System.ComponentModel.DataAnnotations;
using ChefizaApi.Contracts;

namespace ChefizaApi.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [MaxLength(30)]
        public required string LastName { get; set; }
        [Key]
        [EmailAddress]
        [MaxLength(60)]
        public required string Email { get; set; }
    }
}