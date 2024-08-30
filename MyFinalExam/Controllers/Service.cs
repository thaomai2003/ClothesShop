using Microsoft.AspNetCore.Mvc;

namespace MyFinalExam.Controllers
{
    public class Service : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
