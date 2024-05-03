using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30),MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30), MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(30), MinLength(3)]
        public string Address { get; set; }
        [Required]
        [Phone]
        public string Telephone { get; set; }
        [Required]
        [MaxLength(30), MinLength(3)]
        public string City { get; set; }
        public string? Region { get; set; }

        [Required]
        public int PostalCode { get; set; }

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Country { get; set; }

        [Required]
        [MaxLength(30), MinLength(3)]
        public string Profession { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public ICollection<Book>? Books { get; set; } 
         public ICollection<Message>? Messages { get; set; }
         public ICollection<Image>? Images { get; set; }
         public ICollection<Replay>? Replayes { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        public string? Role { get; set; }
      
    }
}
