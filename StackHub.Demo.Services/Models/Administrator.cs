using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StackHub.Demo.Services.Models
{
    public class Administrator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string? Username { get; set; }
        
        [Required]
        [MinLength(5)]
        [MaxLength(35)]
        public string? Email { get; set; }

        [Required]
        public byte[]? PasswordHash { get; set; }

        [Required]
        public bool IsConnected { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}