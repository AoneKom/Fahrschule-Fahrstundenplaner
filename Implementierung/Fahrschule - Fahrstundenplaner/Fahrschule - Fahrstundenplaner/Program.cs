using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrschule___Fahrstundenplaner
{
    internal class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== FAHRSCHULE VERWALTUNG ====\n");
                Console.WriteLine("1) Schüler verwalten");
                Console.WriteLine("2) Fahrstunden verwalten");
                Console.WriteLine("3) Buchungen verwalten");
                Console.WriteLine("0) Beenden");
                Console.Write("\nAuswahl: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": StudentMenu(); break;
                    case "2": LessonMenu(); break;
                    case "3": BookingMenu(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Ungültige Eingabe.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void StudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Schüler ====\n");
                Console.WriteLine("1) Alle anzeigen");
                Console.WriteLine("2) Schüler hinzufügen");
                Console.WriteLine("3) Schüler löschen");
                Console.WriteLine("0) Zurück\n");

                Console.Write("Auswahl: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": StudentService.ShowAll(); break;
                    case "2": StudentService.Create(); break;
                    case "3": StudentService.Delete(); break;
                    case "0": return;
                }
            }
        }

        static void LessonMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Fahrstunden ====\n");
                Console.WriteLine("1) Alle anzeigen");
                Console.WriteLine("2) Fahrstunde hinzufügen");
                Console.WriteLine("3) Fahrstunde löschen");
                Console.WriteLine("0) Zurück\n");

                Console.Write("Auswahl: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": LessonService.ShowLessons(); break;
                    case "2": LessonService.CreateLesson(); break;
                    case "3": LessonService.DeleteLesson(); break;
                    case "0": return;
                }
            }
        }

        static void BookingMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== Buchungen ====\n");
                Console.WriteLine("1) Alle Buchungen anzeigen");
                Console.WriteLine("2) Buchung erstellen");
                Console.WriteLine("3) Buchung löschen");
                Console.WriteLine("0) Zurück\n");

                Console.Write("Auswahl: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": BookingService.ShowBookings(); break;
                    case "2": BookingService.CreateBooking(); break;
                    case "3": BookingService.DeleteBooking(); break;
                    case "0": return;
                }
            }
        }
    }
}
