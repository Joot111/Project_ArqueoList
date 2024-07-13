using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_ArqueoList.Data;
using Project_ArqueoList.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ArqueoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Carregar artigos validados do banco de dados
            var artigos = await _context.Artigos
                .Where(a => a.validado)
                .OrderByDescending(a => a.data_validacao)
                .ToListAsync();

            return View(artigos);
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
