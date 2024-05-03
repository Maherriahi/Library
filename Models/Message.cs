using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Full_name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Description { get; set; }
        public DateTime dateTime { get; set; }= DateTime.Now;

        [ForeignKey(nameof(studentMessage))]
        public int? StudentId { get; set; }
        public Student? studentMessage { get; set; }

        public ICollection<Replay>? Replayes { get; set; }

    }
}
