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
        static Stack<Order> refundStack = new Stack<Order>();

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

        static List<string> ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string current = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            fields.Add(current);
            return fields;
        }

        static void LoadOrders()
        {
            try
            {
                string[] lines = File.ReadAllLines("orders.csv");
                int count = 0;

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    List<string> fields = ParseCsvLine(line);

                    if (fields.Count >= 9)
                    {
                        int orderId = int.Parse(fields[0].Trim());
                        string customerEmail = fields[1].Trim();
                        string restaurantId = fields[2].Trim();
                        string deliveryDate = fields[3].Trim();
                        string deliveryTime = fields[4].Trim();
                        string deliveryAddress = fields[5].Trim();
                        string paymentMethod = fields[6].Trim();
                        double orderTotal = double.Parse(fields[7].Trim());
                        string status = fields[8].Trim();

                        DateTime deliveryDateTime = DateTime.Parse($"{deliveryDate} {deliveryTime}");
                        DateTime orderDateTime = DateTime.Now;

                        Order order = new Order(orderId, orderDateTime, orderTotal, status,
                                              deliveryDateTime, deliveryAddress, paymentMethod, true);
                        order.CustomerEmail = customerEmail;

                        if (fields.Count >= 10 && !string.IsNullOrEmpty(fields[9].Trim()))
                        {
                            string itemsField = fields[9].Trim();
                            string[] itemEntries = itemsField.Split('|');
                            Restaurant r = FindRestaurantById(restaurantId);

                            foreach (string entry in itemEntries)
                            {
                                string[] parts = entry.Split(',');
                                if (parts.Length >= 2)
                                {
                                    string itemName = parts[0].Trim();
                                    if (int.TryParse(parts[1].Trim(), out int qty))
                                    {
                                        FoodItem fi = null;
                                        if (r != null)
                                        {
                                            foreach (FoodItem f in r.Menu)
                                            {
                                                if (f.ItemName == itemName)
                                                {
                                                    fi = f;
                                                    break;
                                                }
                                            }
                                        }
                                        if (fi == null)
                                            fi = new FoodItem(itemName, "", 0);

                                        order.AddOrderedFoodItem(new OrderedFoodItem(fi, qty, fi.ItemPrice * qty));
                                    }
                                }
                            }
                        }

                        Restaurant restaurant = FindRestaurantById(restaurantId);
                        if (restaurant != null)
                            restaurant.OrderQueue.Enqueue(order);

                        Customer customer = FindCustomerByEmail(customerEmail);
                        if (customer != null)
                            customer.AddOrder(order);

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

        static Restaurant FindRestaurantById(string restaurantId)
        {
            foreach (Restaurant restaurant in restaurantList)
            {
                if (restaurant.RestaurantId == restaurantId)
                    return restaurant;
            }
            return null;
        }

        static Customer FindCustomerByEmail(string email)
        {
            foreach (Customer customer in customerList)
            {
                if (customer.EmailAddress == email)
                    return customer;
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
                Console.WriteLine("7. Bulk process unprocessed orders");
                Console.WriteLine("8. Display total order amount");
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
                        case 7:
                            BulkProcessPendingOrders();
                            break;
                        case 8:
                            DisplayOrderAmount();
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

        static void ListAllOrders()
        {
            Console.WriteLine("\nAll Orders");
            Console.WriteLine("==========");
            Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-20} {"Delivery Date/Time",-20} {"Amount",-10} Status");
            Console.WriteLine($"{new string('-', 8),-10} {new string('-', 10),-15} {new string('-', 13),-20} {new string('-', 18),-20} {new string('-', 6),-10} {new string('-', 9)}");

            foreach (Restaurant r in restaurantList)
            {
                foreach (Order o in r.OrderQueue)
                {
                    Customer c = FindCustomerByEmail(o.CustomerEmail);
                    string customerName = c != null ? c.CustomerName : "Unknown";
                    Console.WriteLine($"{o.OrderId,-10} {customerName,-15} {r.RestaurantName,-20} {o.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"),-20} ${o.OrderTotal:F2,-9} {o.OrderStatus}");
                }
            }
        }

        static void CreateOrder()
        {
            Console.WriteLine("\nCreate New Order");
            Console.WriteLine("================");

            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine().Trim();
            Customer customer = FindCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.Write("Enter Restaurant ID: ");
            string restId = Console.ReadLine().Trim();
            Restaurant restaurant = FindRestaurantById(restId);
            if (restaurant == null)
            {
                Console.WriteLine("Restaurant not found.");
                return;
            }

            Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
            string date = Console.ReadLine();
            Console.Write("Enter Delivery Time (hh:mm): ");
            string time = Console.ReadLine();
            DateTime deliveryDT;
            if (!DateTime.TryParse($"{date} {time}", out deliveryDT))
            {
                Console.WriteLine("Error: Invalid date or time format.");
                return;
            }

            Console.Write("Enter Delivery Address: ");
            string address = Console.ReadLine();

            List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();
            Console.WriteLine("\nAvailable Food Items:");
            for (int i = 0; i < restaurant.Menu.Count; i++)
            {
                FoodItem fi = restaurant.Menu[i];
                Console.WriteLine($"{i + 1}. {fi.ItemName} - ${fi.ItemPrice:F2}");
            }

            while (true)
            {
                Console.Write("Enter item number (0 to finish): ");
                int itemChoice;
                if (!int.TryParse(Console.ReadLine(), out itemChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }
                if (itemChoice == 0) break;
                if (itemChoice < 1 || itemChoice > restaurant.Menu.Count)
                {
                    Console.WriteLine("Invalid item number. Please try again.");
                    continue;
                }

                Console.Write("Enter quantity: ");
                int qty;
                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity. Please try again.");
                    continue;
                }

                FoodItem selected = restaurant.Menu[itemChoice - 1];
                orderedItems.Add(new OrderedFoodItem(selected, qty, selected.ItemPrice * qty));
            }

            if (orderedItems.Count == 0)
            {
                Console.WriteLine("No items selected. Order cancelled.");
                return;
            }

            Console.Write("Add special request? [Y/N]: ");
            string specialReq = "";
            if (Console.ReadLine().ToUpper() == "Y")
            {
                Console.Write("Enter special request: ");
                specialReq = Console.ReadLine();
            }

            double subtotal = 0;
            foreach (OrderedFoodItem ofi in orderedItems)
                subtotal += ofi.GetSubtotal();

            double deliveryFee = 5.0;
            double total = subtotal + deliveryFee;

            Console.WriteLine($"Order Total: ${subtotal:F2} + ${deliveryFee:F2} (delivery) = ${total:F2}");
            Console.Write("Proceed to payment? [Y/N]: ");
            if (Console.ReadLine().ToUpper() != "Y") return;

            Console.Write("Payment method:\n[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            string payment = Console.ReadLine().ToUpper().Trim();
            if (payment != "CC" && payment != "PP" && payment != "CD")
            {
                Console.WriteLine("Invalid payment method.");
                return;
            }

            int newOrderId = 1000;
            foreach (Restaurant r in restaurantList)
                foreach (Order o in r.OrderQueue)
                    if (o.OrderId > newOrderId) newOrderId = o.OrderId;
            newOrderId++;

            Order newOrder = new Order(newOrderId, DateTime.Now, total, "Pending",
                                       deliveryDT, address, payment, true);
            newOrder.CustomerEmail = email;

            foreach (OrderedFoodItem ofi in orderedItems)
                newOrder.AddOrderedFoodItem(ofi);

            restaurant.OrderQueue.Enqueue(newOrder);
            customer.AddOrder(newOrder);

            File.AppendAllText("orders.csv",
                $"\n{newOrderId},{email},{restId},{date},{time},{address},{payment},{total},Pending,");

            Console.WriteLine($"Order {newOrderId} created successfully! Status: Pending");
        }

        static void ProcessOrder()
        {
            Console.WriteLine("\nProcess Order");
            Console.WriteLine("=============");
            Console.Write("Enter Restaurant ID: ");
            string restaurantId = Console.ReadLine().Trim();

            Restaurant restaurant = FindRestaurantById(restaurantId);
            if (restaurant == null)
            {
                Console.WriteLine("Error: Restaurant not found!");
                return;
            }

            if (restaurant.OrderQueue.Count == 0)
            {
                Console.WriteLine("No orders to process for this restaurant.");
                return;
            }

            Queue<Order> tempQueue = new Queue<Order>();

            while (restaurant.OrderQueue.Count > 0)
            {
                Order order = restaurant.OrderQueue.Dequeue();
                Console.WriteLine($"\nOrder {order.OrderId}:");

                Customer customer = FindCustomerByEmail(order.CustomerEmail);
                string customerName = customer != null ? customer.CustomerName : "Unknown";
                Console.WriteLine($"Customer: {customerName}");

                Console.WriteLine("Ordered Items:");
                int itemNum = 1;
                foreach (OrderedFoodItem item in order.OrderedFoodItems)
                {
                    Console.WriteLine($"{itemNum}. {item.ItemName} - {item.QtyOrdered}");
                    itemNum++;
                }

                Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
                Console.WriteLine($"Order Status: {order.OrderStatus}");

                Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
                string action = Console.ReadLine().ToUpper().Trim();

                switch (action)
                {
                    case "C":
                        if (order.OrderStatus == "Pending")
                        {
                            order.OrderStatus = "Preparing";
                            Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                        }
                        else
                        {
                            Console.WriteLine("Error: Can only confirm orders with 'Pending' status.");
                        }
                        break;

                    case "R":
                        if (order.OrderStatus == "Pending")
                        {
                            order.OrderStatus = "Rejected";
                            refundStack.Push(order);
                            Console.WriteLine($"Order {order.OrderId} rejected. Refund of ${order.OrderTotal:F2} processed.");
                        }
                        else
                        {
                            Console.WriteLine("Error: Can only reject orders with 'Pending' status.");
                        }
                        break;

                    case "S":
                        if (order.OrderStatus == "Cancelled")
                        {
                            Console.WriteLine($"Order {order.OrderId} skipped.");
                        }
                        else
                        {
                            Console.WriteLine("Error: Can only skip orders with 'Cancelled' status.");
                        }
                        break;

                    case "D":
                        if (order.OrderStatus == "Preparing")
                        {
                            order.OrderStatus = "Delivered";
                            Console.WriteLine($"Order {order.OrderId} delivered. Status: Delivered");
                        }
                        else
                        {
                            Console.WriteLine("Error: Can only deliver orders with 'Preparing' status.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid action. Order skipped.");
                        break;
                }

                tempQueue.Enqueue(order);
            }

            while (tempQueue.Count > 0)
                restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
        }

        static void ModifyOrder()
        {
            Console.WriteLine("\nModify Order");
            Console.WriteLine("============");
            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine().Trim();
            Customer c = FindCustomerByEmail(email);
            if (c == null)
            {
                Console.WriteLine("Error: Customer not found.");
                return;
            }

            List<Order> pending = c.GetPendingOrders();
            if (pending.Count == 0)
            {
                Console.WriteLine("No pending orders.");
                return;
            }

            Console.WriteLine("Pending Orders:");
            foreach (Order o in pending)
                Console.WriteLine(o.OrderId);

            Console.Write("Enter Order ID: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Error: Invalid Order ID.");
                return;
            }

            Order order = pending.Find(o => o.OrderId == id);
            if (order == null)
            {
                Console.WriteLine("Error: Order not found.");
                return;
            }

            Console.WriteLine("Order Items:");
            int itemNum = 1;
            foreach (OrderedFoodItem item in order.OrderedFoodItems)
            {
                Console.WriteLine($"{itemNum}. {item.ItemName} - {item.QtyOrdered}");
                itemNum++;
            }
            Console.WriteLine($"Address:\n{order.DeliveryAddress}");
            Console.WriteLine($"Delivery Date/Time:\n{order.DeliveryDateTime:d/M/yyyy, HH:mm}");

            Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Error: Invalid option.");
                return;
            }

            if (choice == 1)
            {
                Console.WriteLine("Feature coming soon.");
            }
            else if (choice == 2)
            {
                Console.Write("Enter new address: ");
                order.DeliveryAddress = Console.ReadLine();
                Console.WriteLine($"Order {order.OrderId} updated. New Address: {order.DeliveryAddress}");
            }
            else if (choice == 3)
            {
                Console.Write("Enter new Delivery Time (hh:mm): ");
                string t = Console.ReadLine();
                string[] timeParts = t.Split(':');
                if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hrs) && int.TryParse(timeParts[1], out int mins))
                {
                    order.DeliveryDateTime = new DateTime(
                        order.DeliveryDateTime.Year,
                        order.DeliveryDateTime.Month,
                        order.DeliveryDateTime.Day,
                        hrs, mins, 0);
                    Console.WriteLine($"Order {order.OrderId} updated. New Delivery Time: {t}");
                }
                else
                {
                    Console.WriteLine("Error: Invalid time format.");
                }
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        static void DeleteOrder()
        {
            Console.WriteLine("\nDelete Order");
            Console.WriteLine("============");
            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine().Trim();

            Customer customer = FindCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("Error: Customer not found!");
                return;
            }

            List<Order> pendingOrders = new List<Order>();
            Console.WriteLine("Pending Orders:");
            foreach (Order order in customer.Orders)
            {
                if (order.OrderStatus == "Pending")
                {
                    pendingOrders.Add(order);
                    Console.WriteLine(order.OrderId);
                }
            }

            if (pendingOrders.Count == 0)
            {
                Console.WriteLine("No pending orders to delete.");
                return;
            }

            Console.Write("Enter Order ID: ");
            int orderId;
            if (!int.TryParse(Console.ReadLine(), out orderId))
            {
                Console.WriteLine("Error: Invalid Order ID format!");
                return;
            }

            Order orderToDelete = null;
            foreach (Order order in pendingOrders)
            {
                if (order.OrderId == orderId)
                {
                    orderToDelete = order;
                    break;
                }
            }

            if (orderToDelete == null)
            {
                Console.WriteLine("Error: Order not found or not in pending status!");
                return;
            }

            Console.WriteLine($"Customer: {customer.CustomerName}");
            Console.WriteLine("Ordered Items:");
            int itemNum = 1;
            foreach (OrderedFoodItem item in orderToDelete.OrderedFoodItems)
            {
                Console.WriteLine($"{itemNum}. {item.ItemName} - {item.QtyOrdered}");
                itemNum++;
            }
            Console.WriteLine($"Delivery date/time: {orderToDelete.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${orderToDelete.OrderTotal:F2}");
            Console.WriteLine($"Order Status: {orderToDelete.OrderStatus}");

            Console.Write("Confirm deletion? [Y/N]: ");
            string confirm = Console.ReadLine().ToUpper().Trim();

            if (confirm == "Y")
            {
                orderToDelete.OrderStatus = "Cancelled";
                refundStack.Push(orderToDelete);
                Console.WriteLine($"Order {orderToDelete.OrderId} cancelled. Refund of ${orderToDelete.OrderTotal:F2} processed.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        static void BulkProcessPendingOrders()
        {
            Console.WriteLine("\n===== Bulk Process Pending Orders =====");

            int totalPending = 0;
            int totalProcessed = 0;
            int preparingCount = 0;
            int rejectedCount = 0;
            DateTime currentTime = DateTime.Now;

            Console.WriteLine("Scanning all restaurants for pending orders...\n");

            foreach (Restaurant restaurant in restaurantList)
            {
                Queue<Order> tempQueue = new Queue<Order>();
                while (restaurant.OrderQueue.Count > 0)
                {
                    Order order = restaurant.OrderQueue.Dequeue();
                    if (order.OrderStatus == "Pending")
                        totalPending++;
                    tempQueue.Enqueue(order);
                }
                while (tempQueue.Count > 0)
                    restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
            }

            Console.WriteLine($"Total Pending Orders: {totalPending}");

            if (totalPending == 0)
            {
                Console.WriteLine("No pending orders to process.");
                return;
            }

            Console.WriteLine("\nProcessing orders...\n");

            foreach (Restaurant restaurant in restaurantList)
            {
                Queue<Order> tempQueue = new Queue<Order>();

                while (restaurant.OrderQueue.Count > 0)
                {
                    Order order = restaurant.OrderQueue.Dequeue();

                    if (order.OrderStatus == "Pending")
                    {
                        TimeSpan timeDiff = order.DeliveryDateTime - currentTime;

                        if (timeDiff.TotalHours < 1)
                        {
                            order.OrderStatus = "Rejected";
                            refundStack.Push(order);
                            rejectedCount++;
                            totalProcessed++;
                            Console.WriteLine($"Order {order.OrderId} REJECTED (Delivery time less than 1 hour)");
                            Console.WriteLine($"  Customer: {order.CustomerEmail}");
                            Console.WriteLine($"  Delivery Time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                            Console.WriteLine($"  Refund: ${order.OrderTotal:F2}");
                            Console.WriteLine();
                        }
                        else
                        {
                            order.OrderStatus = "Preparing";
                            preparingCount++;
                            totalProcessed++;
                            Console.WriteLine($"Order {order.OrderId} set to PREPARING");
                            Console.WriteLine($"  Customer: {order.CustomerEmail}");
                            Console.WriteLine($"  Delivery Time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                            Console.WriteLine();
                        }
                    }

                    tempQueue.Enqueue(order);
                }

                while (tempQueue.Count > 0)
                    restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
            }

            Console.WriteLine("\n===== Processing Summary =====");
            Console.WriteLine($"Total Orders Processed: {totalProcessed}");
            Console.WriteLine($"Orders set to Preparing: {preparingCount}");
            Console.WriteLine($"Orders Rejected: {rejectedCount}");

            int totalOrders = 0;
            foreach (Restaurant restaurant in restaurantList)
                totalOrders += restaurant.OrderQueue.Count;

            if (totalOrders > 0)
            {
                double percentage = (totalProcessed * 100.0) / totalOrders;
                Console.WriteLine($"Percentage of orders automatically processed: {percentage:F2}%");
            }
            else
            {
                Console.WriteLine("Percentage of orders automatically processed: 0.00%");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void DisplayOrderAmount()
        {
            Console.WriteLine("\n===== Total Order Amounts =====");

            double grandTotalOrders = 0;
            double grandTotalRefunds = 0;
            double deliveryFee = 5.0;

            foreach (Restaurant r in restaurantList)
            {
                double restaurantTotal = 0;
                double restaurantRefunds = 0;

                Console.WriteLine($"\nRestaurant: {r.RestaurantName}");

                foreach (Order o in r.OrderQueue)
                {
                    if (o.OrderStatus == "Delivered")
                    {
                        restaurantTotal += o.OrderTotal - deliveryFee;
                    }
                    else if (o.OrderStatus == "Rejected" || o.OrderStatus == "Cancelled")
                    {
                        restaurantRefunds += o.OrderTotal;
                    }
                }

                Console.WriteLine($"  Total Order Amount (excl. delivery): ${restaurantTotal:F2}");
                Console.WriteLine($"  Total Refunds: ${restaurantRefunds:F2}");

                grandTotalOrders += restaurantTotal;
                grandTotalRefunds += restaurantRefunds;
            }

            double gruberooFee = grandTotalOrders * 0.30;

            Console.WriteLine("\n===== Summary =====");
            Console.WriteLine($"Total Order Amount: ${grandTotalOrders:F2}");
            Console.WriteLine($"Total Refunds: ${grandTotalRefunds:F2}");
            Console.WriteLine($"Gruberoo Earnings (30%): ${gruberooFee:F2}");
        }
    }
}