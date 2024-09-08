using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;
using WebStoreBook.Models.ViewModel;

namespace WebStoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM();
            productVM.product = new Product();
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }
                );
            productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }
                );
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            }
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (!ModelState.IsValid) return View(obj);
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);
                if (obj.product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.product.ImageUrl = @"images\products\" + fileName + extension;
            }
            if (obj.product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.product);
                _unitOfWork.Save();
                TempData["Success"] = "Product create successfully!";

            }
            else
            {
                _unitOfWork.Product.Update(obj.product);
                _unitOfWork.Save();
                TempData["Success"] = "Product update successfully!";
            }
            return RedirectToAction("Index");

        }
        #region API_CALLS
        [HttpGet()]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (obj == null) return Json(new { success = false, message = "Xoá thất bại" });
            if (obj.ImageUrl != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var oldImageUrl = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImageUrl))
                {
                    System.IO.File.Delete(oldImageUrl);
                }
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Xoá thành công!" });
        }
        #endregion

    }
}
