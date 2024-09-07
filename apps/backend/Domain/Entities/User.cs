using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChefizaApi.Contracts;

namespace ChefizaApi.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(30)]
        public required string FirstName { get; set; }

        [MaxLength(30)]
        public required string LastName { get; set; }

        [Key, EmailAddress, MaxLength(60), ForeignKey("Credentials")]
        public required string Email { get; set; }
        [InverseProperty("Profile")]
        public virtual Auth? Credentials { get; set; }
    }
}