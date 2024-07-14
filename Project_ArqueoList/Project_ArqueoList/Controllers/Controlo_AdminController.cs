using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_ArqueoList.Models;
using System.Data;
using System.Threading.Tasks;

namespace Project_ArqueoList.Controllers
{
    public class Controlo_AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Controlo_AdminController(UserManager<IdentityUser> userManager,
                                        RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Controlo_Admin/Controlo
        public IActionResult Controlo()
        {
            return View();
        }

        // POST: Controlo_Admin/Controlo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Controlo(string username, string newRole)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newRole))
            {
                return BadRequest("Username and role must be provided.");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                return StatusCode(500, "Error removing current roles.");
            }

            if (!await _roleManager.RoleExistsAsync(newRole))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(newRole));
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, "Error creating role.");
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addRoleResult.Succeeded)
            {
                return StatusCode(500, "Error adding role to user.");
            }

            return RedirectToAction("Index");
        }
    }

}
