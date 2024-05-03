using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClub.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set; }
        public byte[]? imagePath { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;

        [ForeignKey(nameof(studentProfile))]
		public int? studentId { get; set; }
		public Student? studentProfile { get; set; }

		[NotMapped]
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
