using Microsoft.AspNetCore.Mvc;

namespace QAPortal.MVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
