using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFinalExam.Data;
using MyFinalExam.ViewModels;
using MyFinalExam.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace MyFinalExam.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopFixContext db;

        public ShopController(ShopFixContext context)
        {
            db = context;
        }
        public IActionResult Index(int? category)
        {
            var products = db.Products.AsQueryable();
            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }
            var result = products.Include(p => p.Category).Select(p => new ProductVM
            {
                ID = p.Id,
                Name = p.Name,
                Price = p.Price ?? 0,
                ImageURL = p.ImageUrl ?? "",
                CategoryName = p.Category.Name,
                DetailSale = p.Sale ?? 0,
            }).ToList();
            return View(result);
        }
        public IActionResult Search(string? query)
        {
            var products = db.Products.AsQueryable();
            if (query != null)
            {
                products = products.Where(p => p.Name.Contains(query));
            }
            var result = products.Include(p => p.Category).Select(p => new ProductVM
            {
                ID = p.Id,
                Name = p.Name,
                Price = p.Price ?? 0,
                ImageURL = p.ImageUrl ?? "",
                DetailSale = p.Sale ?? 0,

            }).ToList();
            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.Products
                .Include(p => p.Category)
                .SingleOrDefault(p => p.Id == id);
            if (data == null)
            {
                TempData["Message"] = $"Không có sản phẩm với ID: {id}";
                return Redirect("/404NotFound");
            }
            var similarProducts = db.Products
               .Where(p => p.CategoryId == data.CategoryId && p.Id != id)
               .Select(p => new ProductVM
               {
                   ID = p.Id,
                   Name = p.Name,
                   Price = p.Price ?? 0,
                   ImageURL = p.ImageUrl ?? ""
               })
               .ToList();
            var result = new ProductDetailVM
            {
                ID = id,
                Name = data.Name,
                CategoryId = data.CategoryId,
                CategoryName = data.Category?.Description ?? string.Empty,
                Price = data.Price,
                ImageURL = data.ImageUrl ?? string.Empty,
                CategoryDescription = data.Category?.Description ?? string.Empty,
                StockQuantity = data.StockQuantity,
                Sale = data.Sale,
                NumberRate = 5,
                SimilarProducts = similarProducts
            };
            return View(result);
        }

    }
}
