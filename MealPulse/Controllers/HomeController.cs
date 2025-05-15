using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using Web.ViewModels;


namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult TestDbConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    ViewBag.Message = "Connection to the database was successful!";
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = "Connection to the database failed: " + ex.Message;
            }

            return View();
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var quotes = new List<(string Text, string Author)>();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "quotes.txt");
            if (System.IO.File.Exists(filePath))
            {
                foreach (var line in System.IO.File.ReadAllLines(filePath, Encoding.UTF8))

                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        quotes.Add((parts[0].Trim('"'), parts[1]));
                    }
                }
            }

            ViewBag.Quotes = quotes;
            return View("Landing");
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
}
