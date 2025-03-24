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

        public Appointments(string doctor)
        {
            InitializeComponent();
            ChoosenDoctor = doctor;
            Title = $"Választott orvosa: {doctor}";
            DoctorText = $"{doctor} rendelési időpontjai";
            DataContext = this;
            UpdateMenu(); // Frissítjük az időpontokat
            StartTimer(); // Percenkénti frissités elinditása
        }

        private void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1) // Percenkénti frissítés
            };
            timer.Tick += (sender, args) => UpdateMenu(); // Percenként frissítjük az időpontokat
            timer.Start();
        }

        private void UpdateMenu()
        {
            string isAvailable = "Elérhető";
            int currentHour = DateTime.Now.Hour;
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            MenuItems.Clear();

            bool hasAvailableTime = false;

            // Orvosokra elérhető nap szűrés
            switch (ChoosenDoctor)
            {
                case "Mesterseges":
                    if (currentDay == DayOfWeek.Monday || currentDay == DayOfWeek.Tuesday)
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;

                case "Orsolya":
                    if (currentDay == DayOfWeek.Wednesday)
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;

                case "Musky":
                    if (currentDay == DayOfWeek.Thursday || currentDay == DayOfWeek.Friday || currentDay == DayOfWeek.Saturday)
                    {
                        hasAvailableTime = AddAvailableTimes();
                    }
                    break;
            }

            // Ha nincs elérhető időpont, írja ki, hogy "A mai napon nem rendel"
            if (!hasAvailableTime)
            {
                MenuItems.Add(new MenuItem {Description = $"A mai napon {ChoosenDoctor} nem rendel.", IsButtonVisible = false });

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

            for (int hour = 8; hour <= 16; hour++)
            {
                // Ne lehessen foglalni a közvetlen a következő órára, csak 2 vel későbbire
                if (hour > currentHour + 1)
                {
                    hasAvailableTime = true; // Van elérhető időpont
                                             // Az összes órát hozzáadjuk, nem csak a jövőbeli időpontokat
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

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }
    }
}
