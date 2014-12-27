using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAppNoSql.Web.Controllers;
using Moq;
using WebAppNoSql.Repo.Repositories;
using WebAppNoSql.Domain;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net;

namespace WebAppNoSql.Web.Tests.Controllers
{
    /// <summary>
    /// Summary description for CustomerApiController
    /// </summary>
    [TestClass]
    public class CustomerControllerTest
    {
        private Mock<ICustomerRepository> _mockService; // dependency of controller
        private CustomerController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockService = new Mock<ICustomerRepository>();
            _controller = new CustomerController(_mockService.Object);
        }

        [TestMethod]
        public void GetCustomers_Return_NotNull()
        {
            List<Customer> list = new List<Customer>
            {
                new Customer(){ Id = 1, name="Luis", address = "Content 1", phone = 57678979, email = "levb20@gmail.com", status = true },
                new Customer(){ Id = 2, name="Maria", address = "Content 2", phone = 2221123, email = "morx@gmail.com",  status = true }
            };
            
            _mockService.Setup(f => f.GetCustomers()).Returns(list.AsQueryable());

            var result = _controller.GetCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
      

        [TestMethod]
        public void GetCustomer_Returns_Customer()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _mockService.Setup(m => m.Read(customer.Id)).Returns(customer);
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.GetCustomer(customer.Id);

            // Assert
            Assert.IsTrue(response.TryGetContentValue<Customer>(out customer));
            Assert.AreEqual(1, customer.Id);
        }

        [TestMethod]
        public void GetCustomer_Returns_Customer_Null()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };
         
            // Arrange
            _mockService.Setup(m => m.Read(customer.Id)).Returns<Customer>(null);
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.GetCustomer(customer.Id) as HttpResponseMessage;

            // Assert
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Post_Return_NotNull()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://localhost:1466/api/Customer";

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            _controller.Url = mockUrlHelper.Object;

            // Act
            var response = _controller.Post(customer);

            // Assert
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
            _mockService.Verify(foo => foo.Create(customer), Times.Once);
            _mockService.VerifyAll();
        }

        [TestMethod]
        public void Post_Return_BadRequest()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _controller.ModelState.AddModelError("Id", "error message");

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.Post(customer) as HttpResponseMessage;

            // Assert
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Put_Return_Ok()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.PutCustomer(customer) as HttpResponseMessage;

            // Assert
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Put_Return_BadRequest()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };     

            _controller.ModelState.AddModelError("Id", "error message");

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.PutCustomer(customer) as HttpResponseMessage;

            // Assert
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        //[ExpectedException(typeof(Exception))]
        public void Put_Return_NotFound()
        {
            //Faking ModelState.IsValid = false      
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _mockService.Setup(m => m.Update(customer)).Throws<Exception>();

            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.PutCustomer(customer) as HttpResponseMessage;

            // Assert
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void DeleteCustomer_Returns_Ok()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _mockService.Setup(m => m.Delete(customer)).Returns(customer);
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.DeleteCustomer(customer) as HttpResponseMessage;

            // Assert
            Assert.IsTrue(response.TryGetContentValue<Customer>(out customer));
            Assert.AreEqual(1, customer.Id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void DeleteCustomer_Returns_NotFound()
        {
            Customer customer = new Customer() { Id = 1, name = "Luis Vasquez", address = "GT", phone = 2223143, email = "levb20@gmail.com", status = true };

            _mockService.Setup(m => m.Delete(customer)).Throws<Exception>();
            // Arrange
            _controller.Request = new HttpRequestMessage();
            _controller.Configuration = new HttpConfiguration();

            // Act
            var response = _controller.DeleteCustomer(customer) as HttpResponseMessage;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

    }

}
