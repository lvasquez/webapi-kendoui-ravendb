using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppNoSql.Domain
{
    public class Customer
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public int phone { get; set; }

        public string email { get; set; }

        public bool status { get; set; }


    }
}
