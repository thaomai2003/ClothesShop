using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinalExam.Areas.Admin.Models.ViewModels;
using MyFinalExam.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MyFinalExam.Areas.Admin.HelperAdmin;
using System.Data.Entity;
namespace MyFinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerAdminController : Controller
    {
        private readonly ShopFixContext _context;
        private readonly IMapper _mapper;
        public CustomerAdminController(ShopFixContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        [Area("Admin")]
        public IActionResult AdminRegister(RegisterViewModel model, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = _mapper.Map<Customer>(model);
                    customer.RandomKey = MyUtil.GenerateRamdomKey();
                    customer.Password = model.Password.ToMd5Hash(customer.RandomKey);
                    customer.Effect = true;
                    customer.Role = 1; // Admin role
                    if (Image != null)
                    {
                        try
                        {
                            customer.Image = MyUtil.UploadImage(Image, "Customer");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Error uploading image: " + ex.Message);
                            return View(model);
                        }
                    }
                    _context.Add(customer);
                    _context.SaveChanges();
                    return Redirect("/Admin/CustomerAdmin/AdminLogin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred during registration. Please try again!");
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
            return View(model);
        }
        [Area("Admin")]
        [HttpGet]
        public IActionResult AdminLogin(string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult AdminLogin(LoginViewModel model, string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.SingleOrDefault(ct => ct.Id == model.Id);
                if (customer == null)
                {
                    ModelState.AddModelError("ERROR", "Invalid login attempt.");
                }
                else
                {
                    if (!customer.Effect)
                    {
                        ModelState.AddModelError("ERROR", "This account has been locked. Please contact Admin!");
                    }
                    else
                    {
                        if (customer.Password != model.passwordLogin.ToMd5Hash(customer.RandomKey))
                        {
                            ModelState.AddModelError("ERROR", "Invalid login attempt.");
                        }
                        else
                        {
                            if (customer.Role == 0)
                            {
                                ModelState.AddModelError("ERROR", "You are not authorized to access this area!");
                            }
                            else
                            {
                                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, customer.Email),
                            new Claim(ClaimTypes.Name, customer.Name),
                            new Claim(ClaimTypes.StreetAddress, customer.Address),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(MySetting.CLAIM_CUSTOMERID, customer.Id.ToString())
                        };

                                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                HttpContext.SignInAsync(claimsPrincipal);

                                if (Url.IsLocalUrl(ReturnURL))
                                {
                                    return Redirect(ReturnURL);
                                }
                                else
                                {
                                    return Redirect("/Admin");
                                }
                            }
                        }
                    }
                }
            }
            return View(model); // Return model to re-render the form with validation messages
        }

    }
}
