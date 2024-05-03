using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork;

namespace StudentClub.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private readonly IUnitOfWork _unitOfWork;
        public async Task<IActionResult> Index()
        {

            var book = await _unitOfWork.books.FindAllAsync();
            var section = await _unitOfWork.sections.FindAllAsync();
            var viewModel = new Tuple<IEnumerable<Book>, IEnumerable<Section>>(book, section);

            return View(viewModel);

        }
    
        //Details
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var books = await _unitOfWork.books.FindById(Id.Value);
            if (books == null)
            {
                return NotFound();


            }
            return View(books);
        }
        //Get New
        public IActionResult New()
        {
            createlist();
            return View();
        }
        //post New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    book.clientFile.CopyTo(stream);
                    book.imagePath = stream.ToArray();

                }
                _unitOfWork.books.AddOne(book);
                return RedirectToAction("Index");
            }
            else
            {
                createlist();
                return View(book);
            }
               

        }
        public void createlist(int selectId=1)
        {
            var sections = _unitOfWork.sections.FindAllAsync();
            SelectList listItems = new SelectList(sections.Result, "Id", "Name", selectId);
            ViewBag.BooksList=listItems;
        }
        //Get Edit
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var book =await _unitOfWork.books.FindById(Id.Value);
            if (book == null)
            {
                return NotFound();

            }
            createlist();
            return View(book);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    book.clientFile.CopyTo(stream);
                    book.imagePath = stream.ToArray();

                }
                _unitOfWork.books.UpdateOne(book);
                return RedirectToAction("Index");

            }
            else
            {
                createlist();
                return View(book);
            }
         
        }

        //Get Delete
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var book =await _unitOfWork.books.FindById(Id.Value);
            if (book == null)
            {
                return NotFound();

            }
            createlist();
            return View(book);
        }

        //post Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Book book)
        {
            _unitOfWork.books.DeleteOne(book);
            TempData["successData"] = "book has been deleted successfully";
            return RedirectToAction("Index");

        }



    }
}
