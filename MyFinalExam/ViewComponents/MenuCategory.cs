using Microsoft.AspNetCore.Mvc;
using MyFinalExam.Data;
using MyFinalExam.ViewModels;

namespace MyFinalExam.ViewComponents
{
    public class MenuCategory : ViewComponent
    {
        private readonly ShopFixContext db;
        public MenuCategory(ShopFixContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(lo => new MenuCategoryVM
            {
                Id = lo.Id,
                Name = lo.Name,
                Quantity = lo.Quantity
            }).OrderBy(p => p.Name);
            return View(data);
        }
    }
}
