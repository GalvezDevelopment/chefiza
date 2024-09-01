using System.ComponentModel.DataAnnotations.Schema;

namespace ChefizaApi.Entities {
    public class BaseEntity {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}