﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Arrange - create a mock repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            //Arrange - Create an empty cart
            Cart cart = new Cart();

            //Arrange - Create the order.
            Order order = new Order();

            //Arrange - create a new instance of the controller
            OrderController target = new OrderController(mock.Object, cart);

            //Act
            ViewResult result = target.Checkout(order) as ViewResult;

            //Assert - check that the order hasn't been stored.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            //Assert - check that the method is returning the default view.
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            //Assert - check if the an invalida model is being passed to the view.
            Assert.False(result.ViewData.ModelState.IsValid);

        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange - Create a mock order repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            //Arrange - Create a cart with one item 
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            //Arrange - create an instance of the controller class.
            OrderController target = new OrderController(mock.Object, cart);

            //Act- try to checkout
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            //Assert - check that the order has been stored.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            //Assert - check that the method is redirecting to the Completed action
            Assert.Equal("Completed", result.ActionName);                
        }

    }
}
