using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentClub.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _user;
        public CommentsController(IUnitOfWork unitOfWork, UserManager<IdentityUser> user, AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            string userId = _user.GetUserId(User);
            var studId = _db.Students.FirstOrDefault(x => x.UserId == userId);
            var image = await _unitOfWork.images.FindAllAsync();
            if (studId != null) 
            {
                if (studId.Role == "Admin")
                {

                    IEnumerable<Replay> replay = null;
                    var message = await _unitOfWork.messages.FindAllAsync();
                    var viewModel = new Tuple<IEnumerable<Message>, IEnumerable<Replay>, IEnumerable<Image>>(message, replay,image);
                    return View(viewModel);
                }

                else
                {
                    IEnumerable<Message> message = null;
                    var replay = _db.Replayes.Where(m => m.StudentId == studId.Id);
                    var viewModel = new Tuple<IEnumerable<Message>, IEnumerable<Replay>, IEnumerable<Image>>(message, replay,image);
                    return View(viewModel);
                }

            }
            else 
            { 
                return RedirectToAction("New", "Students");
            }
           
        }

        public async Task<IActionResult> ReplayForMessage(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var message = await _unitOfWork.messages.FindById(Id.Value);
            var image=_db.Images.FirstOrDefault(x => x.studentId ==  message.StudentId);
            if (message == null)
            {
                return NotFound();
            }

            var viewModel = new MessageReplayViewModel
            {
                Message = message,
                Replay = new Replay(), // Initialize the Replay object here if needed
                Image= image
            };

            return View(viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReplayForMessage(MessageReplayViewModel viewModel)
        {
            var admin = _db.Students.FirstOrDefault(x => x.Role == "Admin");
            if (admin != null)
            {
            var replay = viewModel.Replay;
            replay.dateTime = DateTime.Now;
            replay.Email = admin.Email;
            replay.Phone = admin.Telephone;
            replay.Full_name = admin.FirstName+" "+admin.LastName;

            _unitOfWork.replayes.AddOne(replay);

            return RedirectToAction("Index"); // Redirect wherever you want after successful submission
            }
            else
            {
                // Redirect to ReplayForMessage action with an error message
                return RedirectToAction("ReplayForMessage", "Comments");
            }

        }
    }
}
    

