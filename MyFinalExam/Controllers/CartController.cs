using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFinalExam.Data;
using MyFinalExam.ViewModels;
using MyFinalExam.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace MyFinalExam.Controllers
{

    public class CartController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly ShopFixContext _context;
        public CartController(ShopFixContext context, PaypalClient paypalClient)
        {
            _context = context;
            _paypalClient = paypalClient;
        }
        //truy xuất dữ liệu của session
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(x => x.Id == id);
            if (item == null)
            {
                var product = _context.Products.SingleOrDefault(x => x.Id == id);
                if (product == null)
                {
                    TempData["Message"] = $"Not found product where id : {id}";
                    return Redirect("/404NotFound");
                }
                item = new CartItem
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    ProductSale = product.Sale ?? 0,
                    Quantity = quantity,
                    ProductImageURL = product.ImageUrl ?? string.Empty
                };
                cart.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCart(int id)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult CheckOut()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CheckOut(CheckOutVM model)
        {
            if (ModelState.IsValid)
            {
                var customerID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                var customer = new Customer();
                if (model.LikeCustomers)
                {
                    customer = _context.Customers.SingleOrDefault(ct => ct.Id == customerID);
                }
                var totalAmount = Cart.Sum(item => item.Quantity * item.ProductSale);
                var order = new Order
                {
                    CustomerId = customerID,
                    ReceiverName = model.Name ?? customer.Name,
                    ReceiverPhone = model.Phone ?? customer.Phone,
                    City = model.City ?? customer.City,
                    OrderDate = DateTime.Now,
                    Pay = "Cast On Delivery",
                    Email = model.Email ?? customer.Email,
                    PaymentId = 1,
                    ShippingAddress = model.Address ?? customer.Address,
                    Note = model.Note,
                    Status = "Success",
                    TotalAmount = totalAmount,
                    ShippingDate = new DateOnly(2024, 6, 13),
                    ReceiveDay = new DateOnly(2024, 6, 20),
                };
                _context.Database.BeginTransaction();
                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    var orderDetails = new List<OrderDetail>();
                    foreach (var item in Cart)
                    {
                        orderDetails.Add(new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.Id,
                            Quantity = item.Quantity,
                            UnitPrice = item.ProductPrice,
                        });
                    }
                    //thêm 1 đối tượng 
                    _context.AddRange(orderDetails);
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                    HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());
                    return View("Success");
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction(); 
                }

            }

            return View(Cart);
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View("Success");
        }
        #region Paypal payment

        [Authorize]
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken, CheckOutVM model)
        {
            var totalAmount = Cart.Sum(p => p.PriceTotal + 15).ToString();
            var currency = "USD";
            var referenceOrderId = "Cart" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalClient.CreateOrder(totalAmount, currency, referenceOrderId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        [Authorize]
        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken, CheckOutVM model)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                if (ModelState.IsValid)
                {
                    var customerID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
                    var customer = new Customer();
                    customer = _context.Customers.SingleOrDefault(ct => ct.Id == customerID);
                    var totalAmount = Cart.Sum(item => item.Quantity * item.ProductSale);
                    var order = new Order
                    {
                        CustomerId = customerID,
                        ReceiverName = model.Name ?? customer.Name,
                        ReceiverPhone = model.Phone ?? customer.Phone,
                        City = model.City ?? customer.City,
                        OrderDate = DateTime.Now,
                        Pay = "Paypal",
                        Email = model.Email ?? customer.Email,
                        PaymentId = 2,
                        ShippingAddress = model.Address ?? customer.Address,
                        Status = "Success",
                        TotalAmount = totalAmount,
                        ShippingDate = new DateOnly(2024, 6, 13),
                        ReceiveDay = new DateOnly(2024, 6, 20),
                    };
                    _context.Database.BeginTransaction();
                    try
                    {
                        _context.Orders.Add(order);
                        _context.SaveChanges();
                        var orderDetails = new List<OrderDetail>();
                        foreach (var item in Cart)
                        {
                            orderDetails.Add(new OrderDetail
                            {
                                OrderId = order.Id,
                                ProductId = item.Id,
                                Quantity = item.Quantity,
                                UnitPrice = item.ProductPrice,
                            });
                        }
                        _context.AddRange(orderDetails);
                        _context.SaveChanges();
                        _context.Database.CommitTransaction();
                        HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());
                        return View("Success");
                    }
                    catch (Exception ex)
                    {
                        _context.Database.RollbackTransaction();
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion
    }
}