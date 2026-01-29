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
    public class FoodItem
    {
        private string itemName;
        private string itemDesc;
        private double itemPrice;
        private string customise;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }

        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise = "")
        {
            this.itemName = itemName;
            this.itemDesc = itemDesc;
            this.itemPrice = itemPrice;
            this.customise = customise;
        }

        public string ToString()
        {
            if (!string.IsNullOrEmpty(customise))
            {
                return $"{itemName} - {itemDesc} (${itemPrice:F2}) | Customise: {customise}";
            }
            else
            {
                return $"{itemName} - {itemDesc} (${itemPrice:F2})";
            }
        }
    }
}
