using System.Collections.Generic;

namespace SavedCardAdminTool
{
    public class AdminTool
    {
        public AdminTool()
        {
            AllCustomers = new List<Customer>()
            {
                new Customer( "Aaron Jackson", "Aaron_j@aol.com"),
                new Customer( "Nick Zack", "nick_za@aol.com"),
                new Customer( "Peter Kings", "kings_p@aol.com"),
                new Customer( "test", "test_p@test.com"),
            };
        }

        public List<Customer> AllCustomers { get;  }

        public string RegisterNewCustomer(Customer customer)
        {
            AllCustomers.Add(customer);
            return $"{customer.FullName} successfully joined";
        }

        public void RemoveCustomer(Customer customer)
        {
            AllCustomers.Remove(customer);
        }
    }
}