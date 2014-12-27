using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppNoSql.Domain;
using WebAppNoSql.Repo.Repositories;

namespace WebAppNoSql.Web.Controllers
{
    public class CustomerController : ApiController
    {
        readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        // GET: api/Customer
        public IQueryable<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        // GET: api/Customer/5
        public HttpResponseMessage GetCustomer(int id)
        {
            Customer customer = this._customerRepository.Read(id);
            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(customer);
        }

        // POST: api/Customer
        public HttpResponseMessage Post(Customer customer)
        {
            if (ModelState.IsValid)
            {
                this._customerRepository.Create(customer);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customer);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customer.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT: api/Customer/5
        public HttpResponseMessage PutCustomer(Customer customer)
        {
            return PutCustomer(customer.Id, customer);
        }

        public HttpResponseMessage PutCustomer(int id, Customer customer)
        {
            if (ModelState.IsValid && id == customer.Id)
            {
                try
                {
                    this._customerRepository.Update(customer);
                    return Request.CreateResponse(HttpStatusCode.OK, customer);
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }             
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/Customer/5
        public HttpResponseMessage DeleteCustomer(Customer customer)
        {
            try
            {
                this._customerRepository.Delete(customer);
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }        
        }

    }
}
