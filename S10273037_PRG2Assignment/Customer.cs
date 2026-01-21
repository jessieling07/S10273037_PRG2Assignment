using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10273037B
// Student Name : Jessie Ling
// Partner Name : Lu Sijin
//==========================================================

namespace S10273037_PRG2Assignment
{
    public class Customer
    {
        private string emailAddress;
        private string customerName;
        private List<Order> orders;

        public Customer(string emailAddress, string customerName)
        {
            this.emailAddress = emailAddress;
            this.customerName = customerName;
            this.orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public void DisplayAllOrders()
        {
            Console.WriteLine($"\nOrders for {customerName}:");
            foreach (Order order in orders)
            {
                Console.WriteLine($"  - {order.ToString()}");
            }
        }

        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }

        public string ToString()
        {
            return $"Customer: {customerName}, Email: {emailAddress}, Orders: {orders.Count}";
        }
    }
}
