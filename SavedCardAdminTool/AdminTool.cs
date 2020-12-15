using System.Collections.Generic;

namespace SavedCardAdminTool
{
    public class AdminTool
    {
        public AdminTool()
        {
            AllCustomers = new List<Customer>()
            {
                new Customer("2101", "Aaron Jackson", "Aaron_j@aol.com"),
                new Customer("2102", "Nick Zack", "nick_za@aol.com"),
                new Customer("2103", "Pete Kings", "kings_p@aol.com"),
            };
        }

        public List<Customer> AllCustomers { get; set; }

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