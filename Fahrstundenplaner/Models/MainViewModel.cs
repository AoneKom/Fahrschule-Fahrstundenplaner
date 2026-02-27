using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Fahrstundenplaner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string ConnectionString = "Data Source=Fahrschule.db";

        public static readonly Dictionary<string, double> Tarife = new Dictionary<string, double>
        {
            { "Übungsfahrt", 45.0 },
            { "Überlandfahrt", 55.0 },
            { "Autobahn", 60.0 },
            { "Nachtfahrt", 65.0 },
            { "Prüfung", 120.0 }
        };

        private ObservableCollection<Fahrstunde> _fahrstunden = new ObservableCollection<Fahrstunde>();
        public ObservableCollection<Fahrstunde> Fahrstunden
        {
            get => _fahrstunden;
            set { _fahrstunden = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AvailableLehrer { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableTypes { get; set; }

        private bool _isAdmin = true;
        public bool IsAdmin
        {
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                FahrstundenView.Refresh();
            }
        }

        private bool _showOnlyUnpaid;
        public bool ShowOnlyUnpaid
        {
            get => _showOnlyUnpaid;
            set
            {
                _showOnlyUnpaid = value;
                OnPropertyChanged();
                FahrstundenView.Refresh();
            }
        }

        public ICollectionView FahrstundenView { get; set; }


        public MainViewModel()
        {
            AvailableTypes = new ObservableCollection<string>(Tarife.Keys);
            LoadLehrerList();
            LoadFromDatabase();

            FahrstundenView = CollectionViewSource.GetDefaultView(Fahrstunden);
            FahrstundenView.Filter = FilterLogic;


            Fahrstunden.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (Fahrstunde item in e.NewItems)
                        item.PropertyChanged += Item_PropertyChanged;
                }
                SaveToDatabase();
            };
        }

  
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveToDatabase();
        }

        private bool FilterLogic(object item)
        {
            var stunde = item as Fahrstunde;
            if (stunde == null) return true;

            if (ShowOnlyUnpaid && stunde.Bezahlt) return false;

            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                string search = FilterText.ToLower();
                bool matches = stunde.StudentName.ToLower().Contains(search) ||
                               stunde.LehrerName.ToLower().Contains(search);
                if (!matches) return false;
            }
            return true;
        }

        public void SaveToDatabase()
        {
            try
            {
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        var delCmd = new SqliteCommand("DELETE FROM Fahrstunden", db, transaction);
                        delCmd.ExecuteNonQuery();

                        foreach (var stunde in Fahrstunden)
                        {
                            var insCmd = new SqliteCommand(@"
                                INSERT INTO Fahrstunden (StudentName, LehrerName, LessonType, Startzeit, Preis, Bezahlt) 
                                VALUES (@s, @l, @t, @sz, @p, @b)", db, transaction);

                            insCmd.Parameters.AddWithValue("@s", stunde.StudentName ?? "");
                            insCmd.Parameters.AddWithValue("@l", stunde.LehrerName ?? "");
                            insCmd.Parameters.AddWithValue("@t", stunde.LessonType ?? "");
                            insCmd.Parameters.AddWithValue("@sz", stunde.Startzeit ?? "");
                            insCmd.Parameters.AddWithValue("@p", stunde.Preis);
                            insCmd.Parameters.AddWithValue("@b", stunde.Bezahlt ? 1 : 0);
                            insCmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Ошибка автосохранения: {ex.Message}");
            }
        }

        public void LoadFromDatabase()
        {
          
            using (var db = new SqliteConnection(ConnectionString))
            {
                db.Open();
                var createTableCmd = new SqliteCommand(@"
            CREATE TABLE IF NOT EXISTS Fahrstunden (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StudentName TEXT,
                LehrerName TEXT,
                LessonType TEXT,
                Startzeit TEXT,
                Preis REAL,
                Bezahlt INTEGER
            )", db);
                createTableCmd.ExecuteNonQuery();
            }
  

            Fahrstunden.Clear();
         
        }

        public void AddNewRow()
        {
            string defaultTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            string defaultLehrer = AvailableLehrer.Count > 0 ? AvailableLehrer[0] : "Kominch";

            if (CheckForConflict(defaultTime, defaultLehrer)) return;

            // 1. Создаем объект
            var newStunde = new Fahrstunde
            {
                Startzeit = defaultTime,
                LehrerName = defaultLehrer,
                StudentName = "NEUER SCHÜLER",
                LessonType = "Übungsfahrt",
                Preis = 45.0
            };
            newStunde.PropertyChanged += Item_PropertyChanged;


    
  

        Fahrstunden.Add(newStunde);
        }

        public void GenerateAutoPlan(string student, string lehrer, string type, DayOfWeek tag, int count, string time)
        {
            DateTime current = DateTime.Now;
            while (current.DayOfWeek != tag) current = current.AddDays(1);

            for (int i = 0; i < count; i++)
            {
                string targetTime = current.AddDays(i * 7).ToString("dd.MM.yyyy") + " " + time;

                if (CheckForConflict(targetTime, lehrer)) continue;

                Fahrstunden.Add(new Fahrstunde
                {
                    StudentName = student,
                    LehrerName = lehrer,
                    LessonType = type,
                    Startzeit = targetTime,
                    Preis = Tarife.ContainsKey(type) ? Tarife[type] : 45.0
                });
            }
        }

        public void LoadLehrerList()
        {
            AvailableLehrer.Clear();
            try
            {
                using (var db = new SqliteConnection(ConnectionString))
                {
                    db.Open();
                    var cmd = new SqliteCommand("SELECT Username FROM Users WHERE LOWER(Username) != 'admin'", db);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AvailableLehrer.Add(reader.GetString(0));
                        }
                    }
                }
                if (AvailableLehrer.Count == 0) AvailableLehrer.Add("Kominch");
            }
            catch { AvailableLehrer.Add("Kominch"); }
        }

        public bool CheckForConflict(string newTime, string lehrer)
        {
            var conflict = Fahrstunden.FirstOrDefault(f => f.Startzeit == newTime && f.LehrerName == lehrer);
            if (conflict != null)
            {
                MessageBox.Show($"ACHTUNG! {lehrer} ist am {newTime} bereits belegt!",
                                "Terminkonflikt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}