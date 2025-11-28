using System;
using System.Data.SqlClient;
using Fahrschule___Fahrstundenplaner;

namespace Fahrschule___Fahrstundenplaner
{
    internal class StudentService
    {
        public static void ShowAll()
        {
            Console.Clear();
            Console.WriteLine("==== Schülerliste ====\n");

            string sql = "SELECT StudentID, FirstName, LastName, Phone, TotalHours FROM Student";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"{reader["StudentID"]}: {reader["FirstName"]} {reader["LastName"]} | " +
                        $"Phone: {reader["Phone"]} | Stunden: {reader["TotalHours"]}");
                }
            }

            Console.WriteLine("\nWeiter mit Taste...");
            Console.ReadKey();
        }

        public static void Create()
        {
            Console.Clear();
            Console.WriteLine("==== Schüler erstellen ====\n");

            Console.Write("Vorname: ");
            string fn = Console.ReadLine();

            Console.Write("Nachname: ");
            string ln = Console.ReadLine();

            Console.Write("Telefon: ");
            string phone = Console.ReadLine();

            string sql = "INSERT INTO Student (FirstName, LastName, Phone) VALUES (@fn, @ln, @phone)";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.Parameters.AddWithValue("@phone", phone);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Schüler erstellt!");
            Console.ReadKey();
        }

        public static void Delete()
        {
            Console.Clear();
            Console.WriteLine("==== Schüler löschen ====\n");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            string sql = "DELETE FROM Student WHERE StudentID=@id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Schüler gelöscht!");
            Console.ReadKey();
        }
    }
}
    