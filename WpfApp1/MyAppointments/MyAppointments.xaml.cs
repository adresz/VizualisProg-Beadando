using Model; // Namespace a DbContext és entitások számára
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfApp1.MyAppointments
{
    public partial class MyAppointments : Window
    {
        public int accessID;
        public string username;

        public MyAppointments(string Username, int access_ID)
        {
            accessID = access_ID;
            username = Username;
            InitializeComponent();
            this.Title = $"{Username} foglalásai";

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            using (var db = new MyDbContext())
            {
                // Jelenlegi felhasználó lekérdezése
                var user = db.Users.FirstOrDefault(u => u.username == username);
                if (user == null)
                {
                    MessageBox.Show("Felhasználó nem található.");
                    return;
                }

                int userId = user.user_id;

                // Foglalások lekérdezése, beleértve a felhasználó nevét
                var appointments = db.Appointments
                                     .Where(a => a.user_id == userId)
                                     .Select(a => new AppointmentDisplay
                                     {
                                         appointment_id = a.appointment_id,
                                         datetime = a.datetime,
                                         doctor_name = a.doctor_id == 66 ? "Dr. Muskovics" :
                                                       a.doctor_id == 67 ? "Dr. Sipkovics" :
                                                       a.doctor_id == 68 ? "Dr. Mesterség" :
                                                       $"Ismeretlen (ID: {a.doctor_id})",
                                         username = user.username
                                     })
                                     .ToList();

                AppointmentsDataGrid.ItemsSource = appointments;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }

        // Segédosztály a DataGrid-hez
        public class AppointmentDisplay
        {
            public int appointment_id { get; set; }
            public DateTime datetime { get; set; }
            public string doctor_name { get; set; }
            public string username { get; set; } // A user_id helyett a felhasználó neve jelenik meg
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
