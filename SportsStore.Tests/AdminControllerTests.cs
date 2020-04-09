using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;
using Moq;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            //Arrange - Create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            //Arrange - create an admin controller
            AdminController target = new AdminController(mock.Object);

            //Action
            Product[] result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            //Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(1, result[0].ProductID);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);

        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact] 
        public void Can_Edit_Product()
        {
            //Arrange - Create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            //Arrange - create an admin controller
            AdminController target = new AdminController(mock.Object);

            //Act
            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));

            //Assert
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3,p3.ProductID);

        }

        [Fact]
        public void Cannot_Edit_Non_Existent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            //Arrange - create an admin controller
            AdminController target = new AdminController(mock.Object);

            //Act 
            Product p4 = GetViewModel<Product>(target.Edit(4));

            //Assert
            Assert.Null(p4);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            //Arrange- Create a new product
            Product p2 = new Product { ProductID = 2, Name = "Test" };

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            //Arrange- Create a new admin controler
            AdminController target = new AdminController(mock.Object);

            ////Act 
            //Product result = target.Delete(1) as Product ;

            ////Assert
            //Assert.Equal(1, result.ProductID);

            //Act
            target.Delete(p2.ProductID);

            //Assert- ensure that the repo delete method was called in the correct product
            mock.Verify(m => m.DeleteProduct(p2.ProductID));

        }

    }
}
