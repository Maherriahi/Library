using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentClub.Models;
using StudentClub.UnitOfWork;

namespace StudentClub.Controllers
{
    [Authorize]
    public class SectionsController : Controller
    {
        public SectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private readonly IUnitOfWork _unitOfWork;

        public async Task<IActionResult> Index()
        {

            var allCat = await _unitOfWork.sections.FindAllAsync();

            return View(allCat);
        }
        //Get New
        public IActionResult New()
        {
            return View();
        }

        //post New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Section section)
        {
            if (ModelState.IsValid)
            {
                if (section.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    section.clientFile.CopyTo(stream);
                    section.imagePath = stream.ToArray();

                }
                _unitOfWork.sections.AddOne(section);
                return RedirectToAction("Index");
            }
            else
                return View(section);
        }

        //Get Edit
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var section =await _unitOfWork.sections.FindById(Id.Value);
            if (section == null)
            {
                return NotFound();

            }
            return View(section);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Section section)
        {
            if (ModelState.IsValid)
            {
				if (section.clientFile != null)
				{
					MemoryStream stream = new MemoryStream();
					section.clientFile.CopyTo(stream);
					section.imagePath = stream.ToArray();

				}
				_unitOfWork.sections.UpdateOne(section);
                return RedirectToAction("Index");

            }
            else
                return View(section);
        }

        //Get Delete
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var section =await _unitOfWork.sections.FindById(Id.Value);
            if (section == null)
            {
                return NotFound();

            }
            return View(section);
        }

        //post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Section section)
        {
            _unitOfWork.sections.DeleteOne(section);
            TempData["successData"] = "section has been deleted successfully";
            return RedirectToAction("Index");

        }







    }
}
