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
    public class Order
    {
        private int orderId;
        private DateTime orderDateTime;
        private double orderTotal;
        private string orderStatus;
        private DateTime deliveryDateTime;
        private string deliveryAddress;
        private string orderPaymentMethod;
        private bool orderPaid;
        private List<object> orderedFoodItems;

        public Order(int orderId, DateTime orderDateTime, double orderTotal, string orderStatus,
                     DateTime deliveryDateTime, string deliveryAddress, string orderPaymentMethod, bool orderPaid)
        {
            this.orderId = orderId;
            this.orderDateTime = orderDateTime;
            this.orderTotal = orderTotal;
            this.orderStatus = orderStatus;
            this.deliveryDateTime = deliveryDateTime;
            this.deliveryAddress = deliveryAddress;
            this.orderPaymentMethod = orderPaymentMethod;
            this.orderPaid = orderPaid;
            this.orderedFoodItems = new List<object>();
        }

        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (object item in orderedFoodItems)
            {
                total += 0;
            }
            return total;
        }

        public void AddOrderedFoodItem(object orderedFoodItem)
        {
            orderedFoodItems.Add(orderedFoodItem);
        }

        public bool RemoveOrderedFoodItem(object orderedFoodItem)
        {
            return orderedFoodItems.Remove(orderedFoodItem);
        }

        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine($"\nOrdered Food Items for Order #{orderId}:");
            foreach (object item in orderedFoodItems)
            {
                Console.WriteLine($"  - {item.ToString()}");
            }
        }

        public string ToString()
        {
            return $"Order ID: {orderId}, Status: {orderStatus}, Total: ${orderTotal:F2}, Paid: {orderPaid}";
        }
    }
}
