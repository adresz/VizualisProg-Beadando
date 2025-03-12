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
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként5
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();
        }
    }
}
