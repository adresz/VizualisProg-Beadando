using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1.Appointment
{
    public class MenuItem
    {
        public string Time { get; set; }
        public string Description { get; set; }
    }

    public partial class Appointments : Window
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; } = new ObservableCollection<MenuItem>();

        public Appointments(string doctor)
        {
            InitializeComponent();
            Title = $"Választott orvosa: {doctor}";

            DataContext = this;
            UpdateMenu();
            StartTimer();
        }

        private void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1) // Percenként frissítés
            };
            timer.Tick += (sender, args) => UpdateMenu();
            timer.Start();
        }

        private void UpdateMenu()
        {
            string isAvailable = "Elérhető";
            int currentHour = DateTime.Now.Hour;
            MenuItems.Clear();

            for (int hour = 8; hour <= 18; hour++)
            {
                //Ne lehessen foglalni a következő órára, csak 2 vel későbbire
                if (hour > currentHour+1)
                {
                    // Az összes órát hozzáadjuk, nem csak a jövőbeli időpontokat
                    MenuItems.Add(new MenuItem { Time = $"{hour}:00", Description = $"{isAvailable} időpont" });
                }
                
            }
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

        private void GoBack_click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
