using Microsoft.AspNetCore.Mvc;

namespace Riode_BackendProject.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
