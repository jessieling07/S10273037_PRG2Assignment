//==========================================================
// Student Number : S10273037
// Student Name : Jessie Ling
// Partner Name : Lu Sijin
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

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
                Console.WriteLine("8. Display order amount");
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

                        // advanced b
                        double deliveryFee = 5.0;
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


        // FEATURE 5
        static void CreateOrder()
        {
            Console.WriteLine("\nCreate New Order");
            Console.WriteLine("================");

            // 1. 获取客户信息
            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine().Trim();
            Customer customer = FindCustomerByEmail(email);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            // 2. 获取餐厅信息
            Console.Write("Enter Restaurant ID: ");
            string restId = Console.ReadLine().Trim();
            Restaurant restaurant = FindRestaurantById(restId);
            if (restaurant == null)
            {
                Console.WriteLine("Restaurant not found.");
                return;
            }

            // 3. 获取配送信息
            Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
            string date = Console.ReadLine();
            Console.Write("Enter Delivery Time (hh:mm): ");
            string time = Console.ReadLine();
            DateTime deliveryDT = DateTime.Parse($"{date} {time}");

            Console.Write("Enter Delivery Address: ");
            string address = Console.ReadLine();

            // 4. 显示菜单并选择食物
            List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();
            Console.WriteLine("\nAvailable Food Items:");
            for (int i = 0; i < restaurant.Menu.Count; i++)
            {
                FoodItem fi = restaurant.Menu[i];
                Console.WriteLine($"{i + 1}. {fi.ItemName} - ${fi.ItemPrice:F2}");
            }

            // 用于存储特殊请求
            string specialRequest = "";

            while (true)
            {
                Console.Write("\nEnter item number (0 to finish): ");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (choice == 0) break;

                if (choice < 1 || choice > restaurant.Menu.Count)
                {
                    Console.WriteLine("Invalid item number. Please try again.");
                    continue;
                }

                Console.Write("Enter quantity: ");
                int qty;
                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity. Please enter a positive number.");
                    continue;
                }

                FoodItem selected = restaurant.Menu[choice - 1];

                // 检查是否需要添加特殊请求（只在第一次选择时询问）
                if (string.IsNullOrEmpty(specialRequest))
                {
                    Console.Write("Add special request for this item? [Y/N]: ");
                    string addRequest = Console.ReadLine().ToUpper();

                    if (addRequest == "Y")
                    {
                        Console.Write("Enter special request (e.g., extra toppings, no onions): ");
                        string request = Console.ReadLine().Trim();

                        // 创建一个新的FoodItem副本并添加特殊请求
                        selected = new FoodItem(
                            selected.ItemName,
                            selected.ItemDesc,
                            selected.ItemPrice,
                            request  // 设置自定义请求
                        );
                        specialRequest = request;
                    }
                }

                // 创建OrderedFoodItem - 这里使用你实际的方法名
                OrderedFoodItem orderedItem = new OrderedFoodItem(selected, qty, selected.ItemPrice);
                orderedItems.Add(orderedItem);

                Console.WriteLine($"Added {qty} x {selected.ItemName} to order.");
            }

            if (orderedItems.Count == 0)
            {
                Console.WriteLine("No items selected. Order cancelled.");
                return;
            }

            // 5. 计算订单总额
            double foodTotal = 0;
            foreach (OrderedFoodItem ofi in orderedItems)
                foodTotal += ofi.GetSubtotal();  // 使用 GetSubtotal() 而不是 CalculateSubtotal()

            double deliveryFee = 5.0;
            double total = foodTotal + deliveryFee;

            Console.WriteLine($"\nOrder Summary:");
            Console.WriteLine($"Food Total: ${foodTotal:F2}");
            Console.WriteLine($"Delivery Fee: ${deliveryFee:F2}");
            Console.WriteLine($"Order Total: ${total:F2}");

            if (!string.IsNullOrEmpty(specialRequest))
            {
                Console.WriteLine($"Special Request: {specialRequest}");
            }

            // 6. 支付确认
            Console.Write("\nProceed to payment? [Y/N]: ");
            string proceed = Console.ReadLine().ToUpper();
            if (proceed != "Y")
            {
                Console.WriteLine("Order cancelled.");
                return;
            }

            // 7. 选择支付方式
            string payment = "";
            while (true)
            {
                Console.WriteLine("\nPayment method:");
                Console.WriteLine("[CC] Credit Card");
                Console.WriteLine("[PP] PayPal");
                Console.WriteLine("[CD] Cash on Delivery");
                Console.Write("Choose payment method: ");

                payment = Console.ReadLine().ToUpper();

                if (payment == "CC" || payment == "PP" || payment == "CD")
                    break;
                else
                    Console.WriteLine("Invalid payment method. Please enter CC, PP, or CD.");
            }

            // 8. 生成订单ID
            int newOrderId = 1000;
            foreach (Restaurant r in restaurantList)
            {
                foreach (Order o in r.OrderQueue)
                {
                    if (o.OrderId >= newOrderId)
                        newOrderId = o.OrderId + 1;
                }
            }

            // 9. 创建订单
            Order newOrder = new Order(
                newOrderId,
                DateTime.Now,
                total,
                "Pending",
                deliveryDT,
                address,
                payment,
                true  // 假设已经支付
            );

            // 添加已订购的食物项目
            foreach (OrderedFoodItem ofi in orderedItems)
                newOrder.AddOrderedFoodItem(ofi);

            // 10. 添加到餐厅队列和客户订单列表
            restaurant.OrderQueue.Enqueue(newOrder);
            customer.AddOrder(newOrder);

            // 11. 保存到CSV文件
            try
            {
                // 格式化日期时间用于CSV
                string orderDateTimeStr = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                string deliveryDateStr = deliveryDT.ToString("dd/MM/yyyy");
                string deliveryTimeStr = deliveryDT.ToString("HH:mm");

                string csvLine = $"\n{newOrderId},{email},{restId},{deliveryDateStr},{deliveryTimeStr},{address},{orderDateTimeStr},{total},Pending";
                File.AppendAllText("orders.csv", csvLine);

                Console.WriteLine($"\nOrder {newOrderId} created successfully! Status: Pending");
                Console.WriteLine($"Order has been added to {restaurant.RestaurantName}'s queue.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving order to file: {ex.Message}");
            }
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

                Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
                Console.WriteLine($"Order status: {order.OrderStatus}");

                Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
                string action = Console.ReadLine().ToUpper().Trim();

                switch (action)
                {
                    case "C":

                        if (order.OrderStatus == "Pending")
                        {
                            order.OrderStatus = "Preparing";
                        }
                        else
                        {
                            Console.WriteLine($"Error: Can only confirm orders with 'Pending' status.");
                        }
                        break;

                    case "R":

                        if (order.OrderStatus == "Pending")
                        {
                            order.OrderStatus = "Rejected";
                            Console.WriteLine($"Order {order.OrderId} has been rejected. Refound of ${order.OrderTotal:F2} processed.");
                        }
                        else
                        {
                            Console.WriteLine($"Error: Can only reject orders with 'Pending' status.");
                        }
                        break;

                    case "S":
                        if (order.OrderStatus == "Cancelled")
                        {
                            Console.WriteLine($"Order {order.OrderId} skipped. ");
                        }
                        else
                        {
                            Console.WriteLine($"Error: Can onyl skip orders with 'Cancelled' status. ");
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
                            Console.WriteLine($"Error: Can only deliver orders with 'Preparing' status.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid action. Order skipped.");
                        break;

                }

                tempQueue.Enqueue(order);

            }

            while (tempQueue.Count > 0)
            {
                restaurant.OrderQueue.Enqueue(tempQueue.Dequeue());
            }
        }


        // FEATURE 7
        static void ModifyOrder()
        {
            Console.Write("\nEnter Customer Email: ");
            string email = Console.ReadLine();
            Customer c = FindCustomerByEmail(email);
            if (c == null)
            {
                Console.WriteLine("Customer not found.");
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
            int id = int.Parse(Console.ReadLine());
            Order order = pending.Find(o => o.OrderId == id);
            if (order == null) return;

            Console.WriteLine("[1] Address [2] Delivery Time");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                Console.Write("Enter new address: ");
                order.DeliveryAddress = Console.ReadLine();
            }
            else if (choice == 2)
            {
                Console.Write("Enter new time (hh:mm): ");
                string t = Console.ReadLine();
                order.DeliveryDateTime =
                    new DateTime(order.DeliveryDateTime.Year,
                                 order.DeliveryDateTime.Month,
                                 order.DeliveryDateTime.Day,
                                 int.Parse(t.Split(':')[0]),
                                 int.Parse(t.Split(':')[1]),
                                 0);
            }

            Console.WriteLine("Order updated successfully.");
        }



        static void DeleteOrder()
        {
            // FEATURE 8
            Console.WriteLine("\nDelete Order");
            Console.WriteLine("=============");
            Console.WriteLine("Enter Customer Email: ");
            string email = Console.ReadLine().Trim();

            Customer customer = FindCustomerByEmail(email);

            if (customer == null)
            {
                Console.WriteLine("error: Customer not found!");
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

            Console.Write("enter Order ID: ");
            int orderId;

            if (!int.TryParse(Console.ReadLine(), out orderId))
            {
                Console.WriteLine("error: Invalid Order ID format!");
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
                Console.WriteLine("error: Order not found or not in pending status!");
                return;
            }

            Console.WriteLine($"Customer: {customer.CustomerName}");
            Console.WriteLine("Ordered Items:");

            int itemNum = 1;
            foreach (var item in orderToDelete.OrderedFoodItems)
            {
                Console.WriteLine($"{itemNum}. {item.ToString()}");
                itemNum++;
            }

            Console.WriteLine($"Delivery date/time: {orderToDelete.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${orderToDelete.OrderTotal:F2}");
            Console.WriteLine($"Order Status: {orderToDelete.OrderStatus}");

            Console.Write("Confirm deletion? [Y/N]");
            string confirm = Console.ReadLine().ToUpper().Trim();

            if (confirm == "Y")
            {
                // Update order status to Cancelled
                orderToDelete.OrderStatus = "Cancelled";

                // Add to refund stack (if you have implemented it)
                // refundStack.Push(orderToDelete);

                Console.WriteLine($"Order {orderToDelete.OrderId} cancelled. Refund of ${orderToDelete.OrderTotal:F2} processed.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }

        }

      

        // advanced feature b
        static void DisplayOrderAmount()
        {
            double totalSuccessfulAmount = 0;
            double totalRefundedAmount = 0;
            int totalDeliveredOrders = 0;
            int totalRefundedOrders = 0;

            Console.WriteLine("\n===========================================");
            Console.WriteLine("        GRUBEROO FINANCIAL SUMMARY");
            Console.WriteLine("===========================================");

            // 先检查所有订单的状态和金额
            Console.WriteLine("\n=== DEBUG: All Orders Summary ===");
            int allOrdersCount = 0;
            foreach (Restaurant r in restaurantList)
            {
                Console.WriteLine($"\nRestaurant: {r.RestaurantName} - {r.OrderQueue.Count} orders");
                foreach (Order o in r.OrderQueue)
                {
                    allOrdersCount++;
                    Console.WriteLine($"  Order #{o.OrderId}: Status={o.OrderStatus}, Total=${o.OrderTotal:F2}, Customer={o.CustomerEmail}");
                }
            }
            Console.WriteLine($"Total orders in system: {allOrdersCount}");

            Console.WriteLine("\n=== FINANCIAL CALCULATION ===");
            foreach (Restaurant r in restaurantList)
            {
                Console.WriteLine($"\n--- {r.RestaurantName} ({r.RestaurantId}) ---");

                double restaurantSuccessful = 0;
                double restaurantRefunded = 0;
                int delivered = 0;
                int refunded = 0;

                foreach (Order o in r.OrderQueue)
                {
                    Console.WriteLine($"  Order #{o.OrderId}: Status={o.OrderStatus}, Total=${o.OrderTotal:F2}");

                    if (o.OrderStatus == "Delivered")
                    {
                        restaurantSuccessful += o.OrderTotal;
                        delivered++;
                        Console.WriteLine($"    -> Counted as DELIVERED: +${o.OrderTotal:F2}");
                    }
                    else if (o.OrderStatus == "Rejected" || o.OrderStatus == "Cancelled")
                    {
                        restaurantRefunded += o.OrderTotal;
                        refunded++;
                        Console.WriteLine($"    -> Counted as REFUNDED: -${o.OrderTotal:F2}");
                    }
                    else
                    {
                        Console.WriteLine($"    -> Status '{o.OrderStatus}' NOT counted");
                    }
                }

                Console.WriteLine($"  Summary: Delivered={delivered} (${restaurantSuccessful:F2}), Refunded={refunded} (${restaurantRefunded:F2})");
                Console.WriteLine($"  Restaurant Net: ${(restaurantSuccessful - restaurantRefunded):F2}");

                totalSuccessfulAmount += restaurantSuccessful;
                totalRefundedAmount += restaurantRefunded;
                totalDeliveredOrders += delivered;
                totalRefundedOrders += refunded;
            }

            Console.WriteLine("\n===========================================");
            Console.WriteLine("           OVERALL FINANCIAL SUMMARY");
            Console.WriteLine("===========================================");
            Console.WriteLine($"Delivered Orders: {totalDeliveredOrders}");
            Console.WriteLine($"Total Successful Amount: ${totalSuccessfulAmount:F2}");
            Console.WriteLine($"Refunded Orders: {totalRefundedOrders}");
            Console.WriteLine($"Total Refunded Amount: ${totalRefundedAmount:F2}");
            Console.WriteLine($"─────────────────────────────────────────────");

            double finalAmount = totalSuccessfulAmount - totalRefundedAmount;
            Console.WriteLine($"FINAL AMOUNT GRUBEROO EARNS: ${finalAmount:F2}");

            // 分析为什么是负数
            if (finalAmount < 0)
            {
                Console.WriteLine("\n=== WARNING: Negative Earnings! Analysis ===");
                Console.WriteLine($"Reason: Refunds (${totalRefundedAmount:F2}) > Success (${totalSuccessfulAmount:F2})");
                Console.WriteLine($"Difference: ${Math.Abs(finalAmount):F2}");

                if (totalDeliveredOrders == 0 && totalRefundedOrders > 0)
                {
                    Console.WriteLine("No delivered orders, only refunds!");
                }
            }

            Console.WriteLine("===========================================");
        }












    }
}

    

    
