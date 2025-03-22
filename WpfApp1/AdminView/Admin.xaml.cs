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
using WpfApp1.UserDatas;
using WpfApp1.UserLogs;

namespace WpfApp1.AdminView
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class AdminV : Window
    {
        public AdminV(int accessID, string Username)
        {
            InitializeComponent();

            if(accessID == 2)
            {
            Title = $"Bejelentkezve mint [Tulaj] {Username}";
            }
            else
            {
            Title = $"Bejelentkezve mint [Admin] {Username}";
            }

            Application.Current.MainWindow = this;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();
        }

        private void UserGrid_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            UserDataGrid asd = new UserDataGrid();
            asd.Show();
        }

        private void LogView_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            User_Logs assd = new User_Logs();
            assd.Show();
        }
    }
}
