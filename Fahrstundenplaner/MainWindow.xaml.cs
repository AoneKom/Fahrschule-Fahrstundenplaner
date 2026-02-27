using Fahrstundenplaner.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Fahrstundenplaner.Models;

namespace Fahrstundenplaner
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;

 
            AtLehrer.ItemsSource = _viewModel.AvailableLehrer;


            FillGermanDays();

            LogSystemAction("Anwendung gestartet", "System");
        }

        private void FillGermanDays()
        {
            var deCulture = new CultureInfo("de-DE");
            var days = new List<string>();


            foreach (DayOfWeek d in Enum.GetValues(typeof(DayOfWeek)))
            {
           
                days.Add(deCulture.DateTimeFormat.GetDayName(d));
            }

            AtDay.ItemsSource = days;
            AtDay.SelectedIndex = 1; 
        }

     

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddNewRow();
            LogSystemAction("Neuer Eintrag hinzugefügt", "Admin");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            if (MainGrid.SelectedItem is Fahrstundenplaner.ViewModels.Fahrstunde selected)
            {
                _viewModel.Fahrstunden.Remove(selected);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveToDatabase();
            LogSystemAction("Datenbank gespeichert", "Admin");
        }


        private void AutoGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные из полей
            string name = AtName.Text;
            string lehrer = AtLehrer.SelectedItem?.ToString();
            string type = AtType.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(name) || name == "Schülername...") return;

            DateTime startPoint;
            if (!DateTime.TryParse(AtTime.Text, out startPoint)) return;

            int count;
            int.TryParse(AtCount.Text, out count);

            for (int i = 0; i < count; i++)
            {
                string targetTime = startPoint.AddDays(i * 7).ToString("dd.MM.yyyy HH:mm");

               
                if (_viewModel.Fahrstunden.Any(f => f.Startzeit == targetTime && f.LehrerName == lehrer))
                {
                    continue;
                }

                _viewModel.Fahrstunden.Add(new Fahrstunde
                {
                    Startzeit = targetTime,
                    StudentName = name,
                    LehrerName = lehrer,
                    LessonType = type,
                    Preis = MainViewModel.Tarife.ContainsKey(type) ? MainViewModel.Tarife[type] : 45.0
                });
            }
        }



        private void OpenLog_Click(object sender, RoutedEventArgs e)
        {
            LogWindow logWin = new LogWindow();
            logWin.Owner = this;
            logWin.ShowDialog();
        }

        private void OpenUserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow userWin = new UserManagementWindow();
            userWin.Owner = this;
            userWin.ShowDialog();


            _viewModel.LoadLehrerList();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogSystemAction("Abmeldung", "Admin");
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }



        private void LogSystemAction(string action, string user)
        {
            try
            {
                using (var db = new SqliteConnection("Data Source=Fahrschule.db"))
                {
                    db.Open();
                    var cmd = new SqliteCommand("INSERT INTO Logs (Timestamp, Username, Action) VALUES (@t, @u, @a)", db);
                    cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@a", action);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void Placeholder_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && (tb.Text.Contains("...") || tb.Text == "14:00" || tb.Text == "10"))
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
            
                StackPanel container = new StackPanel { Margin = new Thickness(40) };

         
                TextBlock header = new TextBlock
                {
                    Text = "FAHRSCHUL-FAHRPLAN",
                    FontSize = 24,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)),
                    Margin = new Thickness(0, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                container.Children.Add(header);

                TextBlock subHeader = new TextBlock
                {
                    Text = $"Erstellt am: {DateTime.Now:dd.MM.yyyy HH:mm}",
                    FontSize = 12,
                    Margin = new Thickness(0, 0, 0, 30),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.Gray
                };
                container.Children.Add(subHeader);

      
                DataGrid printGrid = new DataGrid
                {
                    ItemsSource = _viewModel.FahrstundenView, 
                    AutoGenerateColumns = false,
                    CanUserAddRows = false,
                    GridLinesVisibility = DataGridGridLinesVisibility.All,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Foreground = Brushes.Black,
                    FontSize = 12,
                    RowHeight = 30,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

               
                printGrid.Columns.Add(new DataGridTextColumn { Header = " Datum/Zeit ", Binding = new Binding("Startzeit"), Width = 140 });
                printGrid.Columns.Add(new DataGridTextColumn { Header = " Schüler ", Binding = new Binding("StudentName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                printGrid.Columns.Add(new DataGridTextColumn { Header = " Lehrer ", Binding = new Binding("LehrerName"), Width = 120 });
                printGrid.Columns.Add(new DataGridTextColumn { Header = " Typ ", Binding = new Binding("LessonType"), Width = 120 });
                printGrid.Columns.Add(new DataGridTextColumn { Header = " € ", Binding = new Binding("Preis") { StringFormat = "{0:N2}" }, Width = 60 });

                container.Children.Add(printGrid);

            
                TextBlock footer = new TextBlock
                {
                    Text = "\nVielen Dank für die Zusammenarbeit!",
                    FontSize = 10,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                container.Children.Add(footer);

               
                container.Width = printDlg.PrintableAreaWidth;
                printGrid.Width = printDlg.PrintableAreaWidth - 80; 
                container.UpdateLayout(); 
           
                printDlg.PrintVisual(container, "Fahrschule Report");
            }
        }
    }
}