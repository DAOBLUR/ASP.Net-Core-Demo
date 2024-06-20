using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StackHub.Demo.Services.Models
{
    public abstract class BaseModel
    {
        [Required]
        public bool Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [ForeignKey(nameof(Administrator))]
        public Guid AdministratorId { get; set; }
    }
}