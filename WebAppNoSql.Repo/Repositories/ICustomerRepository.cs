using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppNoSql.Domain;

namespace WebAppNoSql.Repo.Repositories
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetCustomers();
        Customer Create(Customer customer);
        Customer Read(int id);
        Customer Update(Customer customer);
        Customer Delete(Customer customer);
    }
}
