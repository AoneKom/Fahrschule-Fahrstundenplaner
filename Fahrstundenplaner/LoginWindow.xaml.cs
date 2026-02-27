using System;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace Fahrstundenplaner
{
    public partial class LoginWindow : Window
    {
        private const string ConnectionString = "Data Source=Fahrschule.db";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string user = TxtUser.Text;
            string pass = TxtPass.Password;

            using (var db = new SqliteConnection(ConnectionString))
            {
                db.Open();
                var cmd = new SqliteCommand("SELECT Role FROM Users WHERE Username = @u AND Password = @p", db);
                cmd.Parameters.AddWithValue("@u", user);
                cmd.Parameters.AddWithValue("@p", pass);

                var role = cmd.ExecuteScalar()?.ToString();

                if (role != null)
                {
                    
                    LogAction($"Login erfolgreich: {user} ({role})");


                    MainWindow main = new MainWindow();

                    
                    if (main.DataContext is ViewModels.MainViewModel vm)
                    {
                        vm.IsAdmin = (role == "Administrator");
                    }

                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Falscher Benutzername oder Passwort!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    LogAction($"Login-Versuch fehlgeschlagen: {user}");
                }
            }
        }

        private void LogAction(string action)
        {
            try
            {
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    var cmd = new SqliteCommand("INSERT INTO Logs (Timestamp, Action) VALUES (@t, @a)", db);
                    cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@a", action);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}