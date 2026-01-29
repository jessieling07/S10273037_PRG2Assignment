//==========================================================
// Student Number : S10273037
// Student Name : Jessie Ling
// Partner Name : Lu Sijin
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace S10273037_PRG2Assignment
{
    class Program
    {
        static List<Restaurant> restaurantList = new List<Restaurant>();
        static List<Customer> customerList = new List<Customer>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Gruberoo Food Delivery System");

            
            LoadRestaurants();
            LoadFoodItems();

            MainMenu();
        }

        static void LoadRestaurants()
        {
            try
            {
                string[] lines = File.ReadAllLines("restaurants.csv");
                int count = 0;

                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');

                    if (data.Length >= 3)
                    {
                        string restaurantId = data[0].Trim();
                        string name = data[1].Trim();
                        string email = data[2].Trim();

                        Restaurant restaurant = new Restaurant(restaurantId, name, email);
                        restaurantList.Add(restaurant);
                        count++;
                    }
                }

                Console.WriteLine($"{count} restaurants loaded!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: restaurants.csv file not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading restaurants: {ex.Message}");
            }
        }

        static void LoadFoodItems()
        {
            try
            {
                string[] lines = File.ReadAllLines("fooditems.csv");
                int count = 0;

                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');

                    if (data.Length >= 4)
                    {
                        string restaurantId = data[0].Trim();
                        string itemName = data[1].Trim();
                        string itemDesc = data[2].Trim();
                        double itemPrice = double.Parse(data[3].Trim());

                        
                        FoodItem foodItem = new FoodItem(itemName, itemDesc, itemPrice);

                        
                        Restaurant restaurant = FindRestaurantById(restaurantId);
                        if (restaurant != null)
                        {
                            restaurant.Menu.Add(foodItem);
                            count++;
                        }
                    }
                }

                Console.WriteLine($"{count} food items loaded!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: fooditems.csv file not found!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid price format in fooditems.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading food items: {ex.Message}");
            }
        }

        static Restaurant FindRestaurantById(string restaurantId)
        {
            foreach (Restaurant restaurant in restaurantList)
            {
                if (restaurant.RestaurantId == restaurantId)
                {
                    return restaurant;
                }
            }
            return null;
        }

        static void MainMenu()
        {
            int choice = -1;

            while (choice != 0)
            {
                Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
                Console.WriteLine("1. List all restaurants and menu items");
                Console.WriteLine("2. List all orders");
                Console.WriteLine("3. Create a new order");
                Console.WriteLine("4. Process an order");
                Console.WriteLine("5. Modify an existing order");
                Console.WriteLine("6. Delete an existing order");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                try
                {
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Feature not yet implemented.");
                            break;
                        case 2:
                            ListAllOrders();
                            break;
                        case 3:
                            CreateOrder();
                            break;
                        case 4:
                            ProcessOrder();
                            break;
                        case 5:
                            ModifyOrder();
                            break;
                        case 6:
                            DeleteOrder();
                            break;
                        case 0:
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        static void ListAllRestaurantsAndMenuItems()
        {
            Console.WriteLine("\nAll Restaurants and Menu Items");
            Console.WriteLine("==============================");

            foreach (Restaurant restaurant in restaurantList)
            {
                Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");

                foreach (FoodItem item in restaurant.Menu)
                {
                    Console.WriteLine($" - {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice:F2}");
                }
                Console.WriteLine();
            }
        }

        static void ListAllOrders()
        {
            // FEATURE 2
        }
        static void CreateOrder()
        {
            // FEATURE 3
        }
        static void ProcessOrder()
        {
            // FEATURE 4
        }
        static void ModifyOrder()
        {
            // FEATURE 5
        }
        static void DeleteOrder()
        {
            // FEATURE 6
        }
    }
}
