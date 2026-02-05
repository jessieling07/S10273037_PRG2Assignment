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

            LoadCustomers();
            LoadOrders();

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
                            ListAllRestaurantsAndMenuItems();
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

        // FEATURE 2
         

        static void LoadCustomers()
        {
            try
            {
                string[] lines = File.ReadAllLines("customers.csv");
                int count = 0;

                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');

                    if (data.Length >= 2)
                    {
                        string name = data[0].Trim();
                        string email = data[1].Trim();

                        Customer customer = new Customer(email, name);
                        customerList.Add(customer);
                        count++;
                    }
                }

                Console.WriteLine($"{count} customers loaded!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: customers.csv file not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customers: {ex.Message}");
            }

        }

        static void LoadOrders()
        {
            try
            {
                string[] lines = File.ReadAllLines("orders.csv");
                int count = 0;

                
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');

                    
                    if (data.Length >= 9)
                    {
                        int orderId = int.Parse(data[0].Trim());
                        string customerEmail = data[1].Trim();
                        string restaurantId = data[2].Trim();
                        string deliveryDate = data[3].Trim();
                        string deliveryTime = data[4].Trim();
                        string deliveryAddress = data[5].Trim();
                        string paymentMethod = data[6].Trim();
                        double orderTotal = double.Parse(data[7].Trim());
                        string status = data[8].Trim();

                        
                        DateTime deliveryDateTime = DateTime.Parse($"{deliveryDate} {deliveryTime}");
                        DateTime orderDateTime = DateTime.Now;

                        
                        Order order = new Order(orderId, orderDateTime, orderTotal, status,
                                              deliveryDateTime, deliveryAddress, paymentMethod, true);

                        
                        Restaurant restaurant = FindRestaurantById(restaurantId);
                        if (restaurant != null)
                        {
                            restaurant.OrderQueue.Enqueue(order);
                        }

                      
                        Customer customer = FindCustomerByEmail(customerEmail);
                        if (customer != null)
                        {
                            customer.AddOrder(order);
                        }

                        count++;
                    }
                }

                Console.WriteLine($"{count} orders loaded!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: orders.csv file not found!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid format in orders.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading orders: {ex.Message}");
            }

        }
        static Customer FindCustomerByEmail(string email)
        {
            foreach (Customer customer in customerList)
            {
                if (customer.EmailAddress == email)
                {
                    return customer;
                }
            }
            return null;
        }

        // FEATURE 4
        
        static void ListAllOrders()
        {
            Console.WriteLine("\n===== All Orders =====");

            foreach (Restaurant r in restaurantList)
            {
                Console.WriteLine($"\nRestaurant: {r.RestaurantName}");
                r.DisplayOrders();
            }
        }
        static void ListAllRestaurantsAndMenuItems()
        {
            // FEATURE 3
            Console.WriteLine("\nAll Restaurants and Menu Items");
            Console.WriteLine("==============================");

            if (restaurantList.Count == 0)
            {
                Console.WriteLine("No restaurants found.");
                return;
            }

            foreach (Restaurant restaurant in restaurantList)
            {
                Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");

                if (restaurant.Menu.Count == 0)
                {
                    Console.WriteLine(" - No menu items available");
                }
                else
                {
                    foreach (FoodItem item in restaurant.Menu)
                    {
                        Console.WriteLine($" - {item.ItemName}: {item.ItemDesc} - ${item.ItemPrice:F2}");
                    }
                }
                Console.WriteLine(); 
            }
        }

        
        static void CreateOrder()
        {
            // FEATURE 5
        }
        static void ProcessOrder()
        {
            // FEATURE 6
            Console.WriteLine("\nProcess Order");
            Console.WriteLine("=============");
            Console.WriteLine("Enter Restaurant ID: ");
            string restaurantId = Console.ReadLine().Trim();

            Restaurant restaurant = FindRestaurantById(restaurantId);

            if (restaurant == null)
            {
                Console.WriteLine(" Error: Restaurant not found!");
                return;
            }

            if (restaurant.OrderQueue.Count == 0)
            {
                Console.WriteLine("No orders to process fo rthis restaurant.");
                return;
            }

            Queue<Order> tempQueue = new Queue<Order>();

            while (restaurant.OrderQueue.Count > 0)
            {
                Order order = restaurant.OrderQueue.Dequeue();
                Console.WriteLine($"\nOrder {order.OrderId}:");
                Customer customer = FindCustomerByEmail(order.CustomerEmail);
                if (customer != null)
                {
                    Console.WriteLine($"Customer: {customer.CustomerName}");
                }
                else
                {
                    Console.WriteLine($"Customer: Unknown");
                }

                Console.WriteLine("Ordered Items:");

                int itemNum = 1;
                foreach (var item in order.OrderedFoodItems)
                    {
                    Console.WriteLine($"{itemNum}. {item}");
                    itemNum++;
                }

            }


        }
        static void ModifyOrder()
        {
            // FEATURE 7
        }
        static void DeleteOrder()
        {
            // FEATURE 8
        }
    }
}
