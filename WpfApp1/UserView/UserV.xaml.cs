using LoginOptions;
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

namespace WpfApp1.UserView
{
    /// <summary>
    /// Interaction logic for UserV.xaml
    /// </summary>
    public partial class UserV : Window
    {
        public string doctor = "";

        public UserV(string username)
        {
            InitializeComponent();
            Title = $"Bejelentkezve mint: {username}";
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();
        }

        private void Mesterseges_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Mesterseges";
            this.Hide();
            Appointment.Appointments Appointment = new Appointment.Appointments(doctor);
            Appointment.Show();
        }

        private void Orsolya_Click(object sender, RoutedEventArgs e)
        {
            doctor = "Orsolya";
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

        private void MyAppointsments_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
