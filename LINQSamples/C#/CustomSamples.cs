using Mapped;
using nwind;
using SampleSupport;
using SampleQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuerySamples
{
    [Title("LINQ Query Samples")]
    [Prefix("Linq")]
    public class CustomSamples : SampleHarness
    {
        private readonly LinqSamples sample = new LinqSamples();

        [Title("LinqQuery01")]
        [Description("A list of all customers whose total orders exceed some given value")]
        public void LinqQuery01()
        {
            List<LinqSamples.Customer> customers = sample.GetCustomerList();
            var result =
                from cus in customers
                where cus.Orders.Sum(o => o.Total) > 50000
                select new { CustomerName = cus.CompanyName, OrdersSum = cus.Orders.Sum(o => o.Total) };
            foreach (var item in result)
            {
                Console.WriteLine($"{item.CustomerName}: {item.OrdersSum:C}");
            }
        }

        [Title("LinqQuery02")]
        [Description("List of suppliers located in the same country and the same city as the client")]
        public void LinqQuery02()
        {
            List<LinqSamples.Customer> customers = sample.GetCustomerList();
            List<LinqSamples.Supplier> suppliers = sample.GetSupplierList();

            var result =
                from customer in customers
                join supplier in suppliers
                on new { customer.Country, customer.City } equals new { supplier.Country, supplier.City } into suppliersGroup
                select new
                {
                    Customer = customer,
                    Suppliers = suppliersGroup
                };

            foreach (var customer in result)
            {
                Console.WriteLine($"{customer.Customer.CompanyName}, {customer.Customer.Country}, {customer.Customer.City}:");
                foreach (var supplier in customer.Suppliers)
                {
                    Console.WriteLine($"\t{supplier.SupplierName}");
                }
                Console.WriteLine();
            }
        }

        [Title("LinqQuery03")]
        [Description("A list of those customers whose orders exceed the set value in total")]
        public void LinqQuery03()
        {
            List<LinqSamples.Customer> customers = sample.GetCustomerList();
            var result =
                from cus in customers
                where cus.Orders.Any(o => o.Total > 5000)
                select new { CustomerName = cus.CompanyName, Order = cus.Orders.First(o => o.Total > 5000).Total };

            foreach (var item in result)
            {
                Console.WriteLine($"{item.CustomerName}: {item.Order:C}");
            }
        }

        [Title("LinqQuery04")]
        [Description("A list of all customers in sorted form by year, month of first order, customer turnover (from maximum to minimum) and customer name")]
        public void LinqQuery04()
        {
            List<LinqSamples.Customer> customers = sample.GetCustomerList();

            var result = customers
                .OrderBy(c => c.Orders.FirstOrDefault()?.OrderDate.Year)
                .ThenBy(c => c.Orders.FirstOrDefault()?.OrderDate.Month)
                .ThenByDescending(c => c.Orders.Sum(o => o.Total))
                .ThenBy(c => c.CompanyName)
                .Select(c => new
                {
                    c.Orders.FirstOrDefault()?.OrderDate.Year,
                    c.Orders.FirstOrDefault()?.OrderDate.Month,
                    TotalOrder = c.Orders.Sum(o => o.Total),
                    c.CompanyName
                });

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Year}, {item.Month}, {item.TotalOrder:C}, {item.CompanyName}.");
            }
        }

        [Title("LinqQuery06")]
        [Description("Groups all products into categories, inside - according to stock status, inside the last group - by value")]
        public void LinqQuery06()
        {
            List<LinqSamples.Product> products = sample.GetProductList();
            var result =
                from product in products
                group product by product.Category into categoryGroup
                from price in
                    (from product in categoryGroup
                     group product by product.UnitsInStock)
                group price by categoryGroup.Key;

            foreach (var outerGroup in result)
            {
                Console.WriteLine($"{outerGroup.Key}");
                foreach (var innerGroup in outerGroup)
                {
                    Console.WriteLine($"\tUnits in stock: {innerGroup.Key}");
                    foreach (var innerGroupElement in innerGroup)
                    {
                        Console.WriteLine($"\t\tPrice: {innerGroupElement.UnitPrice:C}");
                    }
                }
            }
        }
    }
}
