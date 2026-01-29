using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//==========================================================
// Student Number : S10274083K
// Student Name : Lu Sijin
// Partner Name : Jessie Ling
//==========================================================

namespace S10273037_PRG2Assignment
{
    internal class Restaurant
    {
        private string restaurantId;
        private string restaurantName;
        private string restaurantEmail;
        public string RestaurantId { get; set; }
        public string RestaurantName {  get; set; }
        public string RestaurantEmail { get; set; }
        public List<FoodItem> Menu { get; set; }
        public Queue<Order> OrderQueue { get; set; }
        public List<SpecialOffer> Offers { get; set; }

        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
            Menu = new List<FoodItem>();
            OrderQueue = new Queue<Order>();
            Offers = new List<SpecialOffer>();
        }
        public void DisplayOrders()
        {
            Console.WriteLine($"Orders for {RestaurantName} ({RestaurantId})");
            Console.WriteLine("===================================");

            if (OrderQueue == null || OrderQueue.Count == 0)
            {
                Console.WriteLine("No orders available.");
                return;
            }

            foreach (Order o in OrderQueue)
            {
                Console.WriteLine(o);
            }
        }
        public void DisplaySpecialOffers()
        {
            Console.WriteLine($"Special Offers for {RestaurantName}");
            Console.WriteLine("===================================");

            if (Offers == null || Offers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }

            foreach (SpecialOffer s in Offers)
            {
                Console.WriteLine(s);
            }
        }

        public void DisplayMenu()
        {
            Console.WriteLine($"Menu for {RestaurantName}");
            Console.WriteLine("===================================");

            if (Menu == null || Menu.Count == 0)
            {
                Console.WriteLine("No food items available.");
                return;
            }

            foreach (FoodItem f in Menu)
            {
                Console.WriteLine(f);
            }
        }

        public bool AddMenu(FoodItem item)
        {
            if (item == null)
            {
                return false;
            }

            Menu.Add(item);
            return true;
        }

        public bool RemoveMenu()
        {
            if (Menu == null || Menu.Count == 0)
            {
                return false;
            }

            Menu.Clear();
            return true;
        }

       
        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId}) - {RestaurantEmail}";
        }


    }
}
