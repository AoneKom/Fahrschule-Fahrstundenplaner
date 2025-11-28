using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fahrschule___Fahrstundenplaner;

namespace Fahrschule___Fahrstundenplaner
{
    internal class BookingService
    {
        public static void ShowBookings()
        {
            Console.Clear();
            Console.WriteLine("==== Buchungen ====\n");

            string sql = @"
                SELECT 
                    b.BookingID,
                    s.FirstName + ' ' + s.LastName AS StudentName,
                    i.FirstName + ' ' + i.LastName AS InstructorName,
                    l.LessonDate,
                    l.DurationMinutes,
                    l.Status
                FROM Booking b
                JOIN Student s ON b.StudentID = s.StudentID
                JOIN Lesson  l ON b.LessonID = l.LessonID
                JOIN Instructor i ON l.InstructorID = i.InstructorID
                ORDER BY l.LessonDate;
            ";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    Console.WriteLine(
                        $"{r["BookingID"]}: {r["StudentName"]} | " +
                        $"{r["InstructorName"]} | " +
                        $"{Convert.ToDateTime(r["LessonDate"]):yyyy-MM-dd HH:mm} | " +
                        $"{r["DurationMinutes"]} Min | {r["Status"]}"
                    );
                }
            }

            Console.WriteLine("\nWeiter mit Taste...");
            Console.ReadKey();
        }

        public static void CreateBooking()
        {
            Console.Clear();
            Console.WriteLine("==== Buchung erstellen ====\n");

            Console.Write("StudentID: ");
            int sid = int.Parse(Console.ReadLine());

            Console.Write("LessonID: ");
            int lid = int.Parse(Console.ReadLine());

            string sql = "INSERT INTO Booking (StudentID, LessonID) VALUES (@s, @l)";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@s", sid);
                cmd.Parameters.AddWithValue("@l", lid);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Buchung erstellt!");
            Console.ReadKey();
        }

        public static void DeleteBooking()
        {
            Console.Clear();
            Console.WriteLine("==== Buchung löschen ====\n");

            Console.Write("BookingID: ");
            int id = int.Parse(Console.ReadLine());

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand("DELETE FROM Booking WHERE BookingID=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Buchung gelöscht!");
            Console.ReadKey();
        }
    }
}

