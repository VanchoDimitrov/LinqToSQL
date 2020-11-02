using AppliedExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinqToSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            //NewCustomer();           
            //UpdateCustomer();
            //DeleteCustomer();

            //Console.ReadKey();

            //RegularQuery();


            ///reporting
            ///
            //Report1();
            Report2();

            Console.ReadKey();
        }

        #region CRUD
        static void RegularQuery()
        {
            // 1. Datasource - dbContext
            var db = new NorthwindEntities();

            // 2. Linq query
            var query1 = from p in db.Customers
                         where p.CompanyName.Contains("Linq Company")
                         orderby p.CustomerID
                         select new
                         {
                             p.CustomerID,
                             p.CompanyName
                         };

            // 2.1 Method query
            var query2 = db.Products.Select(p => new
            {
                p.ProductID,
                p.ProductName
            }).OrderBy(x => x.ProductID).ToList();


            if (query1.Any())
            {
                // 3. Execute
                foreach (var i in query1)
                {
                    Console.WriteLine(i.CustomerID + " " + i.CompanyName);
                }
            }
            else
            {
                Console.WriteLine("No records by that condition found!");
            }

        }

        static void NewCustomer()
        {
            // 1. datasource
            var db = new NorthwindEntities();


            // 2. Insert code
            var c = new Customer();
            c.CustomerID = "LC";
            c.CompanyName = "Linq Company";


            // 3. Execute
            db.Customers.Add(c);
            db.SaveChanges();

            Console.WriteLine("Record saved!");
            Console.WriteLine("Please press any key to list the new company!");

        }

        static void UpdateCustomer()
        {
            // 1. datasource
            var db = new NorthwindEntities();

            // 2. Linq query
            //var query1 = (from c in db.Customers
            //              where c.CustomerID == "LC"
            //              select c).SingleOrDefault();

            //query1.CompanyName = "Linq Company, Ltd.";
            //db.SaveChanges();

            // 2.1 Method syntax
            var query2 = db.Customers.Where(c => c.CustomerID == "LC").SingleOrDefault();
            query2.CompanyName = "Linq Company, Ltd.";
            db.SaveChanges();

            Console.WriteLine("Record updated!");
            Console.WriteLine("Press any key to list the updated record please!");
        }

        static void DeleteCustomer()
        {
            // 1. datasource
            var db = new NorthwindEntities();

            // 2. Linq query
            var query1 = (from c in db.Customers
                          where c.CustomerID == "LC"
                          select c).SingleOrDefault();


            // 3. Execute the Linq query
            db.Customers.Remove(query1);
            db.SaveChanges();

            Console.WriteLine("Record being deleted!");
        }
        #endregion

        /// <summary>
        ///    Employee sales by Category
        /// </summary>
        static void Report1()
        {
            var db = new NorthwindEntities();

            var query1 = ((from b in db.Products
                           join c in db.Categories on b.CategoryID equals c.CategoryID
                           where b.Discontinued == true
                           orderby b.ProductName
                           select new
                           {
                               ProductID = b.ProductID,
                               ProductName = b.ProductName,
                               SupplierID = b.SupplierID,
                               CategoryID = b.CategoryID,
                               Category = c.CategoryName,
                               QuantityPerUnit = b.QuantityPerUnit,
                               UnitPrice = b.UnitPrice,
                               UnitsInStock = b.UnitsInStock,
                               UnitsOnOrder = b.UnitsOnOrder,
                               ReorderLevel = b.ReorderLevel,
                               Discontinued = b.Discontinued,
                               b.Category.CategoryName
                           }).Distinct()).ToList();

            var query2 = (db.Products.Where(b => b.Discontinued == true).OrderBy(o => o.ProductName).Select(x => new
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                SupplierID = x.SupplierID,
                CategoryID = x.CategoryID,
                //Category = c.CategoryName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                UnitsOnOrder = x.UnitsOnOrder,
                ReorderLevel = x.ReorderLevel,
                Discontinued = x.Discontinued,
                Category = x.Category.CategoryName
            }).Distinct()).ToList();

            foreach (var i in query2)
            {
                Console.WriteLine($"{i.ProductID} {i.ProductName}      {i.CategoryID} {i.Category}");
            }
        }

        /// <summary>
        ///    Order Details which includes the Extended price
        /// </summary>
        static void Report2()
        {
            var db = new NorthwindEntities();

            var query1 = (from y in db.Order_Details
                          orderby y.OrderID
                          select new
                          {
                              y.OrderID,
                              y.ProductID,
                              y.Product.ProductName,
                              y.UnitPrice,
                              y.Quantity,
                              y.Discount,
                              ExtendedPrice = (float?)(y.UnitPrice * y.Quantity) * (float?)(1 - y.Discount)
                          }).Distinct().ToList();

            var query2 = db.Order_Details.Select(y => new
            {
                y.OrderID,
                y.ProductID,
                y.Product.ProductName,
                y.UnitPrice,
                y.Quantity,
                y.Discount,
                ExtendedPrice = (float?)(y.UnitPrice * y.Quantity) * (float?)(1 - y.Discount)
            }).OrderBy(o => o.OrderID).Distinct().ToList();

            foreach (var i in query2)
            {
                Console.WriteLine($"{i.OrderID} {i.ProductName} {i.ExtendedPrice}");
            }
        }
    }
}
