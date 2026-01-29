using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//==========================================================
// Student Number : S10274083K
// Student Name : Lu Sijin
// Partner Name : Jessie Ling
//==========================================================

namespace S10273037_PRG2Assignment
{
    public class OrderedFoodItem
        // OrderedFoodItem is the Association Class (between Oder and FoodItem)
    {
        public FoodItem Item { get; set; }
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

        public OrderedFoodItem(FoodItem item, int qty)
        {
            Item = item;
            QtyOrdered = qty;
        }
        public double GetSubtotal()
        {
            return Item.ItemPrice * QtyOrdered;
        }
    }
}
