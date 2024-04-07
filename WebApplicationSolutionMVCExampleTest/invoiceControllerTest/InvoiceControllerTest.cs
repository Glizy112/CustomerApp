using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationMVCExample.Controllers;
using WebApplicationMVCExample.DataContext;
using WebApplicationMVCExample.Models;

namespace WebApplicationSolutionMVCExampleTest.invoiceControllerTest
{
    public class InvoiceControllerTest
    {
        private readonly AppDBContext _dbContext;
        private readonly DbContextOptions<AppDBContext> options;
        string v = "Server=localhost;Database=crm;User=root;Password=root;";

        public InvoiceControllerTest()
        {
            options = new DbContextOptionsBuilder<AppDBContext>().UseMySql(v, ServerVersion.AutoDetect(v)).Options;
            _dbContext = new AppDBContext(options);
        }

        [Fact]
        public void InvoiceViewExample_Returns()
        {

            // Arrange

            var controller = new InvoiceController(_dbContext);
            var result = controller.Index();

            var ViewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Invoice>>(ViewResult.ViewData.Model);

            Assert.NotNull(model);  
            Assert.NotEmpty(model);
        }
    }
}
