using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c969
{
    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Phone {  get; set; }
        public string City { get; set; }
        public string Country {  get; set; }

       public static BindingList<Customer> customerList = new BindingList<Customer>();


    }
}
