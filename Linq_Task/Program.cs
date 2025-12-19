using Linq_Task.Data;

namespace Linq_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BikeStoresContext context = new BikeStoresContext();
            do
            {
                Console.WriteLine("1-List all customers' first and last names along with their email addresses.");
                Console.WriteLine("2- Retrieve all orders processed by a specific staff member (e.g., staff_id = 3).");
                Console.WriteLine("3- Get all products that belong to a category named \"Mountain Bikes\".");
                Console.WriteLine("4-Count the total number of orders per store.");
                Console.WriteLine("5- List all orders that have not been shipped yet (shipped_date is null).");
                Console.WriteLine("6- Display each customer’s full name and the number of orders they have placed.");
                Console.WriteLine("7- List all products that have never been ordered (not found in order_items).");
                Console.WriteLine("8- Display products that have a quantity of less than 5 in any store stock.");
                Console.WriteLine("9- Retrieve the first product from the products table.");
                Console.WriteLine("10- Retrieve all products from the products table with a certain model year.");
                Console.WriteLine("11- Display each product with the number of times it was ordered.");
                Console.WriteLine("12- Count the number of products in a specific category.");
                Console.WriteLine("13- Calculate the average list price of products.");
                Console.WriteLine("14- Retrieve a specific product from the products table by ID.");
                Console.WriteLine("15- List all products that were ordered with a quantity greater than 3 in any order.");
                Console.WriteLine("16- Display each staff member’s name and how many orders they processed.");
                Console.WriteLine("17- List active staff members only (active = true) along with their phone numbers.");
                Console.WriteLine("18- List all products with their brand name and category name.");
                Console.WriteLine("19- Retrieve orders that are completed.");
                Console.WriteLine("20- List each product with the total quantity sold (sum of quantity from order_items).");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        var customerInfo = context.Customers
                        .Select(c => new { c.FirstName, c.LastName, c.Email });
                        foreach (var customer in customerInfo)
                        {
                            Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}, Email: {customer.Email}");
                        }
                        break;
                    case "2":
                        var staffOrders = context.Orders
                        .Where(o => o.StaffId == 3)
                        .ToList();
                        foreach (var order in staffOrders)
                        {
                            Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, Order Date: {order.OrderDate}");
                        }
                        break;
                    case "3":
                        var mountainBikes = context.Products.Select(p => p.Category.CategoryName.Contains( "Mountain Bikes"));
                        foreach (var bike in mountainBikes)
                        {
                            Console.WriteLine(bike);
                        }
                        break;
                    case "4":
                        var ordersPerStore = context.Orders
                        .GroupBy(o => o.StoreId)
                        .Select(g => new { StoreId = g.Key, OrderCount = g.Count() });
                        foreach (var store in ordersPerStore)
                        {
                            Console.WriteLine($"Store ID: {store.StoreId}, Total Orders: {store.OrderCount}");
                        }
                        break;
                    case "5":
                        var unshippedOrders = context.Orders
                        .Where(o => o.ShippedDate == null)
                        .ToList();
                        foreach (var order in unshippedOrders)
                        {
                            Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, Order Date: {order.OrderDate}");
                        }
                        break;
                    case "6":
                        var customerOrders = context.Customers
                        .Select(c => new
                        {
                            FullName = c.FirstName + " " + c.LastName,
                            OrderCount = c.Orders.Count()
                        });
                        foreach (var customer in customerOrders)
                        {
                            Console.WriteLine($"Customer: {customer.FullName}, Orders Placed: {customer.OrderCount}");
                        }
                        break;
                    case"7":
                        var neverOrderedProducts = context.Products
                        .Where(p => !context.OrderItems.Any(oi => oi.ProductId == p.ProductId))
                        .ToList();
                        foreach (var product in neverOrderedProducts)
                        {
                            Console.WriteLine($"Product ID: {product.ProductId}, Product Name: {product.ProductName}");
                        }
                        break;
                    case"8":
                        var lowStockProducts = context.Stocks
                        .Where(s => s.Quantity < 5)
                        .Select(s => s.Product)
                        .Distinct()
                        .ToList();
                        foreach (var product in lowStockProducts)
                        {
                            Console.WriteLine($"Product ID: {product.ProductId}, Product Name: {product.ProductName}");
                        }
                        break;
                    case"9":
                        var firstProduct = context.Products.FirstOrDefault();
                        if (firstProduct != null)
                        {
                            Console.WriteLine($"Product ID: {firstProduct.ProductId}, Product Name: {firstProduct.ProductName}");
                        }
                        break;
                    case"10":
                        var productsByModelYear = context.Products.Where(p => p.ModelYear == 2017).ToList();
                        foreach (var product in productsByModelYear)
                        {
                            Console.WriteLine($"Product ID: {product.ProductId}, Product Name: {product.ProductName}, Model Year: {product.ModelYear}");
                        }
                        break;
                    case"11":
                        var productOrderCounts = context.Products
                        .Select(p => new
                        {
                            ProductName = p.ProductName,
                            OrderCount = p.OrderItems.Count()
                        });
                        foreach (var product in productOrderCounts)
                        {
                            Console.WriteLine($"Product: {product.ProductName}, Times Ordered: {product.OrderCount}");
                        }
                        break;
                    case"12":
                        var categoryId = 1;
                        var productCount = context.Products.Count(p => p.CategoryId == categoryId);
                        Console.WriteLine($"Total Products in Category ID {categoryId}: {productCount}");
                        break;
                    case"13":
                        var averageListPrice = context.Products.Average(p => p.ListPrice);
                        Console.WriteLine($"Average List Price of Products: {averageListPrice:C}");
                        break;
                    case"14":
                        var productId = 1;
                        var specificProduct = context.Products.SingleOrDefault(p=> p.ProductId == productId);
                        if (specificProduct != null)
                        {
                            Console.WriteLine($"Product ID: {specificProduct.ProductId}, Product Name: {specificProduct.ProductName}");
                        }
                        break;
                    case"15":
                        var productsOrderedMoreThan3 = context.OrderItems
                        .Where(oi => oi.Quantity > 3)
                        .Select(oi => oi.Product)
                        .Distinct()
                        .ToList();
                        foreach (var product in productsOrderedMoreThan3)
                        {
                            Console.WriteLine($"Product ID: {product.ProductId}, Product Name: {product.ProductName}");
                        }
                        break;
                    case"16":
                        var staffOrderCounts = context.Staffs
                        .Select(s => new
                        {
                            FullName = s.FirstName + " " + s.LastName,
                            OrderCount = s.Orders.Count()
                        });
                        foreach (var staff in staffOrderCounts)
                        {
                            Console.WriteLine($"Staff: {staff.FullName}, Orders Processed: {staff.OrderCount}");
                        }
                        break;
                    case"17":
                        var activeStaff = context.Staffs
                        .Where(s => s.Active == 1)
                        .Select(s => new { s.FirstName, s.LastName, s.Phone });
                        foreach (var staff in activeStaff)
                        {
                            Console.WriteLine($"Name: {staff.FirstName} {staff.LastName}, Phone: {staff.Phone}");
                        }
                        break;
                    case"18":
                        var productsWithBrandAndCategory = context.Products
                        .Select(p => new
                        {
                            ProductName = p.ProductName,
                            BrandName = p.Brand.BrandName,
                            CategoryName = p.Category.CategoryName
                        });
                        foreach (var product in productsWithBrandAndCategory)
                        {
                            Console.WriteLine($"Product: {product.ProductName}, Brand: {product.BrandName}, Category: {product.CategoryName}");
                        }
                        break;
                    case"19":
                        var completedOrders = context.Orders
                        .Where(o => o.OrderStatus == 4)
                        .ToList();
                        foreach (var order in completedOrders)
                        {
                            Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, Order Date: {order.OrderDate}");
                        }
                    break;
                    case"20":
                        var totalQuantitySoldPerProduct = context.Products
                        .Select(p => new
                        {
                            ProductName = p.ProductName,
                            TotalQuantitySold = p.OrderItems.Sum(oi => oi.Quantity)
                        });
                        foreach (var product in totalQuantitySoldPerProduct)
                        {
                            Console.WriteLine($"Product: {product.ProductName}, Total Quantity Sold: {product.TotalQuantitySold}");
                        }
                        break;
                    default:
                        break;
                }
            }
            while (true);
        }
    }
}
