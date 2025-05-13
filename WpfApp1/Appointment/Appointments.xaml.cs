using LoginOptions;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace WpfApp1.Appointment
{
    // MenuItem osztály a gombokhoz tartozó időpontok és láthatóság kezeléséhez
    public class MenuItem
    {
        public string? Time { get; set; }
        public string? Description { get; set; }
        public bool IsButtonVisible { get; set; } = true; // Alapértelmezés szerint látható
        public int AppointmentId { get; set; } // A foglalás ID-ja az adatbázisban
    }

    // Kell hogy menjen a visibility
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Appointments : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();
        public string? DoctorText { get; set; }
        public string ChoosenDoctor;
        public string SelectedDateDay = DateTime.Today.DayOfWeek.ToString();
        public string MonthOfChoice = "";
        public string DayOfChoice = "";
        public string YearOfChoice = "";
        public int IdOfDoctor;
        public int AccessID;
        public int user_ID;

        public Appointments(string doctor, int DoctorID, int userID)
        {
            user_ID = userID;
            InitializeComponent();
            IdOfDoctor = DoctorID;
            ChoosenDoctor = doctor;
            Title = $"Választott orvosa: {doctor}";
            DoctorText = $"{doctor} rendelési időpontjai";
            DataContext = this;
            UpdateMenu(); // Frissítjük az időpontokat
            StartTimer(); // Percenkénti frissítés elindítása
            AppointmentPicker.SelectedDate = DateTime.Today;
            AppointmentPicker.DisplayDateStart = DateTime.Today;
            AppointmentPicker.DisplayDateEnd = DateTime.Today.AddMonths(1); // Mától 1 hónapra lehessen csak előre foglalni
        }

        private void StartTimer()
        {
            DateTime now = DateTime.Now;
            DateTime nextHour = now.AddHours(1).AddMinutes(-now.Minute).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
            TimeSpan initialDelay = nextHour - now;

            DispatcherTimer initialTimer = new DispatcherTimer { Interval = initialDelay };
            initialTimer.Tick += (sender, args) =>
            {
                UpdateMenu();
                initialTimer.Stop();

                DispatcherTimer repeatingTimer = new DispatcherTimer { Interval = TimeSpan.FromHours(1) };
                repeatingTimer.Tick += (s, e) => UpdateMenu();
                repeatingTimer.Start();
            };
            initialTimer.Start();
        }

        private void OnChangedDate(object sender, RoutedEventArgs e)
        {
            if (AppointmentPicker.SelectedDate.HasValue)
            {
                SelectedDateDay = AppointmentPicker.SelectedDate.Value.DayOfWeek.ToString();
                DayOfChoice = AppointmentPicker.SelectedDate.Value.Day.ToString();
                MonthOfChoice = AppointmentPicker.SelectedDate.Value.Month.ToString();
                YearOfChoice = AppointmentPicker.SelectedDate.Value.Year.ToString();
                UpdateMenu();
            }
        }

        private void UpdateMenu()
        {
            int currentHour = DateTime.Now.Hour;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            MenuItems.Clear();

            bool hasAvailableTime = false;

            switch (ChoosenDoctor)
            {
                case "Mesterseges":
                    if (SelectedDateDay == "Monday" || SelectedDateDay == "Tuesday")
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;

                case "Dora":
                    if (SelectedDateDay == "Wednesday")
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;

                case "Musky":
                    if (SelectedDateDay == "Thursday" || SelectedDateDay == "Friday" || SelectedDateDay == "Saturday")
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;
            }

            if (!hasAvailableTime)
            {
                MenuItems.Add(new MenuItem { Description = $"{YearOfChoice}.{MonthOfChoice}.{DayOfChoice}. napon {ChoosenDoctor} nem rendel.", IsButtonVisible = false });
            }
            else
            {
                foreach (var item in MenuItems)
                {
                    item.IsButtonVisible = true;
                }
            }
        }

        private bool AddAvailableTimes()
        {
            string isAvailable = "Elérhető";
            int currentHour = DateTime.Now.Hour;
            bool hasAvailableTime = false;

            if (DateTime.Now.Day.ToString() == DayOfChoice)
            {
                for (int hour = 8; hour <= 16; hour++)
                {
                    if (hour > currentHour + 1)
                    {
                        hasAvailableTime = true;
                        MenuItems.Add(new MenuItem { Time = $"{hour}:00", Description = $"{isAvailable} időpont" });
                    }
                }
            }
            else
            {
                for (int hour = 8; hour <= 16; hour++)
                {
                    hasAvailableTime = true;
                    MenuItems.Add(new MenuItem { Time = $"{hour}:00", Description = $"{isAvailable} időpont" });
                }
            }
            return hasAvailableTime;
        }

        private void BookAppointment_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var time = button?.Tag.ToString();
            MessageBox.Show($"A foglalás sikeresen megtörtént: {time}");

            // Foglalás beszúrása az adatbázisba
            BookAppointmentInDb(time);

            // A foglalt időpont eltávolítása a listából
            var bookedAppointment = MenuItems.FirstOrDefault(x => x.Time == time);
            if (bookedAppointment != null)
            {
                MenuItems.Remove(bookedAppointment);
            }
        }

        private void BookAppointmentInDb(string time)
        {
            // A foglalás adatbázisba történő mentése
            // Itt feltételezzük, hogy van egy `DbContext` vagy `DatabaseHelper` osztály, ami kezeli az adatbázis műveleteket

            var appointment = new global::Appointments
            {
                datetime = DateTime.Parse($"{YearOfChoice}-{MonthOfChoice}-{DayOfChoice} {time}"),
                doctor_id = IdOfDoctor,
                user_id = user_ID  // Ez az aktuális felhasználó ID-ja, amit be kell állítani
            };

            // Feltételezzük, hogy van egy `DbContext` példány
            using (var db = new MyDbContext())
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow LoginOptions = new MainWindow();
            this.Close();
            Application.Current.MainWindow.Close();
            LoginOptions.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }
    }
}
