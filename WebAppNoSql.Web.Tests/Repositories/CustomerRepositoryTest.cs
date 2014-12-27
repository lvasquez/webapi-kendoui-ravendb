using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebAppNoSql.Repo.Repositories;
using WebAppNoSql.Domain;
using System.Linq;

namespace WebAppNoSql.Web.Tests.Repositories
{
    /// <summary>
    /// Summary description for CustomerRepositoryTest
    /// </summary>
    [TestClass]
    public class CustomerRepositoryTest
    {
     
        [TestMethod]
        public void GetCustomersRepo_Return_Data()
        {
            // Arrange
            string url = "http://localhost:8081/ravendbserver/databases/northwind";
            
            ICustomerRepository customerRepository = new CustomerRepository(url);

            var customer = customerRepository.GetCustomers();
            
            // Assert
            Assert.IsInstanceOfType(customer, typeof(IQueryable<Customer>));
        }
    }
}
