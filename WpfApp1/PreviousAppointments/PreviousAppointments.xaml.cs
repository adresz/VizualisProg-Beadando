using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Model;  // Feltételezve, hogy itt van a MyDbContext

namespace WpfApp1.PreviousAppointments
{
    public partial class PreviousAppointments : Window
    {
        public int DoctorID;
        public string Doctor_Name;

        public PreviousAppointments(int doctorId, string doctorName)
        {
            InitializeComponent();
            DoctorID = doctorId;
            Doctor_Name = doctorName;

            // Ellenőrzéshez üzenet, hogy biztosan helyes adat jön-e
            // MessageBox.Show($"DoctorId: {DoctorId}, DoctorName: {DoctorName}");

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    // Lekérjük az összes foglalást, ahol a doctor_id megegyezik a megadott DoctorID-vel
                    var appointments = context.Appointments
                        .Where(a => a.doctor_id == DoctorID)  // Csak azok a foglalások, ahol a doctor_id megegyezik
                        .Select(a => new
                        {
                            DoctorName = Doctor_Name,  // Az orvos neve
                            DateTime = a.datetime,     // A foglalás időpontja
                            Username = a.User.username // A felhasználó neve
                        })
                        .ToList();

                    if (appointments.Count == 0)
                    {
                        MessageBox.Show("Nincsenek foglalások az adott orvoshoz.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    // A lekért adatokat beállítjuk a DataGrid-be
                    AppointmentsDataGrid.ItemsSource = appointments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a foglalások betöltése közben:\n" + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.MainWindow.Show();
        }
    }
}
