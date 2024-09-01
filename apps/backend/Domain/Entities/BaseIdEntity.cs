using System.ComponentModel.DataAnnotations;
using ChefizaApi.Entities;

namespace ChefizaApi.Contracts
{
    public class BaseIdEntity : BaseEntity
    {
        [Key]
        public required string Id { get; set; }
    }
}