using Microsoft.AspNetCore.Mvc;

namespace MyFinalExam.Controllers
{
    public class Contact : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
