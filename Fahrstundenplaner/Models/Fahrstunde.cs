using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Fahrstundenplaner.ViewModels
{
    public class Fahrstunde : INotifyPropertyChanged
    {
        private string _studentName;
        private string _lehrerName;
        private string _lessonType;
        private string _startzeit;
        private double _preis;
        private bool _bezahlt;

        public string StudentName { get => _studentName; set { _studentName = value; OnPropertyChanged(); } }
        public string LehrerName
        {
            get => _lehrerName;
            set
            {
                _lehrerName = value;
                OnPropertyChanged();
            }
        }
        public string Startzeit { get => _startzeit; set { _startzeit = value; OnPropertyChanged(); } }
        public bool Bezahlt
        {
            get => _bezahlt;
            set
            {
                _bezahlt = value;
                OnPropertyChanged();

            }
        }

        public string LessonType
        {
            get => _lessonType;
            set
            {
                _lessonType = value;

                if (!string.IsNullOrEmpty(value) && MainViewModel.Tarife.ContainsKey(value))
                {
                    Preis = MainViewModel.Tarife[value];
                }
                OnPropertyChanged();
            }
        }

        public double Preis
        {
            get => _preis;
            set { _preis = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
