using AutoMapper;
using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinalExam.Data;
using MyFinalExam.Helpers;
using MyFinalExam.ViewModels;
using System.Security.Claims;
namespace MyFinalExam.Controllers
{

    public class CustomerController : Controller
    {
        private readonly ShopFixContext _context;
        private readonly IMapper _mapper;
        public CustomerController(ShopFixContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = _mapper.Map<Customer>(model);
                    customer.RandomKey = MyUtil.GenerateRandomKey();
                    customer.Password = model.Password.ToMd5Hash(customer.RandomKey);
                    customer.Effect = true;
                    customer.Role = 0;
                    if (Image != null)
                    {
                        try
                        {
                            customer.Image = MyUtil.UploadImage(Image, "Customer");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error uploading image: " + ex.Message);
                            ModelState.AddModelError("", "Error Image");
                        }
                    }
                    _context.Add(customer);
                    _context.SaveChanges();
                    Console.WriteLine("Customer saved to database");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "An error occurred during registration. Please try again!");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(model);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.SingleOrDefault(ct => ct.Id == model.Id);
                if (customer == null)
                {
                    ModelState.AddModelError("ERROR", "No Customer Information !");
                }
                else
                {
                    if (customer.Role == 1)
                    {
                        ModelState.AddModelError("ERROR", "Access denied for Admin");
                    }
                    else if (!customer.Effect)
                    {
                        ModelState.AddModelError("ERROR", "This account has been locked. Please contact Admin!");
                    }
                    else
                    {
                        if (customer.Password != model.passwordLogin.ToMd5Hash(customer.RandomKey))
                        {
                            ModelState.AddModelError("ERROR", "Wrong account or password!");
                        }
                        else
                        {
                           //lưu trữ thông tin khi người dùng đăng nhập thành công 
                           var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, customer.Email),
                                new Claim(ClaimTypes.Name, customer.Name),
                                new Claim("Address", customer.Address),
                                new Claim(ClaimTypes.Role, "Customer"),
                                new Claim(MySetting.CLAIM_CUSTOMERID, customer.Id.ToString())
                            };
                                //danh tính người dùng 
                                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                await HttpContext.SignInAsync(claimsPrincipal);
                                if (Url.IsLocalUrl(ReturnURL))
                                {
                                    return Redirect(ReturnURL);
                                }
                                else
                                {
                                    return Redirect("/");
                                }
                        }
                    }
                }
            }
            return View();
        }
        #endregion
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}

