using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace Fahrstundenplaner
{
    public partial class UserManagementWindow : Window
    {
        private const string ConnectionString = "Data Source=Fahrschule.db";

        public UserManagementWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = new List<User>();
            try
            {
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    var cmd = new SqliteCommand("SELECT Username, Role FROM Users", db);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Username = reader.GetString(0),
                                Role = reader.GetString(1)
                            });
                        }
                    }
                }
                UsersGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden: {ex.Message}");
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string user = TxtUsername.Text.Trim();
            string pass = TxtPassword.Password.Trim();
            string role = (ComboRole.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Bitte Name und Passwort eingeben!");
                return;
            }

            try
            {
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    var cmd = new SqliteCommand("INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)", db);
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", pass);
                    cmd.Parameters.AddWithValue("@r", role);
                    cmd.ExecuteNonQuery();
                }

 
                TxtUsername.Clear();
                TxtPassword.Clear();
                LoadUsers();


                LogSystemAction($"Neuer Nutzer hinzugefügt: {user} ({role})");
            }
            catch (SqliteException)
            {
                MessageBox.Show("Dieser Benutzername existiert bereits!");
            }
        }


        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.DataContext is User selectedUser)
            {
                if (selectedUser.Username == "admin")
                {
                    MessageBox.Show("Der Haupt-Admin kann nicht gelöscht werden!");
                    return;
                }

                var result = MessageBox.Show($"Nutzer {selectedUser.Username} wirklich löschen?", "Bestätigung", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new SqliteConnection(ConnectionString))
                    {
                        db.Open();
                        var cmd = new SqliteCommand("DELETE FROM Users WHERE Username = @u", db);
                        cmd.Parameters.AddWithValue("@u", selectedUser.Username);
                        cmd.ExecuteNonQuery();
                    }
                    LoadUsers();
                    LogSystemAction($"Nutzer gelöscht: {selectedUser.Username}");
                }
            }
        }

        private void LogSystemAction(string action)
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
    }

    // Простой класс для отображения в таблице
    public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}