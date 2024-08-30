using Microsoft.AspNetCore.Mvc;

namespace MyFinalExam.Controllers
{
    public class Blog : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
