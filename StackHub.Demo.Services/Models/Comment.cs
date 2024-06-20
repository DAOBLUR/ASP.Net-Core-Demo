using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StackHub.Demo.Services.Models
{
    public class Comment : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string? Content { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Question))]
        public Guid QuestionId { get; set; }
    }
}