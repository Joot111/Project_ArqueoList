using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_ArqueoList.Data;
using Project_ArqueoList.Models;
using System.Diagnostics;

namespace Project_ArqueoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var artigos = await _context.Artigos.ToListAsync();

                if (artigos == null || !artigos.Any())
                {
                    ViewBag.Message = "No articles found.";
                }

                return View(artigos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching articles.");
                ViewBag.Message = "An error occurred while fetching articles.";
                return View(new List<Artigo>());
            }
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
