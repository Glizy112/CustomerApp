using Microsoft.EntityFrameworkCore;
using WebApplicationMVCExample.Models;

namespace WebApplicationMVCExample.DataContext
{
    public class AppDBContext : DbContext
    { 
        public AppDBContext(DbContextOptions options):base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PaymentTerms> PaymentTerms { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
    }
}
