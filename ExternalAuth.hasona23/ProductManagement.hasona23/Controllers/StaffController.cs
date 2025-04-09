using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.hasona23.Constants;
using ProductManagement.hasona23.Data;
using ProductManagement.hasona23.Models;

namespace ProductManagement.hasona23.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StaffController> _logger;

        public StaffController(ApplicationDbContext context, ILogger<StaffController> logger,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        // GET: Books
        public async Task<IActionResult> Index(string? searchUserName, string? searchEmail, string? searchRole,
            bool? isEmailConfirmed, int page = 1)
        {
            var searchModel = new UserSearchModel
            {
                SearchUserName = searchUserName,
                SearchEmail = searchEmail,
                SearchRole = searchRole,
                IsEmailConfirmed = isEmailConfirmed,
                CurrentPage = page,
            };
            // Initialize with all users
            const int pageSize = 4;
            // Apply search filters asynchronously
            var filteredUsers = await searchModel.SearchUsers(_userManager);

            // Sort and paginate
            var paginatedUsers = new PaginatedList<IdentityUser>(filteredUsers.OrderBy(user => user.UserName).ToList(),
                page, pageSize);

            searchModel.Users = paginatedUsers.GetItems();
            searchModel.TotalPages = paginatedUsers.TotalPages;
            return View(searchModel);
        }


        // GET: Books/Promote/5
        public async Task<IActionResult> Promote(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!_userManager.IsInRoleAsync(user, Roles.Staff).Result)
            {
                var result = await _userManager.AddToRoleAsync(user, Roles.Staff);
                _logger.LogInformation($"Promoted");
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to promote the user.");
                    _logger.LogError($"Couldn't promote user : {user.Id} to STAFF");
                    return View("Error"); // Optionally, show an error view.
                }

                await _userManager.RemoveFromRoleAsync(user, Roles.Customer);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Demote(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (_userManager.IsInRoleAsync(user, Roles.Staff).Result)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, Roles.Staff);
                if (!await _userManager.IsInRoleAsync(user, Roles.Customer))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Customer);
                    _logger.LogInformation($"Demoted {user.Id} To CUSTOMER");
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to demote the user.");
                    _logger.LogError($"Couldn't demote user : {user.Id}");
                    return View("Error"); // Optionally, show an error view.
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Books/Delete/5

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete the user.");
                return View("Error"); // Optionally, show an error view.
            }

            return RedirectToAction(nameof(Index));
        }
    }
}