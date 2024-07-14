using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_ArqueoList.Data;
using Project_ArqueoList.Models;

namespace Project_ArqueoList.Controllers
{
    [Authorize]
    public class ArtigosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ArtigosController> _logger;

        public ArtigosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager, ILogger<ArtigosController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Artigos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var artigosValidados = _context.Artigos.Where(a => a.validado);
            return View(await artigosValidados.ToListAsync());
        }

        // GET: Artigos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artigo = await _context.Artigos
                .FirstOrDefaultAsync(m => m.ID == id
                && m.validado);

            if (artigo == null)
            {
                return NotFound();
            }

            return View(artigo);
        }

        // GET: Artigos/Pessoais
        public async Task<IActionResult> Pessoais()
        {
            var userIdString = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userIdString))
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.UserId == userIdString);

            if (utilizador == null)
            {
                return NotFound();
            }

            var artigosPessoais = _context.Artigos
                .Where(a => a.ID_Utilizador == utilizador.idUtilizador);

            return View(await artigosPessoais.ToListAsync());
        }

        // GET: Artigos/Create
        public IActionResult Create()
        {
            ViewData["ID_Utilizador"] = new SelectList(_context.Utilizador, "idUtilizador", "Username");
            var artigo = new Artigo();
            return View(artigo);
        }

        // POST: Artigos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Titulo,Conteudo,Nome_Autor,validado,data_validacao,tipo,ID_Utilizador")] Artigo artigo, IFormFile Imagem)
        {
            try
            {
                if (Imagem == null)
                {
                    ModelState.AddModelError("", "O fornecimento de uma imagem é obrigatório!");
                    return View(artigo);
                }
                else if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg" || Imagem.ContentType == "image/jpg"))
                {
                    ModelState.AddModelError("", "Tem de fornecer um ficheiro PNG ou JPG para atribuir uma imagem!");
                    return View(artigo);
                }
                else
                {
                    string nomeImagem = Guid.NewGuid().ToString() + Path.GetExtension(Imagem.FileName);
                    artigo.Imagem = nomeImagem;

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", nomeImagem);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagem.CopyToAsync(stream);
                    }
                }

                artigo.data_validacao = DateTime.Now;

                ModelState.Remove("UtilArtigo");
                if (ModelState.IsValid)
                {
                    _context.Add(artigo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o artigo");
                ModelState.AddModelError("", "Ocorreu um erro ao tentar criar o artigo. Tente novamente.");
            }

            ViewData["ID_Utilizador"] = new SelectList(_context.Utilizador, "idUtilizador", "Username", artigo.ID_Utilizador);
            return View(artigo);
        }

        // GET: Artigos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artigo = await _context.Artigos.FindAsync(id);
            if (artigo == null)
            {
                return NotFound();
            }
            ViewData["ID_Utilizador"] = new SelectList(_context.Utilizador, "idUtilizador", "Username");
            return View(artigo);
        }

        // POST: Artigos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Titulo,Conteudo,Imagem,Nome_Autor,validado,data_validacao,tipo,ID_Utilizador")] Artigo artigo)
        {
            if (id != artigo.ID)
            {
                return NotFound();
            }

            ModelState.Remove("UtilArtigo");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artigo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtigoExists(artigo.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Utilizador"] = new SelectList(_context.Utilizador, "idUtilizador", "Username");
            return View(artigo);
        }

        // GET: Artigos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artigo = await _context.Artigos
                .Include(a => a.UtilArtigo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (artigo == null)
            {
                return NotFound();
            }

            return View(artigo);
        }

        // POST: Artigos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artigo = await _context.Artigos.FindAsync(id);
            if (artigo != null)
            {
                _context.Artigos.Remove(artigo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtigoExists(int id)
        {
            return _context.Artigos.Any(e => e.ID == id);
        }
    }
}
