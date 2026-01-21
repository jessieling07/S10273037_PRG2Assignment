using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10273037B
// Student Name : Jessie Ling
// Partner Name : Lu SIjin
//==========================================================

namespace S10273037_PRG2Assignment
{
    

    public class Menu
    {
        private string menuId;
        private string menuName;
        private List<FoodItem> foodItems;

        public Menu(string menuId, string menuName)
        {
            this.menuId = menuId;
            this.menuName = menuName;
            this.foodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            foodItems.Add(foodItem);
        }

        public bool RemoveFoodItem(FoodItem foodItem)
        {
            return foodItems.Remove(foodItem);
        }

        public void DisplayFoodItems()
        {
            Console.WriteLine($"\n{menuName} Menu:");
            foreach (FoodItem item in foodItems)
            {
                Console.WriteLine($"  - {item.ToString()}");
            }
        }

        public string ToString()
        {
            return $"Menu ID: {menuId}, Name: {menuName}, Items: {foodItems.Count}";
        }
    }
}
