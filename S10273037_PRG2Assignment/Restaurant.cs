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
        public List<SpecialOffer> SpecialOffers { get; set; }

        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
            Menu = new List<FoodItem>();
            OrderQueue = new Queue<Order>();
            SpecialOffers = new List<SpecialOffer>();
        }
       
    }
}
