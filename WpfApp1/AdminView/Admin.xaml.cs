using LoginOptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
using WpfApp1.MyAppointments;
using WpfApp1.UserDatas;
using WpfApp1.UserLogs;

namespace WpfApp1.AdminView
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    /// 
    public partial class AdminV : Window
    {

        public string doctor="";
        public string Username="";
        public string Rank = "";

        public AdminV(int accessID, string Username)
        {
            InitializeComponent();
            this.Username = Username;
            if(accessID == 2)
            {
            Title = $"Bejelentkezve mint [Tulaj] {Username}";
                Rank = "[Tulaj]";
            }
            else
            {
            Title = $"Bejelentkezve mint [Admin] {Username}";
                Rank = "[Adsmin]";
            }

            Application.Current.MainWindow = this;
        }

        private void UserGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            UserDataGrid DataGrid = new UserDataGrid();
            DataGrid.Show();
        }

        private void LogView_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            User_Logs UserLog = new User_Logs();
            UserLog.Show();
        }

        private void Mesterseges_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Mesterseges";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor);
            Appointment.Show();
        }

        private void Dora_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Dora";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor);
            Appointment.Show();
        }

        private void Musky_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Musky";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor);
            Appointment.Show();
        }

        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MyAppointments.MyAppointments MyAppointments = new MyAppointments.MyAppointments(Username, Rank);
            MyAppointments.Show();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();
        }
    }
}
