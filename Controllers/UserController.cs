using Microsoft.AspNetCore.Mvc;

namespace project.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
