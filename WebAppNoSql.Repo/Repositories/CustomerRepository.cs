using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppNoSql.Domain;

namespace WebAppNoSql.Repo.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        DocumentStore _store = null;

        public CustomerRepository(string url)
        {
            _store = new DocumentStore() { Url = url };
            _store.Initialize();
        }

        public IQueryable<Customer> GetCustomers()
        {
            using (var documentSession = _store.OpenSession())
            {
                var list = documentSession.Query<Customer>().AsQueryable();
                return list;
            }
        }

        public Customer Create(Customer customer)
        {
            using (var documentSession = _store.OpenSession())
            {
                documentSession.Store(customer);
                documentSession.SaveChanges();
                return customer;
            }
        }

        public Customer Read(int id)
        {
            using (var documentSession = _store.OpenSession())
            {
                var customer = documentSession.Load<Customer>(id);
                return customer;
            }
        }

        public Customer Update(Customer customer)
        {
            using (var documentSession = _store.OpenSession())
            {
                Customer currentCustomer = documentSession.Load<Customer>(customer.Id);
                currentCustomer.name = customer.name;
                currentCustomer.address = customer.address;
                currentCustomer.phone = customer.phone;
                currentCustomer.email = customer.email;
                currentCustomer.status = customer.status;

                documentSession.Store(currentCustomer);
                documentSession.SaveChanges();
                return customer;
            }
        }

        public Customer Delete(Customer customer)
        {
            using (var documentSession = _store.OpenSession())
            {
                Customer currentCustomer = documentSession.Load<Customer>(customer.Id);
                documentSession.Delete<Customer>(currentCustomer);
                documentSession.SaveChanges();
                return customer;
            }
        }
    }
}
