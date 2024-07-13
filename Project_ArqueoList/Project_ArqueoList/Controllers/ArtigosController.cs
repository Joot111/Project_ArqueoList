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

        public ArtigosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
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
            // Obter o utilizador atual
            var userIdString = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userIdString))
            {
                return NotFound();
            }

            // Encontrar o Utilizador correspondente ao userIdString
            var utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.UserId == userIdString);

            if (utilizador == null)
            {
                return NotFound();
            }

            // Selecionar os artigos escritos pelo utilizador atual
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Titulo,Conteudo, Nome_Autor,validado,data_validacao,tipo,ID_Utilizador")] Artigo artigo, IFormFile Imagem)
        {
            // Vars auxiliares
            string nomeImagem = "";
            bool haImagem = false;

            // Há ficheiro?
            if (Imagem == null)
            {
                ModelState.AddModelError("", "O fornecimento de uma imagem é obrigatório!");
                return View(artigo);
            }
            else
            {
                // Há ficheiro, mas é imagem?
                if (!(Imagem.ContentType == "image/png" || Imagem.ContentType == "image/jpeg" || Imagem.ContentType == "image/jpg"))
                {
                    ModelState.AddModelError("", "Tem de fornecer um ficheiro PNG ou JPG para atribuir uma imagem!");
                    return View(artigo);
                }
                else
                {
                    // Há ficheiro, e é uma imagem válida
                    haImagem = true;
                    // Obter o nome a atribuir à imagem
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    // Obter a extensão do nome do ficheiro
                    string extensao = Path.GetExtension(Imagem.FileName);
                    // Adicionar a extensão ao nome da imagem
                    nomeImagem += extensao;
                    // Adicionar o nome do ficheiro ao objeto que vem do browser
                    artigo.Imagem = nomeImagem;
                }
            }

            ModelState.Remove("UtilArtigo");
            if (ModelState.IsValid)
            {
                _context.Add(artigo);
                await _context.SaveChangesAsync();

                if (haImagem)
                {
                    string nomePastaImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens");
                    if (!Directory.Exists(nomePastaImagem))
                    {
                        Directory.CreateDirectory(nomePastaImagem);
                    }
                    string caminhoFinalImagem = Path.Combine(nomePastaImagem, nomeImagem);
                    using var stream = new FileStream(caminhoFinalImagem, FileMode.Create);
                    await Imagem.CopyToAsync(stream);
                }

                return RedirectToAction(nameof(Index));
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
