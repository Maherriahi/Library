using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Required] 
        [MaxLength(20)]
        public string Name { get; set; }
		[Required]
		[MaxLength(145)]
		public string Description { get; set; }
        public byte[]? imagePath { get; set; }

        [NotMapped] //maysama3ich fi database
        public IFormFile clientFile { get; set; }
        public ICollection<Book>? Books { get; set; }

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
