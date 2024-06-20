using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StackHub.Demo.Services.Models
{
    public class Category : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
