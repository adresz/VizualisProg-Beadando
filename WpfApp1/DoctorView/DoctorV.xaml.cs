using LoginOptions;
using Model;
using System;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WpfApp1.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorV.xaml
    /// </summary>
    public partial class DoctorV : Window
    {
        public int _accessID;
        public int userID;
        public string Username;
        
        // Konstruktor, amely fogadja az accessID-t
        public DoctorV(int accessID, string username, int user_id)
        {
            InitializeComponent();
            Username = username;
            _accessID = accessID;  // Beállítjuk az accessID értékét
            Application.Current.MainWindow = this;
        }

        // Gomb eseménykezelők
        private void News_Click(object sender, RoutedEventArgs e)
        {
            // Hírek gomb kattintásának kezelése
            MessageBox.Show("Hírek megjelenítése...");
        }

        private void Treatments_Click(object sender, RoutedEventArgs e)
        {
            // Kezelések gomb kattintásának kezelése
            MessageBox.Show("Kezelések megjelenítése...");
        }

        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            // Foglalások gomb kattintásának kezelése
            MessageBox.Show("Foglalásaim megjelenítése...");
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newMainWindow = new MainWindow(); // Új példány létrehozása
            newMainWindow.InitializeComponent();
            Application.Current.MainWindow = newMainWindow; // Új ablak beállítása főablakként
            newMainWindow.Show(); // Új ablak megnyitása
            this.Close();
            // Logolás: Admin kijelentkezett
            LogAction($"[Doktor] {Username} kijelentkezett");
        }

        private void UserGrid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogAction(string action)
        {
            using (var context = new MyDbContext())
            {
                var log = new Logs
                {
                    user_id = userID, // Admin ID-ja (vagy 0 ha nincs azonosító)
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
