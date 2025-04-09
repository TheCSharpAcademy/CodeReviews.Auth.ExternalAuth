using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.hasona23.Models;

namespace ProductManagement.hasona23.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        if (User.Identity != null)
        {
            _logger.LogCritical($"Access Denied for {User.Identity.Name}");
        }

        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}