using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using adamsapp.Models;
using Microsoft.Data.SqlClient;

namespace adamsapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        SqlConnection connection = new SqlConnection("Server=tcp:adamsapp.database.windows.net,1433;Initial Catalog=adams-db;Persist Security Info=False;User ID=sqladmin;Password=nic3pass!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        SqlCommand cmd = new SqlCommand("SELECT TOP 1 test1 from test", connection);
        connection.Open();
        object x = cmd.ExecuteScalar();
        connection.Close();
        ViewData["Message"] = x;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
