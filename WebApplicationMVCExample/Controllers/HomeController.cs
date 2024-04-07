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

        public HomeController(AppDBContext appDBContext) {
            appContext = appDBContext;
        }

        public IActionResult Index()
        {
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
                    existingCustomer.Name = cust.Name;
                    existingCustomer.Address1 = cust.Address1;
                    existingCustomer.Address2 = cust.Address2;
                    existingCustomer.ContactEmail = cust.ContactEmail;
                    existingCustomer.Phone = cust.Phone;
                    existingCustomer.ZipOrPostalCode = cust.ZipOrPostalCode;
                    existingCustomer.City = cust.City;
                    existingCustomer.ProvinceOrState = cust.ProvinceOrState;
                    existingCustomer.ContactFirstName = cust.ContactFirstName;
                    existingCustomer.ContactLastName = cust.ContactLastName;
                    existingCustomer.ContactEmail = cust.ContactEmail;
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
            existingCustomer.IsDeleted = true;
            TempData["DeletedUserMessage"] = $"The customer {existingCustomer.Name} was deleted. ";
            var message = TempData["DeletedUserMessage"].ToString();
            TempData["UndoMessage"] = message;
            System.Diagnostics.Debug.WriteLine("Deleting");
            System.Diagnostics.Debug.WriteLine("view message" + message);
            TempData["DeletedCustId"] = existingCustomer.CustomerId;

            appContext.Update(existingCustomer);
            appContext.SaveChanges();

            return RedirectToAction("Index");
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

        [HttpPost]
        public ActionResult FilterCustomers(string filterOption)
        {
            char x = (char)(filterOption[0] + 1);
            List<Customer> allCustomers = appContext.Customers.ToList();
            System.Diagnostics.Debug.WriteLine("Selected Filter" + x);

            if (filterOption != null)
            {
                if (filterOption == "A")
                {
                    System.Diagnostics.Debug.WriteLine("Selected Filter");
                    allCustomers = allCustomers
             .Where(p => p.Name.StartsWith(filterOption[0]) || p.Name.StartsWith((char)(filterOption[0] + 1)) ||
                         p.Name.StartsWith((char)(filterOption[0] + 2)) || p.Name.StartsWith((char)(filterOption[0] + 3)) ||
                         p.Name.StartsWith((char)(filterOption[0] + 4))).ToList();
                }
                else if (filterOption == "F")
                {
                    System.Diagnostics.Debug.WriteLine("Selected Filter");
                    allCustomers = allCustomers
              .Where(p => p.Name.StartsWith(filterOption[0]) || p.Name.StartsWith((char)(filterOption[0] + 1)) ||
                          p.Name.StartsWith((char)(filterOption[0] + 2)) || p.Name.StartsWith((char)(filterOption[0] + 3)) ||
                          p.Name.StartsWith((char)(filterOption[0] + 4)) || p.Name.StartsWith((char)(filterOption[0] + 5))).ToList();
                }
                else if (filterOption == "L")
                {
                    System.Diagnostics.Debug.WriteLine("Selected Filter");
                    allCustomers = allCustomers
                .Where(p => p.Name.StartsWith(filterOption[0]) || p.Name.StartsWith((char)(filterOption[0] + 1)) ||
                            p.Name.StartsWith((char)(filterOption[0] + 2)) || p.Name.StartsWith((char)(filterOption[0] + 3)) ||
                            p.Name.StartsWith((char)(filterOption[0] + 4)) || p.Name.StartsWith((char)(filterOption[0] + 5)) || p.Name.StartsWith((char)(filterOption[0] + 6))).ToList();
                }
               else
                {
                    System.Diagnostics.Debug.WriteLine("Selected Filter");
                    allCustomers = allCustomers
               .Where(p => p.Name.StartsWith(filterOption[0]) || p.Name.StartsWith((char)(filterOption[0] + 1)) ||
                           p.Name.StartsWith((char)(filterOption[0] + 2)) || p.Name.StartsWith((char)(filterOption[0] + 3)) ||
                           p.Name.StartsWith((char)(filterOption[0] + 4)) || p.Name.StartsWith((char)(filterOption[0] + 5)) || p.Name.StartsWith((char)(filterOption[0] + 6)) || p.Name.StartsWith((char)(filterOption[0] + 7))).ToList();

                }
                return View("Index", allCustomers);
            }
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
