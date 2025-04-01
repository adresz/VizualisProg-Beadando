using LoginOptions;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
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

        public Appointments(string doctor)
        {
            InitializeComponent();
            ChoosenDoctor = doctor;
            Title = $"Választott orvosa: {doctor}";
            DoctorText = $"{doctor} rendelési időpontjai";
            DataContext = this;
            UpdateMenu(); // Frissítjük az időpontokat
            StartTimer(); // Percenkénti frissités elinditása
            AppointmentPicker.SelectedDate = DateTime.Today;
            AppointmentPicker.DisplayDateStart = DateTime.Today;
            AppointmentPicker.DisplayDateEnd = DateTime.Today.AddMonths(1); // Mától 1 hónapra lehessen csak előre foglalni
        }

        private void StartTimer()
        {
            // Calculate the time until the next full hour
            DateTime now = DateTime.Now;
            DateTime nextHour = now.AddHours(1).AddMinutes(-now.Minute).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
            TimeSpan initialDelay = nextHour - now;

            // Create a one-time timer to sync exactly at the start of the next hour
            DispatcherTimer initialTimer = new DispatcherTimer { Interval = initialDelay };
            initialTimer.Tick += (sender, args) =>
            {
                UpdateMenu(); // Update immediately at the next hour
                initialTimer.Stop(); // Stop the initial one-time timer

                // Start a repeating timer that fires every hour
                DispatcherTimer repeatingTimer = new DispatcherTimer { Interval = TimeSpan.FromHours(1) };
                repeatingTimer.Tick += (s, e) => UpdateMenu();
                repeatingTimer.Start();
            };
            initialTimer.Start();
        }

        private void OnChangedDate(object sender, RoutedEventArgs e)
        {
            //Warning, hogy ne legyen NULLable
            //Év hónap nap kiszedése kiváalsztáskor, hogy könnyebb kezelés, elérhetőség legyen
            if(AppointmentPicker.SelectedDate.HasValue)
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

            // Orvosokra elérhető nap szűrés
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

            // Ha nincs elérhető időpont, írja ki, hogy "A mai napon nem rendel"
            if (!hasAvailableTime)
            {
                MenuItems.Add(new MenuItem {Description = $"{YearOfChoice}.{MonthOfChoice}.{DayOfChoice}. napon {ChoosenDoctor} nem rendel.", IsButtonVisible = false });

                // Ha nincs elérhető időpont, az összes gombot elrejtjük
                foreach (var item in MenuItems)
                {
                    item.IsButtonVisible = false; // Elrejti az összes gombot
                }
            }
            else
            {
                // Ha van elérhető időpont, biztosítjuk, hogy a gomb látható legyen
                foreach (var item in MenuItems)
                {
                    item.IsButtonVisible = true; // Láthatóvá tesszük a gombokat
                }
            }
        }

        private bool AddAvailableTimes()
        {
            string isAvailable = "Elérhető";
            int currentHour = DateTime.Now.Hour;
            bool hasAvailableTime = false;

                //ha mai nap
                if(DateTime.Now.Day.ToString() == DayOfChoice)
                {
                    for (int hour = 8; hour <= 16; hour++)
                    {
                        // Ne lehessen foglalni a közvetlen a következő órára, csak 2 vel későbbire
                        if (hour > currentHour + 1)
                        {
                            hasAvailableTime = true; // Van elérhető időpont
                                                     // Csak azt az órát adjuk hozzá, ami a jelenlegi után van legalább 1+ órával
                            MenuItems.Add(new MenuItem { Time = $"{hour}:00", Description = $"{isAvailable} időpont" });
                        }
                    }
                }
                //ha nem mai nap
                else
                {
                    for (int hour = 8; hour <= 16; hour++)
                    {
                            hasAvailableTime = true; // Van elérhető időpont
                                                     // Az összes órát hozzáadjuk, ami az adott naphoz tartozik
                            MenuItems.Add(new MenuItem { Time = $"{hour}:00", Description = $"{isAvailable} időpont" });
                    }
                }
                return hasAvailableTime;
        }

        // Foglalás gomb eseménykezelője
        private void BookAppointment_Click(object sender, RoutedEventArgs e)
        {
            // A gomb Tag-jából lekérjük az időpontot
            var button = sender as Button;
            var time = button?.Tag.ToString();
            MessageBox.Show($"A foglalás sikeresen megtörtént: {time}");
        }

        //Kijelentkezés/bezárással kapcsolatos dolgok

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
