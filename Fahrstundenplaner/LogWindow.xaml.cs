using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace Fahrstundenplaner
{
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            LoadLogs();
        }

        private void LoadLogs()
        {
            var logs = new List<LogEntry>();
            try
            {
                using (var db = new SqliteConnection("Data Source=Fahrschule.db"))
                {
                    db.Open();
                    var cmd = new SqliteCommand("SELECT Timestamp, Username, Action FROM Logs ORDER BY Id DESC", db);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(new LogEntry
                            {
                                Timestamp = reader.GetString(0),
                                Username = reader.IsDBNull(1) ? "System" : reader.GetString(1),
                                Action = reader.GetString(2)
                            });
                        }
                    }
                }

                LogGrid.ItemsSource = logs;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();
    }

    public class LogEntry
    {
        public string Timestamp { get; set; }
        public string Username { get; set; }
        public string Action { get; set; }
    }
}