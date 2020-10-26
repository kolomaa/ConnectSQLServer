using System;
using System.Data;
using System.Data.SqlClient;

namespace SchoolSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=School;Integrated Security=True";

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
            ShowClasses(connection);
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.Enter:
                    Console.Write("Id of class: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Name:");
                    string name = Console.ReadLine();
                    AddPupil(connection, name, id);
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
                ShowTable("Subjects", connection);
                ShowTable("Teachers", connection);
                ShowTable("Classes", connection);
                ShowTable("Rooms", connection);
                ShowTable("Lessons", connection);
                ShowTable("Pupils", connection);
                menuAction = ShowMenuForAdmin();

                switch (menuAction)
                {
                    case 1:
                        Console.Write("Name:");
                        string name = Console.ReadLine();
                        AddSubject(connection, name);
                        break;
                    case 2:
                        Console.Write("Name:");
                        name = Console.ReadLine();
                        Console.Write("SubjectID:");
                        int idsubj = int.Parse(Console.ReadLine());
                        Console.Write("Phone:");
                        string phone = Console.ReadLine();
                        AddTeacher(connection, name, idsubj, phone);
                        break;
                    case 3:
                        Console.Write("Name:");
                        name = Console.ReadLine();
                        Console.Write("ClassID:");
                        int idclass = int.Parse(Console.ReadLine());
                        AddPupil(connection, name, idclass);
                        break;
                    case 4:
                        Console.Write("Id:");
                        int id = int.Parse(Console.ReadLine());
                        RemoveSubject(connection, id);
                        break;
                    case 5:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemoveTeacher(connection, id);
                        break;
                    case 6:
                        Console.Write("Id:");
                        id = int.Parse(Console.ReadLine());
                        RemovePupil(connection, id);
                        break;
                    default:
                        break;
                }
                Console.ReadLine();
                Console.Clear();

            } while (menuAction != 0);
        }



        public static void ShowClasses(SqlConnection connection)
        {
            string sql = String.Format("SELECT * FROM Classes");
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


        public static void AddSubject(SqlConnection connection, string n)
        {
            string query = "INSERT INTO Subjects (Name) VALUES (@Name)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = n;
            command.ExecuteNonQuery();
        }
      
        public static void AddTeacher(SqlConnection connection, string n,  int p, string d)
        {
            string query = "INSERT INTO Teachers (Name, SubjectsID, Phone) VALUES (@Name, @SubjectsID, @Phone)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = n;
            command.Parameters.Add("@SubjectsID", SqlDbType.Int).Value = p;
            command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = d;
            command.ExecuteNonQuery();
        }

        public static void AddPupil(SqlConnection connection, string name, int id)
        {
            string query = "INSERT INTO Pupils (Name, ClassID) VALUES (@Name, @ClassID)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
            command.Parameters.Add("@ClassID", SqlDbType.Int).Value =id;
            command.ExecuteNonQuery();
        }

        public static void RemoveSubject(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Subjects WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static void RemoveTeacher(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Teachers WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static void RemovePupil(SqlConnection connection, int id)
        {
            string query = "DELETE FROM Pupils WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
            command.ExecuteNonQuery();
        }

        public static int ShowMenuForAdmin()
        {
            Console.WriteLine("-----------------");
            Console.WriteLine("1-Add new subject");
            Console.WriteLine("2-Add new teacher");
            Console.WriteLine("3-Add new pupil");
            Console.WriteLine("4-Remove subject");
            Console.WriteLine("5-Remove teacher");
            Console.WriteLine("6-Remove pupil");
            return int.Parse(Console.ReadLine());
        }
    }
}
