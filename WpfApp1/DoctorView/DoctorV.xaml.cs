using LoginOptions;
using Model;
using System.Windows;
using WpfApp1.PreviousAppointments; // Fontos importálni a PreviousAppointments névteret

namespace WpfApp1.DoctorView
{
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
            userID = user_id;
            _accessID = accessID;  // Beállítjuk az accessID értékét
            Application.Current.MainWindow = this;
        }

        // Previous Appointment gomb eseménykezelője
        private void PreviousAppointment_Click(object sender, RoutedEventArgs e)
        {
            // Létrehozzuk a PreviousAppointments ablakot és megnyitjuk
            PreviousAppointments.PreviousAppointments asd = new PreviousAppointments.PreviousAppointments(userID, Username);
            LogAction($"[Doktor] {Username} megtekintette az foglalási előzményeket.");
            asd.Show();
            this.Hide();  // Opcióként bezárhatjuk az aktuális ablakot, ha szükséges
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
