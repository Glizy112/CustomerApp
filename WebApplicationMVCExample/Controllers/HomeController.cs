using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationMVCExample.DataContext;
using WebApplicationMVCExample.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            //var products = prods.Where(u => !u.isDeleted).ToList();
            var customers = appContext.Customers.ToList();
            return View(customers);
        }

        public ActionResult AddView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddView(Customer cust)
        {
            if (ModelState.IsValid)
            {
                var customers = appContext.Customers.ToList();
                cust.CustomerId = customers.Count + 1;

                //System.Diagnostics.Debug.WriteLine("prodId"+prod.Id);
                //customers.Add(cust);
                appContext.Add(cust);
                appContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cust);
        }

        [HttpGet]
        public async Task<IActionResult> EditView(int id)
        {
            var cust = appContext.Customers.FirstOrDefault(u => u.CustomerId == id);
            if (cust == null)
            {
                return Error();
            }


            return View(cust);
        }

        [HttpPost]
        public async Task<IActionResult> EditView(Customer cust)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = appContext.Customers.FirstOrDefault(u => u.CustomerId == cust.CustomerId);
                if (existingCustomer != null)
                {
                    System.Diagnostics.Debug.WriteLine("Editing");
                    //existingCustomer.Name = cust.Name;
                    //existingCustomer.Type = cust.Type;
                    existingCustomer.Name = cust.Name;
                    existingCustomer.Address1 = cust.Address1;
                    existingCustomer.ContactEmail = cust.ContactEmail;
                    existingCustomer.Phone = cust.Phone;
                    existingCustomer.ZipOrPostalCode = cust.ZipOrPostalCode;
                }
                appContext.Update(existingCustomer);
                appContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cust);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteView(int id)
        {

            var existingCustomer = appContext.Customers.FirstOrDefault(u => u.CustomerId == id);
            //  prods.Remove(existingProduct);
            existingCustomer.IsDeleted = true;
            TempData["DeletedUserMessage"] = $"User {existingCustomer.Name} has been deleted.";
            var message = TempData["DeletedUserMessage"].ToString();
            TempData["UndoMessage"] = message;
            System.Diagnostics.Debug.WriteLine("Deleting");
            System.Diagnostics.Debug.WriteLine("view message" + message);
            TempData["DeletedCustId"] = existingCustomer.CustomerId;

            appContext.Update(existingCustomer);
            appContext.SaveChanges();

            //var tempDelete = Task.Run(() =>
            //{
            //await Task.Delay(5000);
            return RedirectToAction("Index");
            //});
            //if (tempDelete.Wait(TimeSpan.FromSeconds(5)))
            //    return tempDelete.Result;
            //else
            //    throw new Exception("Timed Out");
            
        }

        public async Task<IActionResult> UndoDelete()
        {
            var custId = (int)TempData["DeletedCustId"];
            var existingCustomer = appContext.Customers.FirstOrDefault(u => u.CustomerId == custId);
            System.Diagnostics.Debug.WriteLine("Deleted "+ existingCustomer.Name);
            existingCustomer.IsDeleted = false;
            ViewBag.UndoMessage =  TempData["DeletedUserMessage"].ToString();

            appContext.Update(existingCustomer);
            appContext.SaveChanges();

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
