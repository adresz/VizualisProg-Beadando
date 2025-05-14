using LoginOptions;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using WpfApp1.UserDatas;
using WpfApp1.UserLogs;
using Microsoft.Identity.Client;

namespace WpfApp1.UserView
{
    /// <summary>
    /// Interaction logic for UserV.xaml
    /// </summary>
    public partial class UserV : Window
    {
        public string doctor = "";
        public int user_ID;
        public int access_ID;
        public string Username;

        public UserV(int accessID, string username, int user_id)
        {
            Username = username;
            access_ID = accessID;
            user_ID = user_id;
            InitializeComponent();
            Title = $"Bejelentkezve mint: {username}";
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
            LogAction($"{Username} az alábbo orvost választotta: Dr.{doctor}");
        }

        // Dora_Click esemény
        private void Dora_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Dora";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor, 67, user_ID);
            Appointment.Show();

            // Logolás: Admin választotta a Dora orvost
            LogAction($"{Username}  az alábbo orvost választotta: Dr.{doctor}");
        }

        // Musky_Click esemény
        private void Musky_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Musky";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor, 66, user_ID);
            Appointment.Show();

            // Logolás: Admin választotta a Musky orvost
            LogAction($"{Username} az alábbi orvost választotta Dr.{doctor}");
        }

        // MyAppointments_Click esemény
        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MyAppointments.MyAppointments MyAppointments = new MyAppointments.MyAppointments(Username,access_ID);
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

        protected override void OnClosed(EventArgs e)
        {
            this.Close();
        }
    }
}
