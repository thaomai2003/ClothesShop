using Microsoft.AspNetCore.Mvc;

namespace MyFinalExam.Controllers
{
    public class About : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
