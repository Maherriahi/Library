using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
    public class Replay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Full_name { get; set; }
		[EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
		public string? Subject { get; set; }
		[Required]
        [MaxLength(2000),MinLength(50)]
        public string? Description { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;

        [ForeignKey(nameof(messageRepaly))]
        public int? messageId { get; set; }
        public Message? messageRepaly { get; set; }

        [ForeignKey(nameof(studRepaly))]
        public int StudentId { get; set; }
        public Student? studRepaly { get; set; }
    }
}
