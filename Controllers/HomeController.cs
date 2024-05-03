using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork;
using System.Diagnostics;

namespace StudentClub.Controllers
{
    
    public class HomeController : Controller
    {
      
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> user, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _user = user;
            _db = db;

        }
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _user;
		private readonly AppDbContext _db;



		public IActionResult Index()
        {

            
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Message message)
        {
            string userId = _user.GetUserId(User);
			var studentId = _db.Students.FirstOrDefault(x => x.UserId == userId);

			if (ModelState.IsValid  && studentId !=null)
            {
               
                  message.StudentId =studentId.Id ;
                _unitOfWork.messages.AddOne(message);
                return RedirectToAction("Index");
            }
            else

                 return RedirectToAction("New","Students");

        }

	
		public IActionResult Privacy()
        {
        
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
