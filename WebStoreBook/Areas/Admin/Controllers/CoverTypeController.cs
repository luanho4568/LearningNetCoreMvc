using Microsoft.AspNetCore.Mvc;
using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;

namespace WebStoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (!ModelState.IsValid) return View(obj);
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Cover Type create successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (coverTypeFromDb == null) return NotFound();
            return View(coverTypeFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (!ModelState.IsValid) return View(obj);
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Cover Type update successfully!";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (coverTypeFromDb == null) return NotFound();
            return View(coverTypeFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (obj == null) return NotFound();
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Cover Type delete successfully!";
            return RedirectToAction("Index");
        }
    }
}
