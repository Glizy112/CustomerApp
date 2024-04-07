using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using WebApplicationMVCExample.Controllers;
using WebApplicationMVCExample.DataContext;
using WebApplicationMVCExample.Models;

namespace WebApplicationSolutionMVCExampleTest
{
    public class UnitTest1
    {
        [Fact]
        public void AddViewExample_ReturnsEmpty()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.AddView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void IndexExample_ReturnsCustomers()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();
   
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Customer>>(viewResult.Model);
            Assert.NotNull(model);
        }

        private readonly AppDBContext _dbContext;
        private readonly DbContextOptions<AppDBContext> options;
        string v = "Server=localhost;Database=crm;User=root;Password=root;";

        public UnitTest1()
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