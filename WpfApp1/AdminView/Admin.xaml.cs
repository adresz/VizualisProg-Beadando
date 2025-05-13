using LoginOptions;
using Microsoft.VisualBasic.ApplicationServices;
using Model;
using System;
using System.Linq;
using System.Windows;
using WpfApp1.MyAppointments;
using WpfApp1.UserDatas;
using WpfApp1.UserLogs;

namespace WpfApp1.AdminView
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class AdminV : Window
    {
        public string doctor = "";
        public string Username = "";
        public string Rank = "";
        public int user_ID;

        public AdminV(int accessID, string Username, int userID)
        {
            InitializeComponent();
            this.Username = Username;
            user_ID = userID;
            if (accessID == 3)
            {
                Title = $"Bejelentkezve mint [Admin] {Username}";
                Rank = "[Admin]";
            }

            Application.Current.MainWindow = this;
        }

        // UserGrid_Click esemény
        private void UserGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            UserDataGrid DataGrid = new UserDataGrid();
            DataGrid.Show();

            // Logolás: Admin belépett a felhasználók nézetére
            LogAction($"[Admin] {Username} belépett a Felhasználók nézetére");
        }

        // LogView_Click esemény
        private void LogView_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            User_Logs UserLog = new User_Logs();
            UserLog.Show();

            // Logolás: Admin belépett a napló nézetére
            LogAction($"[Admin] {Username} belépett a Napló nézetére");
        }

        // Mesterseges_Click esemény
        private void Mesterseges_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Mesterseges";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor, 68, user_ID);
            Appointment.Show();

            // Logolás: Admin választotta a Mesterseges orvost
            LogAction($"[Admin] {Username} választotta a {doctor} orvost");
        }

        // Dora_Click esemény
        private void Dora_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Dora";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor, 67, user_ID);
            Appointment.Show();

            // Logolás: Admin választotta a Dora orvost
            LogAction($"[Admin] {Username} választotta a {doctor} orvost");
        }

        // Musky_Click esemény
        private void Musky_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Musky";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor, 66, user_ID);
            Appointment.Show();

            // Logolás: Admin választotta a Musky orvost
            LogAction($"[Admin] {Username} választotta a {doctor} orvost");
        }

        // MyAppointments_Click esemény
        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MyAppointments.MyAppointments MyAppointments = new MyAppointments.MyAppointments(Username, Rank);
            MyAppointments.Show();

            // Logolás: Admin belépett a Saját időpontok nézetére
            LogAction($"[Admin] {Username} belépett a Saját időpontok nézetére");
        }

        // LogOut_Click esemény
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();

            // Logolás: Admin kijelentkezett
            LogAction($"[Admin] {Username} kijelentkezett");
        }

        // Logolás metódus
        private void LogAction(string action)
        {
            using (var context = new MyDbContext())
            {
                var log = new Logs
                {
                    user_id = user_ID, // Admin ID-ja (vagy 0 ha nincs azonosító)
                    Action = action,
                    involved_user = null, // Az aktuális felhasználó próbálkozása
                    date = DateTime.Now
                };
                context.Logs.Add(log);
                context.SaveChanges();
            }
        }
    }
}
