using Microsoft.AspNetCore.Mvc;
using System;
using WebApplicationMVCExample.DataContext;
using WebApplicationMVCExample.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplicationMVCExample.Controllers
{
    public class InvoiceController : Controller
    {
        private AppDBContext appContext;

        public InvoiceController(AppDBContext appDBContext)
        {
            appContext = appDBContext;
        }

        public IActionResult Index()
        {
            var invoices = appContext.Customers.ToList();
            return View();
        }

        List<Invoice> allInvoices = new List<Invoice>();
        
        public ActionResult InvoiceView(int id)
        {
            System.Diagnostics.Debug.WriteLine("Customer ID is here only-> " + id);
            var customer = appContext.Customers.Where(u=> u.CustomerId == id).FirstOrDefault();
            List<Invoice> tempInv = new List<Invoice>();
            Invoice tempInvoice = new Invoice();
            List<Invoice> invoices = appContext.Invoices.Where(u=> u.CustomerId == id).ToList();
            PaymentTerms firstInvPayTerms = new PaymentTerms();
            ViewBag.invId = 1;

            if ((bool)(customer?.Name.StartsWith("A")) || (bool)(customer?.Name.StartsWith("B")) || (bool)(customer?.Name.StartsWith("C")) || (bool)(customer?.Name.StartsWith("D")) || (bool)(customer?.Name.StartsWith("E")))
            {
                ViewBag.filter = "A-E";
                ViewBag.filterValue = "A";
            }
            else if ((bool)(customer?.Name.StartsWith("F")) || (bool)(customer?.Name.StartsWith("G")) || (bool)(customer?.Name.StartsWith("H")) || (bool)(customer?.Name.StartsWith("I")) || (bool)(customer?.Name.StartsWith("J")) || (bool)(customer?.Name.StartsWith("K")))
            {
                ViewBag.filter = "F-K";
                ViewBag.filterValue = "F";
            }
            else if ((bool)(customer?.Name.StartsWith("L")) || (bool)(customer?.Name.StartsWith("M")) || (bool)(customer?.Name.StartsWith("N")) || (bool)(customer?.Name.StartsWith("O")) || (bool)(customer?.Name.StartsWith("P")) || (bool)(customer?.Name.StartsWith("Q")) || (bool)(customer?.Name.StartsWith("R")))
            {
                ViewBag.filter = "L-R";
                ViewBag.filterValue = "L";
            }
            else if ((bool)(customer?.Name.StartsWith("S")) || (bool)(customer?.Name.StartsWith("T")) || (bool)(customer?.Name.StartsWith("U")) || (bool)(customer?.Name.StartsWith("V")) || (bool)(customer?.Name.StartsWith("W")) || (bool)(customer?.Name.StartsWith("X")) || (bool)(customer?.Name.StartsWith("Y")) || (bool)(customer?.Name.StartsWith("Z")))
            {
                ViewBag.filter = "S-Z";
                ViewBag.filterValue = "S";
            }

            if (invoices?.Count > 0) { 
                firstInvPayTerms = appContext.PaymentTerms.Where(u => u.PaymentTermsId == invoices[0].PaymentTermsId).FirstOrDefault();
                allInvoices = invoices;

                var firstInv = appContext.Invoices.Where(u => u.CustomerId == id).First();
                ViewBag.firstInvId = firstInv?.InvoiceId;

                var latestInv = appContext.Invoices.Where(u => u.CustomerId == id).OrderByDescending(t => t.InvoiceId).First();
                var latestPayTerms = appContext.PaymentTerms.Where(u => u.PaymentTermsId == latestInv.PaymentTermsId).FirstOrDefault();
                ViewBag.invId = latestInv?.InvoiceId;

                if (latestPayTerms != null)
                {
                    ViewBag.invTerms = (int)(latestPayTerms?.DueDays);
                }

                invoices.ForEach(invoice =>
                {
                    tempInvoice = invoice;
                    PaymentTerms paymentTerms = appContext.PaymentTerms.Where(u => u.PaymentTermsId == invoice.PaymentTermsId).FirstOrDefault();
                    tempInvoice.InvoiceDueDate = invoice.InvoiceDate?.AddDays(Convert.ToDouble(paymentTerms?.DueDays));
                    tempInv.Add(tempInvoice);
                });
            }

            var firstInvoice = 0;
            var invTerms = 0;

            if(firstInvPayTerms != null) { 
                invTerms = (int)(firstInvPayTerms.DueDays);
            }
            if (invoices?.Count > 0){ 
                firstInvoice = (int)(invoices[0]?.PaymentTotal);
            }
            System.Diagnostics.Debug.WriteLine("First Invoice-> "+ firstInvoice);
            ViewBag.CustomerId = customer?.CustomerId;
            ViewBag.CustomerName = customer?.Name;
            ViewBag.CustomerAddress = customer?.Address1;
            ViewBag.defaultInvTerms = invTerms;
            ViewBag.firstInv = firstInvoice;

            System.Diagnostics.Debug.WriteLine("Customer ID is here only-> \n" + id);
            System.Diagnostics.Debug.WriteLine("Customer ID is here only-> \n" + id);
            System.Diagnostics.Debug.WriteLine("Number of invoices in the db I-> \n" + id);

            return View(tempInv);
        }

        
        public ActionResult LineItemView(int id, int custId)
        {
            System.Diagnostics.Debug.WriteLine("Selected Invoice Id-> "+id);
            List<InvoiceLineItem> invLineItems = new List<InvoiceLineItem>();
            var invoiceTotal = 0.0;

            ViewData["msg"] = "Hi, I am LineItemView Method!!";

            Invoice selectedInvoice = appContext.Invoices.Where(u => u.InvoiceId == id && u.CustomerId == custId).FirstOrDefault();
            
            if (selectedInvoice != null) { 
                selectedInvoice = appContext.Invoices.Where(u => u.InvoiceId == id).FirstOrDefault();
                
                ViewData["invTerms"] = appContext.PaymentTerms.Where(u=> u.PaymentTermsId == selectedInvoice.PaymentTermsId).FirstOrDefault()?.DueDays;
                ViewData["invId"] = id;

                invLineItems = appContext.InvoiceLineItems.Where(i => i.InvoiceId == id).ToList();
                System.Diagnostics.Debug.WriteLine("\n\n<---Invoice Line Items Retrieved---->\n\n " + invLineItems);
                ViewData["lineItems"] = invLineItems;

                invoiceTotal = (double)(selectedInvoice?.PaymentTotal);
                ViewData["totalAmount"] = invoiceTotal;
            }
            System.Diagnostics.Debug.WriteLine("Invoice Total-> " + invoiceTotal);
            return PartialView("_LineItemView");
        }

        [HttpPost]
        public ActionResult AddInvoiceView(DateTime date, int terms, int id)
        {
            System.Diagnostics.Debug.WriteLine("Passed date and terms-> " + date + " " + terms + " " + id);
            var payTermsId = appContext.PaymentTerms.Where(u => u.DueDays == terms).FirstOrDefault().PaymentTermsId;
            Invoice inv = new Invoice();
            inv.InvoiceDate = DateTime.Now;
            inv.InvoiceDueDate = date;
            inv.CustomerId = id;
            inv.PaymentTermsId = payTermsId;
            inv.PaymentDate = date;
            inv.PaymentTotal = 0;

            appContext.Add(inv);
            appContext.SaveChanges();

            var latestInv = appContext.Invoices.Where(u => u.CustomerId == id).OrderByDescending(t=> t.InvoiceId).First();
            ViewData["invId"] = latestInv?.InvoiceId;
            System.Diagnostics.Debug.WriteLine("<\n\n-----Latest Invoice Id--->" + latestInv?.InvoiceId);

            System.Diagnostics.Debug.WriteLine("<---Data updated in database successfully--->" + "\nwith pay_id in invoice table-> " +inv.PaymentTermsId + "\nwith pay_id in pay_terms table-> ");

            return RedirectToAction("InvoiceView", id);
        }

        [HttpPost]
        public ActionResult AddLineItemView(int invId, string lineItemDesc, double lineItemAmount)
        {
            System.Diagnostics.Debug.WriteLine("Passed date and terms-> " + invId + " " + lineItemDesc + " " + lineItemAmount);
            InvoiceLineItem invLineItem = new InvoiceLineItem();
            invLineItem.InvoiceId = invId;
            invLineItem.Description = lineItemDesc;
            invLineItem.Amount = lineItemAmount;
            var id = invId;
            appContext.Add(invLineItem);
            appContext.SaveChanges();

            Invoice invoice = appContext.Invoices.Where(u => u.InvoiceId == invId).FirstOrDefault();
            invoice.PaymentTotal += lineItemAmount;

            appContext.Update(invoice);
            appContext.SaveChanges();

            System.Diagnostics.Debug.WriteLine("<---Data updated in database successfully--->"+invLineItem.InvoiceId + " " + invLineItem.Description + " " + invLineItem.Amount);

            return RedirectToAction("LineItemView", id);
        }
    }
}
