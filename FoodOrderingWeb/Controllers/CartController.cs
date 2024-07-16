using FoodOrderingWeb.DataAccess;
using FoodOrderingWeb.Extensions;
using FoodOrderingWeb.Models;
using FoodOrderingWeb.Models.Enum;
using FoodOrderingWeb.Repository.Interface;
using FoodOrderingWeb.Services;
using FoodOrderingWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
	{

		private readonly Interface_CartRepository _cartRepository;
		private readonly Interface_OrderRepository _orderRepository;
		private readonly Interface_FoodItemRepository _foodItemRepository;
		private readonly Interface_RestaurantRepository _restaurantRepository;
        private readonly Interface_OrderDetailsRepository _orderDetailsRepository;
		private readonly ApplicationDatabaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IVnPayService _vnPayService;
		public CartController(Interface_CartRepository cartRepository, Interface_OrderRepository orderRepository,UserManager<User> userManager,IVnPayService vnPayService,Interface_OrderDetailsRepository orderDetailsRepository,
			Interface_FoodItemRepository foodItemRepository , Interface_RestaurantRepository restaurantRepository, ApplicationDatabaseContext context)
        {
            _cartRepository = cartRepository;
            _orderDetailsRepository = orderDetailsRepository;
			_context = context;
            _userManager = userManager;
            _vnPayService = vnPayService;
            _orderRepository = orderRepository;
            _foodItemRepository = foodItemRepository;
            _restaurantRepository = restaurantRepository;
        }

        public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> AddToCart(int id,int quantity)
        {
            var food = await _foodItemRepository.GetByIdAsync(id);
            if (food != null)
            {
                var cartDetail = new CartDetail
                {
                    FoodName = food.FoodName != null ? food.FoodName : "",
                    Quantity = quantity,
                    FoodItemId = id,
                    CategoryDescription = food.Category.Name,
                    Price = food.FoodPrice
                };
                var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();

                cart.AddItem(cartDetail);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                return RedirectToAction("Index");
            }
            else
            {
                throw new FoodNotFoundException($"Food with id {id} not found");
            }
        }

		public IActionResult Checkout() 
		{
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            var cartVM = new CartOrderVM
            {
                Cart = cart,
                Order = new Order()
            };
            return View(cartVM);
		}
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order,string payment = "Thanh toán bằng VnPay")
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index"); // Quay lại trang chủ nếu giỏ hàng trống
            }
            var user = await _userManager.GetUserAsync(User);

            if(payment == "Thanh toán bằng VnPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Price = (double)cart.Items.Sum(x => x.Price),
                    CreatedDate = DateTime.Now,
                    Description = $"{order.UserId}", // Mô tả chứa UserId
                    FullName = user.FullName, // Tên đầy đủ của người dùng
                    OrderId = new Random().Next(1000, 10000) // Tạo mã đơn hàng ngẫu nhiên
                };  
                

                double totalPrice = cart.Items.Sum(item => item.Price);

                // Tạo Order entity
                order.UserId = user.Id;
                order.OrderDate = DateTime.UtcNow;
                order.TotalAmount = totalPrice;
                order.Status = Models.Enum.OrderStatus.Pending;
                if (!string.IsNullOrEmpty(user.DefaultAddress))
                {
                    order.ShippingAddress = user.DefaultAddress;
                }
                else
                {
                    order.ShippingAddress = ""; // Để trống nếu không có địa chỉ mặc định
                }

                order.OrderDetails = cart.Items.Select(item => new OrderDetail
                {
                    FoodItemId = item.FoodItemId,
                    Price = item.Price*item.Quantity,
                    Quantity = item.Quantity,
                    FoodImage = item.FoodImage,
                    
                }).ToList();

                // Tạo URL thanh toán
                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);

                // Lưu trữ thông tin tạm thời của chi tiết đơn hàng và đơn hàng vào Session 
                HttpContext.Session.SetObjectAsJson("PendingOrder", order);

                return Redirect(paymentUrl);

            }
            return View("PaymentFail");
        }


        [Authorize]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            // Lấy lại thông tin từ Session
            var pendingOrder = HttpContext.Session.GetObjectFromJson<Order>("PendingOrder");

            if (pendingOrder == null)
            {
                TempData["Message"] = "Không thể xác thực thông tin thanh toán.";
                return RedirectToAction("PaymentFail");
            }

            // Bắt đầu giao dịch cơ sở dữ liệu
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Cập nhật trạng thái đơn hàng
                    pendingOrder.Status = OrderStatus.Delivered; // Sử dụng enum
                    _context.Orders.Update(pendingOrder);
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    // Xóa thông tin tạm thời khỏi Session
                    HttpContext.Session.Remove("PendingOrder");

                    // Thông báo thanh toán thành công
                    TempData["MessageSuccess"] = $"Thanh toán VNPAY thành công: {response.VnPayResponseCode}";
                    TempData["OrderId"] = response.OrderId;

                    return View("PaymentSuccess");
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    await transaction.RollbackAsync();
                    TempData["Message"] = "Có lỗi xảy ra trong quá trình xử lý giao dịch.";
                    return RedirectToAction("PaymentFail");
                }
            }
        }

        public IActionResult OrderCompleted()
        {
            return View();
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart =
           HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [Authorize(Roles = Role.Role_Customer)]
        public async Task<IActionResult> ViewingOrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }
            var orders = await _context.Orders.Include(i => i.OrderDetails).Where(o => o.UserId == user.Id).ToListAsync();
            return View(orders);
        }
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View();
        }


        public IActionResult Increase(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            cart.IncreaseQuantity(id);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Decrease(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            cart.DecreaseQuantity(id);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction(nameof(Index));
        }
    }
}
