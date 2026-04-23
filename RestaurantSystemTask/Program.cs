using System;
using RestaurantSystem.Services;

namespace RestaurantSystem
{
    class Program
    {
        static RestaurantManager _manager = new RestaurantManager();
        
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n==================================");
                Console.WriteLine("    RESTAURANT MANAGEMENT SYSTEM  ");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Manage Ingredients");
                Console.WriteLine("2. Manage Dishes");
                Console.WriteLine("3. Manage Orders");
                Console.WriteLine("4. Exit");
                Console.WriteLine("==================================");
                Console.Write("Select an option: ");
                
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        IngredientsMenu();
                        break;
                    case "2":
                        DishMenu();
                        break;
                    case "3":
                        OrderMenu();
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("Exiting application...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        
        // Sub-Menus
        static void IngredientsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- INGREDIENT MENU ---");
                Console.WriteLine("1. View All Ingredients");
                Console.WriteLine("2. Add Ingredient");
                Console.WriteLine("3. Update Ingredient");
                Console.WriteLine("4. Delete Ingredient");
                Console.WriteLine("5. Search Ingredients");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        var ingredients = _manager.GetAllIngredients();
                        Console.WriteLine("\nIngredients List:");
                        foreach (var i in ingredients) Console.WriteLine(i);
                        break;
                    case "2":
                        Console.Write("Enter ingredient name: ");
                        var name = Console.ReadLine();
                        _manager.AddIngredient(name);
                        Console.WriteLine("Ingredient added successfully.");
                        break;
                    case "3":
                        int idToUpdate = ReadInt("Enter Ingredient ID to update: ");
                        Console.Write("Enter new name: ");
                        var newName = Console.ReadLine();
                        _manager.UpdateIngredient(idToUpdate, newName);
                        Console.WriteLine("Ingredient updated successfully.");
                        break;
                    case "4":
                        int idToDelete = ReadInt("Enter Ingredient ID to delete: ");
                        try
                        {
                            bool success = _manager.DeleteIngredient(idToDelete);
                            Console.WriteLine(success ? "Ingredient deleted successfully." : "Ingredient not found.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error: {e.Message}");
                        }
                        break;
                    case "5":
                        Console.Write("Enter search keyword: ");
                        var keyword = Console.ReadLine();
                        var results = _manager.SearchIngredients(keyword);
                        Console.WriteLine($"\nSearch Results for '{keyword}':");
                        foreach (var i in results) Console.WriteLine(i);
                        break;
                    case "6":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        
        static void DishMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- DISH MENU ---");
                Console.WriteLine("1. View a Dish");
                Console.WriteLine("2. Add Dish");
                Console.WriteLine("3. Update Dish Details");
                Console.WriteLine("4. Manage Dish Ingredients (Add/Remove)");
                Console.WriteLine("5. Delete Dish");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        int dishId = ReadInt("Enter Dish ID: ");
                        var dish = _manager.GetDish(dishId);
                        Console.WriteLine(dish != null ? $"\n{dish}" : "Dish not found.");
                        break;
                    case "2":
                        Console.Write("Enter dish name: ");
                        string name = Console.ReadLine();
                        decimal price = ReadDecimal("Enter dish price: ");
                        int prepTime = ReadInt("Enter prep time (minutes): ");
                        var newDish = _manager.AddDish(name, price, prepTime);
                        Console.WriteLine($"Dish added with ID: {newDish.Id}");
                        break;
                    case "3":
                        int idToUpdate = ReadInt("Enter Dish ID to update: ");
                        Console.Write("Enter new name (leave empty to skip): ");
                        string newName = Console.ReadLine();
                        
                        Console.Write("Enter new price (leave empty to skip): ");
                        string priceInput = Console.ReadLine();
                        decimal? newPrice = string.IsNullOrWhiteSpace(priceInput) ? (decimal?)null : decimal.Parse(priceInput);

                        Console.Write("Enter new prep time (leave empty to skip): ");
                        string prepInput = Console.ReadLine();
                        int? newPrep = string.IsNullOrWhiteSpace(prepInput) ? (int?)null : int.Parse(prepInput);

                        _manager.UpdateDish(idToUpdate, newName, newPrice, newPrep);
                        Console.WriteLine("Dish updated.");
                        break;
                    case "4":
                        ManageDishIngredientsMenu();
                        break;
                    case "5":
                        int idToDelete = ReadInt("Enter Dish ID to delete: ");
                        _manager.DeleteDish(idToDelete);
                        Console.WriteLine("Dish deleted (if it existed).");
                        break;
                    case "6":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ManageDishIngredientsMenu()
        {
            int dishId = ReadInt("Enter Dish ID to modify its ingredients: ");
            if (_manager.GetDish(dishId) == null)
            {
                Console.WriteLine("Dish not found.");
                return;
            }

            Console.WriteLine("1. Add Ingredient to Dish");
            Console.WriteLine("2. Remove Ingredient from Dish");
            Console.Write("Select option: ");
            string choice = Console.ReadLine();

            int ingId = ReadInt("Enter Ingredient ID: ");

            if (choice == "1")
            {
                _manager.AddIngredientToDish(dishId, ingId);
                Console.WriteLine("Ingredient added to dish.");
            }
            else if (choice == "2")
            {
                _manager.RemoveIngredientFromDish(dishId, ingId);
                Console.WriteLine("Ingredient removed from dish.");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        static void OrderMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n--- ORDER MENU ---");
                Console.WriteLine("1. View Order");
                Console.WriteLine("2. Create New Order");
                Console.WriteLine("3. Update Order Dish Quantity");
                Console.WriteLine("4. Change Order Table Number");
                Console.WriteLine("5. Override Order Total Cost");
                Console.WriteLine("6. Delete Order");
                Console.WriteLine("7. Search Orders by Dish Name");
                Console.WriteLine("8. Back to Main Menu");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        int viewId = ReadInt("Enter Order ID: ");
                        var order = _manager.GetOrder(viewId);
                        Console.WriteLine(order != null ? $"\n{order}" : "Order not found.");
                        break;
                    case "2":
                        int table = ReadInt("Enter Table Number: ");
                        var newOrder = _manager.AddOrder(table);
                        Console.WriteLine($"Order created with ID: {newOrder.Id}");
                        break;
                    case "3":
                        int orderId = ReadInt("Enter Order ID: ");
                        int dishId = ReadInt("Enter Dish ID: ");
                        int qty = ReadInt("Enter Quantity (0 to remove): ");
                        _manager.UpdateOrderDishQuantity(orderId, dishId, qty);
                        Console.WriteLine("Order updated.");
                        break;
                    case "4":
                        int oIdTable = ReadInt("Enter Order ID: ");
                        int newTable = ReadInt("Enter new Table Number: ");
                        _manager.UpdateOrderTable(oIdTable, newTable);
                        Console.WriteLine("Table updated.");
                        break;
                    case "5":
                        int oIdTotal = ReadInt("Enter Order ID: ");
                        decimal newTotal = ReadDecimal("Enter new Total Cost override: ");
                        _manager.UpdateOrderTotalCost(oIdTotal, newTotal);
                        Console.WriteLine("Total cost overridden.");
                        break;
                    case "6":
                        int idToDelete = ReadInt("Enter Order ID to delete: ");
                        _manager.DeleteOrder(idToDelete);
                        Console.WriteLine("Order deleted.");
                        break;
                    case "7":
                        Console.Write("Enter dish name to search in orders: ");
                        string keyword = Console.ReadLine();
                        var results = _manager.SearchOrders(keyword);
                        Console.WriteLine($"\nFound {results.Count} orders containing '{keyword}':");
                        foreach (var o in results) Console.WriteLine(o);
                        break;
                    case "8":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        
        // Helper Methods
        static int ReadInt(string prompt)
        {
            int result;
            Console.Write(prompt);
            
            while (!int.TryParse(Console.ReadLine(), out result))
                Console.Write("Invalid input. Please enter a valid whole number: ");
            return result;
        }

        static decimal ReadDecimal(string prompt)
        {
            decimal result;
            Console.Write(prompt);
            
            while (!decimal.TryParse(Console.ReadLine(), out result))
                Console.Write("Invalid input. Please enter a valid number (e.g. 10.50): ");
            return result;    
        }
    }
}