using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }

        [ForeignKey(nameof(StudentBook))]
        public int? studentId { get; set; }
        public Student? StudentBook { get; set; }

        [ForeignKey(nameof(SectionBook))]
        public int SectionId { get; set; }
        public Section? SectionBook { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public byte[]? imagePath { get; set; }

        [NotMapped] //maysama3ich fi database
        public IFormFile clientFile { get; set; }

        public string? imageSrc
        {
            get
            {
                if (imagePath != null)
                {
                    string base64String = Convert.ToBase64String(imagePath, 0, imagePath.Length);
                    return "data:image/jpg;base64," + base64String;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


    }
}
