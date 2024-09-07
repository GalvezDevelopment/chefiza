using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefizaApi.Entities {
    [Table("Authentication")]
    public class Auth : BaseEntity {
        [Key]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public virtual User? Profile { get; set; }
    }
}