using Microsoft.AspNetCore.Mvc;
using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;

namespace WebStoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name must not same displayOrder");
            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category create successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(item => item.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(item => item.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name must not same displayOrder");
            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category update successfully!";

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(item => item.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(item => item.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(item => item.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category delete successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
