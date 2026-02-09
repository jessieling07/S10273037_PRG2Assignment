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
    public class OrderedFoodItem: FoodItem
        // OrderedFoodItem is the Association Class (between Oder and FoodItem)
    {
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

     

        public OrderedFoodItem(FoodItem foodItem,int QtyOrdered, double SubTotal) : base( foodItem.ItemName, foodItem.ItemDesc, foodItem.ItemPrice, foodItem.Customise)
        {
            this.QtyOrdered = QtyOrdered;
            this.SubTotal = SubTotal;
        }
         
        public double GetSubtotal()
        {
            return ItemPrice * QtyOrdered;
        }
    }
   
}
