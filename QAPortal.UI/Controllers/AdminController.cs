using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Questions()
    {
        return View();
    }

    public IActionResult EditQuestion(int id)
    {
        ViewBag.QuestionId = id;
        return View();
    }

    public IActionResult CreateQuestion()
    {
        return View();
    }
    public IActionResult Users()
    {
        return View();
    }

}
