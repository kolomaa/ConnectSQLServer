using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConnectSQLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=InternetShop;Integrated Security=True";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
           
            // Открываем подключение
            connection.Open();
            //Console.WriteLine("Подключение открыто");
            //Console.WriteLine("Свойства подключения:");
            //Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
            //Console.WriteLine("\tБаза данных: {0}", connection.Database);
            //Console.WriteLine("\tСервер: {0}", connection.DataSource);
            //Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
            //Console.WriteLine("\tСостояние: {0}", connection.State);
            //Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
           

            Console.WriteLine("1-user");
            Console.WriteLine("2-admin");
            Console.WriteLine("0-close");
            int person = int.Parse(Console.ReadLine());

            Console.Clear();

            do
            {
                if (person == 1)
                {
                    MenuForUser(connection);
                }
                else
                {
                    MenuForAdmin(connection);
                }
                Console.Clear();
                Console.WriteLine("1-user");
                Console.WriteLine("2-admin");
                Console.WriteLine("0-close");
                person = int.Parse(Console.ReadLine());
                Console.Clear();
            } while (person != 0);

            connection.Close();
            Console.Read();
        }

        private static void MenuForUser(SqlConnection connection)
        {
            ShowProducts(connection);
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.Enter:
                    Console.Write("Id of product: ");
                    int id = int.Parse(Console.ReadLine());
                    ShowInfoAboutProduct(connection, id);
                    key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            Console.Write("Name:");
                            string name = Console.ReadLine();
                            Console.Write("Adress:");
                            string adr = Console.ReadLine();
                            Console.Write("Phone:");
                            string phone = Console.ReadLine();
                            AddCustomer(connection, name, adr, phone);
                            break;
                        case ConsoleKey.Escape:
                            return;
                    }
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }

        private static void MenuForAdmin(SqlConnection connection)
        {
            int menuAction = 0;
            do
            {
                ShowTable("Products", connection);
                ShowTable("ProductDatails", connection);
                ShowTable("Orders", connection);
                ShowTable("Employees", connection);
                ShowTable("Customers", connection);
                ShowTable("OrderDatails", connection);
                menuAction = ShowMenuForAdmin();

                switch (menuAction)
                {
                    case 1:
                        Console.Write("Name:");
                        string name = Console.ReadLine();
                        Console.Write("Color:");
                        string color = Console.ReadLine();
                        Console.Write("description:");
                        string description = Console.ReadLine();
                        Console.Write("id:");
                        int id = int.Parse(Console.ReadLine());
                        AddProduct(connection, name);
                        AddProductDatails(connection, id, color, description);
                        break;
                    case 2:
                        Console.Write("Name:");
                        name = Console.ReadLine();
                        Console.Write("Post:");
                        string post = Console.ReadLine();
                        Console.Write("Salary:");
                        int salary = int.Parse(Console.ReadLine());
                        AddEmployee(connection, name, post, salary);
                        break;
                    case 3:
                        Console.Write("Name:");
                        name = Console.ReadLine();
                        Console.Write("Adress:");
                        string adr = Console.ReadLine();
                        Console.Write("Phone:");
                        string phone = Console.ReadLine();
                        AddCustomer(connection, name, adr, phone);
                        break;
                    case 4:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemoveProduct(connection, id);
                        break;
                    case 5:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemoveProductDatails(connection, id);
                        break;
                    case 6:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemoveEmployee(connection, id);
                        break;
                    case 7:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemoveCustomer(connection, id);
                        break;
                    default:
                        break;
                }
                Console.ReadLine();
                Console.Clear();

            } while (menuAction != 0);
        }

       

        public static void ShowProducts(SqlConnection connection)
        {
            string sql = String.Format("SELECT * FROM Products");
            SqlCommand command = new SqlCommand(sql, connection); // объект для выполнения SQL-запроса
            SqlDataReader reader = command.ExecuteReader(); // объект для чтения ответа сервера

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write("{0,13} ", reader.GetName(i));
            }
            Console.WriteLine();
            while (reader.Read()) // читаем результат
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0,13} ", reader[i].ToString());
                }
                Console.WriteLine();
            }
            reader.Close(); // закрываем reader
        }
        public static void ShowInfoAboutProduct(SqlConnection connection, int id)
        {
            string query = String.Format("SELECT * FROM ProductDatails WHERE ID = @ID;");
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            SqlDataReader reader = command.ExecuteReader();


            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write("{0,8} ", reader.GetName(i));
            }
            Console.WriteLine();
            while (reader.Read()) // читаем результат
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0,10} ", reader[i].ToString());
                }
                Console.WriteLine();
            }
            reader.Close(); // закрываем reader
        }

        public static void ShowTable(string tableName, SqlConnection connection)
        {
            string sql = String.Format("SELECT * FROM {0}", tableName); // запрос
            SqlCommand command = new SqlCommand(sql, connection); // объект для выполнения SQL-запроса
            SqlDataReader reader = command.ExecuteReader(); // объект для чтения ответа сервера

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write("{0,8} ", reader.GetName(i));
            }
            Console.WriteLine();
            while (reader.Read()) // читаем результат
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0,10} ", reader[i].ToString());
                }
                Console.WriteLine();
            }
            reader.Close(); // закрываем reader
            Console.WriteLine();
        }
        

        public static void AddProduct(SqlConnection connection, string n)
        {
            string query = "INSERT INTO Products (Name) VALUES (@Name)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = n;
            command.ExecuteNonQuery();
        }

        private static void AddProductDatails(SqlConnection connection, int id, string color, string description)
        {
            string query = "INSERT INTO ProductDatails (ID, Color, Description) VALUES (@ID, @Color, @Description)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.Parameters.Add("@Color", SqlDbType.VarChar).Value = color;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
            command.ExecuteNonQuery();
        }

        public static void AddEmployee(SqlConnection connection, string n, string d, int p)
        {
            string query = "INSERT INTO Employees (Name, Post, Salary) VALUES (@Name,  @Post, @Salary)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = n;
            command.Parameters.Add("@Post", SqlDbType.VarChar).Value = d;
            command.Parameters.Add("@Salary", SqlDbType.Int).Value = p;
            command.ExecuteNonQuery();
        }

        public static void AddCustomer(SqlConnection connection, string n, string s, string t)
        {
            string query = "INSERT INTO customers (Name, Address, Phone, DateInSystem) VALUES (@Name, @Address, @Phone, @DateInSystem)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = n;
            command.Parameters.Add("@Address", SqlDbType.VarChar).Value = s;
            command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = t;
            command.Parameters.Add("@DateInSystem", SqlDbType.Date).Value = DateTime.Now;
            command.ExecuteNonQuery();
        }

        public static void RemoveProduct(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Products WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static void RemoveProductDatails(SqlConnection connection, int id)
        {
            string query = "DELETE FROM ProductDatails WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static void RemoveEmployee(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Employees WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static void RemoveCustomer(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Customers WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static int ShowMenuForAdmin()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("1-Add new product");
            Console.WriteLine("2-Add new employeer");
            Console.WriteLine("3-Add new customer");
            Console.WriteLine("4-Remove product");
            Console.WriteLine("5-Remove product datails");
            Console.WriteLine("6-Remove employeer");
            Console.WriteLine("7-Remove customer");
            return int.Parse(Console.ReadLine());
        }
    }

}
