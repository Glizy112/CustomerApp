using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationMVCExample.DataContext;
using WebApplicationMVCExample.Models;

namespace WebApplicationMVCExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private AppDBContext appContext ;

        private static List<Product> prods = new List<Product>
        {
            new Product { Id = 1, Name = "A", Type ="product", isDeleted =false },
            new Product { Id = 2, Name = "B", Type ="product", isDeleted = false }
        };

        /* public HomeController(ILogger<HomeController> logger)
         {
             _logger = logger;
         }*/

        public HomeController(AppDBContext appDBContext) {
            appContext = appDBContext;
        }

        public IActionResult Index()
        {
            var products = prods.Where(u => !u.isDeleted).ToList();
            return View(products);
        }

        public ActionResult AddView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddView(Product prod)
        {
            if (ModelState.IsValid)
            {
                prod.Id = prods.Count + 1;
                
               System.Diagnostics.Debug.WriteLine("prodId"+prod.Id);
               
                prods.Add(prod);
                return RedirectToAction("Index");
            }

            return View(prod);
        }

        [HttpGet]
        public async Task<IActionResult> EditView(int id)
        {
            var prod = prods.FirstOrDefault(u => u.Id == id);
            if (prod == null)
            {
                return Error();
            }


            return View(prod);
        }

        [HttpPost]
        public async Task<IActionResult> EditView(Product prod)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = prods.FirstOrDefault(u => u.Id == prod.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = prod.Name;
                    existingProduct.Type = prod.Type;
                   
                }
                return RedirectToAction("Index");
            }

            return View(prod);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteView(int id)
        {
            var existingProduct = prods.FirstOrDefault(u => u.Id == id);
            //  prods.Remove(existingProduct);
            existingProduct.isDeleted = true;
            TempData["DeletedUserMessage"] = $"User {existingProduct.Name} has been deleted.";
            var message = TempData["DeletedUserMessage"].ToString();
            ViewBag.UndoMessage = message;
            System.Diagnostics.Debug.WriteLine("view message" + message);
            TempData["DeletedProdId"] = existingProduct.Id;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UndoDelete()
        {
            var prodId = (int)TempData["DeletedProdId"];
            var existingProduct = prods.FirstOrDefault(u => u.Id == prodId);
            System.Diagnostics.Debug.WriteLine("Deleted "+ existingProduct.Name);
            existingProduct.isDeleted = false;
            ViewBag.UndoMessage =  TempData["DeletedUserMessage"].ToString();;   

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Restore(int id)
        {
            var prod = prods.FirstOrDefault(u => u.Id == id);
            if (prod == null)
            {
                return Empty;
            }

            prod.isDeleted = false;  // Restore
            return RedirectToAction("Index");
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
