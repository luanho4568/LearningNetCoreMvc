using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models.ViewModel;

namespace WebStoreBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWord;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWord = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWord.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWord.ApplicationUser.GetFirstOrDefault(
                    x => x.Id == claim.Value
                );
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal = (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWord.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.CartTotal += (cart.Count * cart.Price);
            }
            return View(ShoppingCartVM);
            return View();
        }
        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWord.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWord.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWord.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWord.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWord.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWord.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWord.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWord.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            _unitOfWord.ShoppingCart.Remove(cart);
            _unitOfWord.Save();
            return RedirectToAction(nameof(Index));
        }
        private double GetPriceBaseOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else
            {
                if (quantity <= 100)
                {
                    return price50;
                }
                return price100;
            }
        }
    }
}
