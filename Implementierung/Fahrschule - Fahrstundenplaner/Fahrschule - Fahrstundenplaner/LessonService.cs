using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fahrschule___Fahrstundenplaner;

namespace Fahrschule___Fahrstundenplaner
{
    internal class LessonService
    {
        public static void ShowLessons()
        {
            Console.Clear();
            Console.WriteLine("==== Fahrstunden ====\n");

            string sql =
                "SELECT LessonID, InstructorID, LessonDate, DurationMinutes, Status FROM Lesson";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"{reader["LessonID"]}: Instr={reader["InstructorID"]}, " +
                        $"{Convert.ToDateTime(reader["LessonDate"]):yyyy-MM-dd HH:mm}, " +
                        $"{reader["DurationMinutes"]} min, {reader["Status"]}");
                }
            }

            Console.WriteLine("\nWeiter mit Taste...");
            Console.ReadKey();
        }

        public static void CreateLesson()
        {
            Console.Clear();
            Console.WriteLine("==== Fahrstunde erstellen ====\n");

            Console.Write("InstructorID: ");
            int instr = int.Parse(Console.ReadLine());

            Console.Write("Datum (YYYY-MM-DD HH:MM): ");
            DateTime dt = DateTime.Parse(Console.ReadLine());

            Console.Write("Dauer (Minuten): ");
            int dur = int.Parse(Console.ReadLine());

            Console.Write("Status: ");
            string status = Console.ReadLine();

            string sql =
                "INSERT INTO Lesson (InstructorID, LessonDate, DurationMinutes, Status) " +
                "VALUES (@i, @d, @m, @s)";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@i", instr);
                cmd.Parameters.AddWithValue("@d", dt);
                cmd.Parameters.AddWithValue("@m", dur);
                cmd.Parameters.AddWithValue("@s", status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Stunde erstellt!");
            Console.ReadKey();
        }

        public static void DeleteLesson()
        {
            Console.Clear();
            Console.WriteLine("==== Fahrstunde löschen ====\n");

            Console.Write("LessonID: ");
            int id = int.Parse(Console.ReadLine());

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand("DELETE FROM Lesson WHERE LessonID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Stunde gelöscht!");
            Console.ReadKey();
        }
    }
}
