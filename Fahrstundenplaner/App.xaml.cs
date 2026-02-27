using Microsoft.Data.Sqlite;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Threading;

namespace Fahrstundenplaner
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("de-DE")));

            base.OnStartup(e);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var db = new SqliteConnection("Data Source=Fahrschule.db"))
            {
                db.Open();
                var cmd = new SqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS Logs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Timestamp TEXT,
                        Username TEXT,
                        Action TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT UNIQUE,
                        Password TEXT,
                        Role TEXT
                    );", db);
                cmd.ExecuteNonQuery();

                var check = new SqliteCommand("SELECT COUNT(*) FROM Users", db);
                if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                {
                    new SqliteCommand("INSERT INTO Users (Username, Password, Role) VALUES ('admin', '1234', 'Administrator')", db).ExecuteNonQuery();
                }
            }
        }
    }
} 