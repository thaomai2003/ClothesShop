using Microsoft.AspNetCore.Mvc;
using MyFinalExam.Data;
using Microsoft.EntityFrameworkCore;
using MyFinalExam.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
namespace MyFinalExam.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ShopFixContext db;
        public ProductController(ShopFixContext context)
        {
            db = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var products = await db.Products
                                   .Include(p => p.Category)
                                   .OrderByDescending(p => p.Id)
                                   .ToListAsync();
            var productViewModels = products.Select(p => new ProductViewModel
            {
                ID = p.Id,
                Name = p.Name,
                Price = p.Price ?? 0,
                ImageURL = p.ImageUrl ?? "",
                CategoryName = p.Category.Name,
                DetailSale = p.Sale ?? 0,
            }).ToList();
            return View(productViewModels);
        }
    }
}
