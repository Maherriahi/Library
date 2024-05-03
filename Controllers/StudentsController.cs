using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork;

namespace StudentClub.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        public StudentsController(IUnitOfWork unitOfWork,UserManager<IdentityUser> user,AppDbContext db)
        {
            _unitOfWork = unitOfWork;
            _users= user;
            _db= db;

        }
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _users;
        private readonly AppDbContext _db;

        [Authorize(Roles = Role.roleAdmin)]
        public async Task<IActionResult> Index(string search)
        {
            ViewData["CurrentFilter"] = search;
            
            if (!String.IsNullOrEmpty(search))
            {
                var students = _db.Students.Where(s => 
                s.FirstName.Contains(search)||
                s.LastName.Contains(search) ||
                s.Profession.Contains(search)
                ).ToList();
                return View(students);
            }
            var image = await _unitOfWork.images.FindAllAsync();
            var student = await _unitOfWork.students.FindAllAsync();
            var viewModel = new Tuple<IEnumerable<Student>, IEnumerable<Image>>(student, image);
            return View(viewModel);
        }



        //Get New
        public IActionResult New()
        {

            return View();
        }

        //post New
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Student student)
        {
            string Id = _users.GetUserId(User);
            var IsUser = _db.Students.Any(x => x.UserId == Id);


            if (ModelState.IsValid && IsUser==false)
            {
                student.CreateDate.ToString("dd/MM/yy");
                student.UserId = _users.GetUserId(User);
                _unitOfWork.students.AddOne(student);
                return RedirectToAction("Index");
            }
            else 
            {
                Console.Error.Write("error save new data !");
                return RedirectToAction("Edit", "Students");
            }
                
        }

        //Get Edit
        public async Task <IActionResult> Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var student =await  _unitOfWork.students.FindById(Id.Value);
            if (student == null)
            {
                return NotFound();


            }
            return View(student);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.students.UpdateOne(student);
                return RedirectToAction("Index");

            }
            else
                return View(student);
        }

        //Get Delete
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();

            }
            var student =await _unitOfWork.students.FindById(Id.Value);
            if (student == null)
            {
                return NotFound();


            }
            return View(student);
        }

        //post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Student student)
        {
            _unitOfWork.students.DeleteOne(student);
            TempData["successData"] = "student has been deleted successfully";
            return RedirectToAction("Index");

        }

        //Details
        public async Task <IActionResult> Details(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var student = await _unitOfWork.students.FindById(Id.Value);
            if (student == null)
            {
                return NotFound();


            }
			var viewModel = new ViewModelStudentReplay
			{
				Student = student,
				Replay = new Replay(), // Initialize the Replay object here if needed
			};
            return View(viewModel);
        }
		


		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ViewModelStudentReplay viewModel)
        {
            var admin = _db.Students.FirstOrDefault(x => x.Role == "Admin");
            if (admin != null)
            {
                var replay = viewModel.Replay;
                replay.dateTime = DateTime.Now;
                replay.Email = admin.Email;
                replay.Phone = admin.Telephone;
                replay.Full_name = admin.FirstName + " " + admin.LastName;

                _unitOfWork.replayes.AddOne(replay);

                return RedirectToAction("Index"); // Redirect wherever you want after successful submission
            }
            else
            {
                // Redirect to ReplayForMessage action with an error message
                return RedirectToAction("Details", "Students");
            }

        }







    }
}
