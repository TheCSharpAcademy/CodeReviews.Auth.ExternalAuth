using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.hasona23.Constants;
using ProductManagement.hasona23.Data;
using ProductManagement.hasona23.Models;

namespace ProductManagement.hasona23.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            ILogger<BooksController> logger, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Books
        public async Task<IActionResult> Index(string? bookName, int? maxPrice, int? minPrice, bool? isActive,
            DateTime? dateAdded, int page = 1)
        {
            var bookSearchModel = new BookSearchModel
            {
                BookName = bookName,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                IsActive = isActive,
                DateAdded = dateAdded,
                CurrentPage = page
            };
            if (!_signInManager.IsSignedIn(User) || IsUserInRole(Roles.Customer))
                bookSearchModel.IsActive = true;

            const int pageSize = 4;
            var books = await _context.Books.ToListAsync();

            // Create paginated list

            // Assign the books and apply search filters
            bookSearchModel.Books = books;
            bookSearchModel.Books = bookSearchModel.SearchBooks().OrderBy(b => b.Name)
                .ThenBy(b => b.IsActive)
                .ThenBy(b => b.DateAdded);
            var paginatedBooks = new PaginatedList<BookModel>(bookSearchModel.Books.ToList(), page, pageSize);
            bookSearchModel.Books = paginatedBooks.GetItems();
            bookSearchModel.TotalPages = paginatedBooks.TotalPages;
            return View(bookSearchModel);
        }


        // GET: Books/Details/5    
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                _logger.LogWarning($"Could not find book ID: {id}");
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: Books/Create
        [Authorize(Roles = $"{Roles.Admin} , {Roles.Staff}")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.Admin} , {Roles.Staff}")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bookModel.DateAdded = DateTime.Now;
                    _context.Add(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"Couldn't Create Book. USER:{_userManager.FindByNameAsync(User.Identity.Name).Result.Id}:{ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(bookModel);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = $"{Roles.Admin} , {Roles.Staff}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel == null)
            {
                _logger.LogWarning($"Could not find book ID: {id}");
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.Admin} , {Roles.Staff}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,DateAdded,IsActive")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                _logger.LogWarning($"Could not find book ID: {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (!BookModelExists(bookModel.Id))
                    {
                        _logger.LogError($"Attempted to Update NON-EXISTING book {ex}");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Error Updating Book Model : {ex}");
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(bookModel);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin , Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                _logger.LogWarning($"Could not find book ID: {id}");
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.Admin} , {Roles.Staff}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookModel = await _context.Books.FindAsync(id);
            if (bookModel != null)
            {
                try
                {
                    _context.Books.Remove(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Couldnt Delete BookID:{bookModel.Id} USER:{_userManager.FindByNameAsync(User.Identity.Name).Result.Id}  :{ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookModelExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private bool IsUserInRole(string role)
        {
            if (!_signInManager.IsSignedIn(User))
                return false;
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            return _userManager.IsInRoleAsync(user, role).Result;
        }
    }
}