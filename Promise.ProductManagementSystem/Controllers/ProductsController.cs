using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promise.ProductManagementSystem.Data;
using Promise.ProductManagementSystem.Models;
using Promise.ProductManagementSystem.ViewModels;

namespace Promise.ProductManagementSystem.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // GET: ProductsController
        public ActionResult Index()
        {
            var products = _dbContext.Products.ToList();
            _logger.LogInformation("--> Total products retrieved: {count}", products.Count());
            var viewModel = new ProductViewModel { Products = products };
            return View(viewModel);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProduct createProduct)
        {
            if (!ModelState.IsValid)
            {
                return View(createProduct);
            }
            var product = new Product
            {
                Name = createProduct.Name,
                Description = createProduct.Description,
                Price = createProduct.Price,
                IsActive = createProduct.IsActive,
                DateAdded = DateTime.UtcNow,
            };
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            _logger.LogInformation("--> Product created: {productName} at {time}", product.Name, product.DateAdded);
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                _logger.LogWarning("--> Product with ID {id} not found for editing", id);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditProduct editProduct)
        {
            try
            {
                var product = _dbContext.Products.Find(id);
                if (product == null)
                    return RedirectToAction(nameof(Index));

                product.Name = editProduct.Name;
                product.Description = editProduct.Description;
                product.Price = editProduct.Price;
                product.IsActive = editProduct.IsActive;

                _dbContext.SaveChanges();
                _logger.LogInformation("--> Product edited: {productName} at {time}", product.Name, DateTime.UtcNow);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                _logger.LogWarning("--> Product with ID {id} not found for deletion", id);
                return RedirectToAction(nameof(Index));

            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            _logger.LogInformation("--> Product deleted: {productName} at {time}", product.Name, DateTime.UtcNow);
            return RedirectToAction(nameof(Index));
        }
        // GET: ProductsController/Details/5
        [HttpGet]
        public ActionResult Get(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                _logger.LogWarning("--> Product with ID {id} not found", id);
                return NotFound();
            }
            return View(product);
        }
    }
}
