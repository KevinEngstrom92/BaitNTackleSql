using BaitNTackleSQL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading;

namespace BaitNTackleSQL
{
    class Program
    {

        
        static bool shouldRun = true;
        static SqlConnection globalConnection = ConnectToDb();
        static void Main(string[] args)
        {
            
            while (shouldRun)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the BaitNTackle Console Application\n\n");
                Console.WriteLine("Menu");
                StringBuilder sbLine = new StringBuilder();
                sbLine.Append('-', Console.WindowWidth);
                Console.WriteLine(sbLine);
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Edit Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. List Product");
                Console.WriteLine("5. Exit");

                ConsoleKeyInfo inputMainMenu = Console.ReadKey(true);
                switch (inputMainMenu.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();

                        Console.Write("Name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Price: ");
                        int price = int.Parse(Console.ReadLine());
                        Product newProd = new Product(name, price);
                        AddProduct(newProd);

                        break;
                    case ConsoleKey.D2:

                        EditProduct();
                        break;
                    case ConsoleKey.D3:
                        DeleteProduct();
                        break;
                    case ConsoleKey.D4:
                        List<Product> prodList = ListProducts();
                        Console.Clear();
                        Console.WriteLine("Name: \t\tPrice:");
                        Console.Write(sbLine);

                        foreach(var prod in prodList)
                        {
                            Console.WriteLine($"{prod.id}. {prod.Name}\t\t{prod.Price}");
                        }
                        Console.ReadKey(true);
                        break;
                    case ConsoleKey.D5:
                        globalConnection.Close();
                        shouldRun = false;
                        break;

                }

            }
        }
        static void AddProduct(Product prod)
        {
            string sql = $"INSERT INTO Product(Title, Price) VALUES(@NAME, @PRICE)";
            SqlCommand sqlCmd = new SqlCommand(sql, globalConnection);
            
            sqlCmd.Parameters.AddWithValue("NAME", prod.Name);
            sqlCmd.Parameters.AddWithValue("PRICE", prod.Price);
            sqlCmd.ExecuteNonQuery();

            Console.Clear();
            Console.WriteLine("Product Added");
            Thread.Sleep(2000);
            return;
        }
        static void DeleteProduct()
        {
            string sql = $"DELETE FROM Product WHERE Id = @ID";
            SqlCommand sqlCmd = new SqlCommand(sql, globalConnection);
            Console.Clear();
            Console.WriteLine("Id of product to delete:");
            int id = int.Parse(Console.ReadLine());
   

         
            sqlCmd.Parameters.AddWithValue("ID", id);
            sqlCmd.ExecuteNonQuery();

            Console.Clear();
            Console.WriteLine("Product Deleted");
            Thread.Sleep(2000);
        }
        static void EditProduct()
        {
            string sql = $"UPDATE Product SET Title=@NAME, Price=@PRICE WHERE Id=@ID";
            SqlCommand sqlCmd = new SqlCommand(sql, globalConnection);
            Console.Clear();
            Console.WriteLine("Id:");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("New name of product:");
            string name = Console.ReadLine();
            Console.WriteLine("New price of product:");
            int price = int.Parse(Console.ReadLine());

            sqlCmd.Parameters.AddWithValue("NAME", name);
            sqlCmd.Parameters.AddWithValue("PRICE", price);
            sqlCmd.Parameters.AddWithValue("ID", id);
            sqlCmd.ExecuteNonQuery();

            Console.Clear();
            Console.WriteLine("Product Updated");
            Thread.Sleep(2000);
            return;
        }
        static List<Product> ListProducts()
        {
            List<Product> prodList = new List<Product>();
            SqlCommand sql = new SqlCommand("SELECT * FROM Product", globalConnection);
            SqlDataReader dataReader = sql.ExecuteReader();

            
                foreach(var data in dataReader)
                {
                    int id = int.Parse(dataReader["Id"].ToString());
                    string name = dataReader["Title"].ToString();
                    int price = int.Parse(dataReader["Price"].ToString());
                    
                    Product prod = new Product(name, price);
                    prod.id = id;
                    prodList.Add(prod);
                }
            
            dataReader.Close();
            return prodList;
        }
        static SqlConnection ConnectToDb()
        {
            string connectionString = "Server=.;Initial Catalog=BaitNTackle;Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
