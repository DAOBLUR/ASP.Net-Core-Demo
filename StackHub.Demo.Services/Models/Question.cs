using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StackHub.Demo.Services.Models
{
    public class Question : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string? Title { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(35)]
        public string? Content { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}