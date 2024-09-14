using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;
using WebStoreBook.Models.ViewModel;
using WebStoreBook.Utility;

namespace WebStoreBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    [BindProperties]
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
        [ActionName("Summary")]
        [HttpPost]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWord.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value, includeProperties: "Product");
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBaseOnQuantity(cart.Count, cart.Product.ListPrice, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal = (cart.Price * cart.Count);
            }
            _unitOfWord.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWord.Save();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWord.OrderDetail.Add(orderDetails);
                _unitOfWord.Save();
            }

            //Payment settings
            //var domain = "https://localhost:7265/";
            //var options = new SessionCreateOptions
            //{
            //    PaymentMethodTypes = new List<string>
            //    {
            //        "cart",
            //    },
            //    LineItems = new List<SessionLineItemOptions>(),
            //    Mode = "payment",
            //    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
            //    CancelUrl = domain + $"customer/cart/Index",
            //};

            //foreach (var item in ShoppingCartVM.ListCart)
            //{
            //    var sessionLineItem = new SessionLineItemOptions
            //    {
            //        PriceData = new SessionLineItemPriceDataOptions
            //        {
            //            UnitAmount = (long)(item.Price * 100),
            //            Currency = "usd",
            //            ProductData = new SessionLineItemPriceDataProductDataOptions
            //            {
            //                Name = item.Product.Title,
            //            },
            //        },
            //        Quantity = item.Count,
            //    };
            //    options.LineItems.Add(sessionLineItem);
            //}


            //var service = new SessionService();
            //Session session = service.Create(options);

            //Response.Headers.Add("Location", session.Url);
            //return new StatusCodeResult(303);

            _unitOfWord.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWord.Save();
            return RedirectToAction("Index", "Home");
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
