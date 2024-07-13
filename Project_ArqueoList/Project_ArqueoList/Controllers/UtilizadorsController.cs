using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_ArqueoList.Data;
using Project_ArqueoList.Models;

namespace Project_ArqueoList.Controllers
{
    public class UtilizadorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizadors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizador.ToListAsync());
        }

        // GET: Utilizadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(m => m.idUtilizador == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // GET: Utilizadors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UtilizadorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Utilizador utilizador;

                switch (model.Role)
                {
                    case "Administrador":
                        utilizador = new Administrador
                        {
                            Username = model.Username,
                            Data_Nascimento = model.Data_Nascimento,
                           // Role = model.Role,
                            // Inicialize propriedades específicas de Administrador
                            //AdminSpecificProperty = "some value"
                        };
                        _context.Administradores.Add((Administrador)utilizador);
                        break;

                    case "Utente":
                        utilizador = new Utente
                        {
                            Username = model.Username,
                            Data_Nascimento = model.Data_Nascimento,
                           // Role = model.Role,
                            // Inicialize propriedades específicas de Utente
                            //UtenteSpecificProperty = "some value"
                        };
                        _context.Utentes.Add((Utente)utilizador);
                        break;

                    case "Autor":
                        utilizador = new Autor
                        {
                            Username = model.Username,
                            Data_Nascimento = model.Data_Nascimento,
                          //  Role = model.Role,
                            // Inicialize propriedades específicas de Autor
                            //AutorSpecificProperty = "some value"
                        };
                        _context.Autores.Add((Autor)utilizador);
                        break;

                    default:
                        utilizador = new Utilizador
                        {
                            Username = model.Username,
                            Data_Nascimento = model.Data_Nascimento,
                         //  Role = model.Role
                        };
                        _context.Utilizador.Add(utilizador);
                        break;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Utilizadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idUtilizador,Username,Data_Nascimento,UserId")] Utilizador utilizador)
        {
            if (id != utilizador.idUtilizador)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.idUtilizador))
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
            return View(utilizador);
        }

        // GET: Utilizadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizador
                .FirstOrDefaultAsync(m => m.idUtilizador == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador != null)
            {
                _context.Utilizador.Remove(utilizador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizador.Any(e => e.idUtilizador == id);
        }
    }
}
