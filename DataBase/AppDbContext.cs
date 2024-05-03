using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentClub.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;


namespace StudentClub.DataBase
{

    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Replay> Replayes { get; set; }

        byte [] css = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\css.jpg"));
         byte [] js = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\js.jpg"));
         byte [] xml = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\xml.jpg"));
         byte [] nodejs = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\nodejs.jpg"));
        byte[] historical = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\pyramid.jpg"));
        byte[] literary = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\literary.webp"));
        byte[] scientific = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "B:\\source\\repos\\StudentClub\\StudentClub\\wwwroot\\image\\sky-earth.jpg"));

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
     
                optionsBuilder.EnableSensitiveDataLogging(); 
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var idUser = Guid.NewGuid().ToString();
            var idRole = Guid.NewGuid().ToString();

            modelBuilder.Entity<Student>().HasData(
             new Student() { Id = 1,LastName="Alex",FirstName="Rio",Email="rio.alex@rio.fr",Address= "Via del Corso, 123", Telephone="+0012365478",City="roma",Country="italy",PostalCode=89751,CreateDate=DateTime.Now,Profession="Student"},
              new Student() { Id = 2,LastName="Lio",FirstName="Renar",Email="Lio.renar@rio.fr",Address= "1 Rue de Rivoli", Telephone="+0078965478", City = "nice", Country = "france", PostalCode = 14512, CreateDate = DateTime.Now, Profession = "Student" ,Role="Admin",UserId= idUser },
              new Student() { Id = 3,LastName="Alexender",FirstName="jenifer",Email="Alex.jenifer@rio.fr",Address= "2718 N College Ave", Telephone=" + 0096365478", City = "maxico", Country = "USA", PostalCode = 63512, CreateDate = DateTime.Now, Profession = "Developer" },
              new Student() {Id = 4, LastName = "Alebrto", FirstName = "reval", Email = "Albert.reval@rio.fr", Address = "7485 Rush River Dr #700", Telephone = "+0085265478", City = "londan", Country = "Uk", PostalCode = 96312, CreateDate = DateTime.Now, Profession = "Scientist" }    
                );
            modelBuilder.Entity<Section>().HasData(
                new Section() { Id=1,Name= "historical",Description= "Exploring the intricate tapestry of ancient civilizations, this book unveils the untold stories that shaped the course of history" ,imagePath=historical},
                new Section() { Id=2,Name= "literary ",Description= "Exploring the vast landscape of literature, this collection unveils the hidden stories that have shaped our understanding of the world.", imagePath=literary },
                new Section() { Id=3,Name= "scientific",Description = "Unveiling the secrets of science, this study illuminates the pathways that have shaped our understanding of the world", imagePath=scientific }
                );
                        modelBuilder.Entity<Book>().HasData(
             new Book() { 
                 Id = 1, Author = "Marc Rodriguez", CreateDate = DateTime.Now, Title = "Mastering CSS Essentials: A Guide", Description = "CSS is a styling language that enhances the presentation of web documents by controlling the layout, design, and visual aspects such as colors, fonts, and spacing.", studentId=1,SectionId=1 ,imagePath=css},
             new Book() { Id = 2, Author = "Ophelia Feest", CreateDate = DateTime.Now, Title = "JavaScript Journey: A Comprehensive Guide", Description = "JavaScript is a high-level programming language used primarily for creating dynamic and interactive content on web pages, ranging from simple functions like form validation to complex web applications", studentId = 2, SectionId = 2,imagePath=js },
             new Book() { Id = 3, Author = "Mariano Harber", CreateDate = DateTime.Now, Title = "XML Unleashed: Exploring Data Markup", Description = "XML, or Extensible Markup Language, is a versatile markup language designed to store and transport data in a readable and structured format, commonly used for representing hierarchical data in a platform-independent manner", studentId = 3, SectionId = 3,imagePath=xml },
             new Book() { Id = 4, Author = "Garrison Rippin MD", CreateDate = DateTime.Now, Title = "Node.js Essentials: Unleashing Power",  Description = "Node.js is a server-side JavaScript runtime environment that enables developers to build scalable, networked applications by executing JavaScript code outside the browser, utilizing event-driven, non-blocking I/O operations for efficient handling of concurrent requests.", studentId = 4, SectionId = 3,imagePath=nodejs }

               );
           modelBuilder.Entity<Message>().HasData(
           new Message()
           {
               Id = 1,
               StudentId = 1,
               Full_name = "Alex rio",
               Email = "Alex.rio@yahoo.fr",
               dateTime = DateTime.Now,
               Phone = "+98522558877",
               Description = "Admin, we're excited to share that we have a historical book available. It's titled 'Example: The Book' and is ready for exploration in our collection. Feel free to take a look and let us know if you have any questions!",
           }
             );

            // Customizations to hash password
            var passwordHasher = new PasswordHasher<IdentityUser>();
            string hashedPassword = passwordHasher.HashPassword(null, "Admin@123");
            modelBuilder.Entity<IdentityUser>().HasData(
             new IdentityUser() { Id = idUser,  UserName= "admin@gmail.com",NormalizedUserName= "ADMIN@GMAIL.COM", NormalizedEmail="ADMIN@GMAIL.COM", Email = "admin@gmail.com", PhoneNumber = "22554477" ,EmailConfirmed=true,PasswordHash=hashedPassword}
               );



            modelBuilder.Entity<Replay>().HasData(
         new Replay()
         {
             Id = 1,
             messageId=1,
             StudentId=1,
             Full_name = "Alex rio",
             Email = "Alex.rio@yahoo.fr",
             dateTime = DateTime.Now,
             Phone = "+98522558877",
             Description = "Hi Alex,\r\n\r\nWe are delighted to inform you that we have an intriguing addition to our collection—a historical book titled 'Example: The Book'. It's ready for exploration, and we encourage you to take a closer look at your earliest convenience. Should you have any inquiries or require further assistance, please don't hesitate to reach out to us. Happy reading!\"\r\n\r\nFeel free to adjust the wording to better fit your communication style or the tone of your organization.",
         }
           );


            modelBuilder.Entity<IdentityRole>().HasData(

              new IdentityRole()
              {
                  Id = idRole,
                  Name = "Admin",
                  NormalizedName = "admin",
                  ConcurrencyStamp = Guid.NewGuid().ToString(),
              },

           new IdentityRole()
           {
               Id = Guid.NewGuid().ToString(),
               Name = "User",
               NormalizedName = "user",
               ConcurrencyStamp = Guid.NewGuid().ToString(),
           }
           );


            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
       new IdentityUserRole<string>
       {
           RoleId = idRole,
           UserId = idUser
       }
   );


            base.OnModelCreating(modelBuilder);
        }
    }
}
