using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Model;  // Győződj meg róla, hogy a megfelelő modellek be vannak importálva.

namespace WpfApp1.UserLogs
{
    public partial class User_Logs : Window
    {
        public User_Logs()
        {
            InitializeComponent();
            LoadLogs(); // Betöltjük a logokat a konstruktorban
        }

        private void LoadLogs()
        {
            using (var context = new MyDbContext())
            {
                // Lekérdezzük az összes logot, beleértve a felhasználói információkat és az involved_user-t
                var logs = context.Logs
                    .Include(l => l.Users)  // Felhasználói információk betöltése
                    .Include(l => l.InvolvedUser)  // Involved user betöltése
                    .Select(l => new
                    {
                        log_id = l.log_id,
                        date = l.date,
                        user_id = l.user_id,
                        Action = l.Action,
                        involved_user = l.involved_user.HasValue ? l.involved_user.Value.ToString() : "N/A" // Ha nincs involved_user, akkor "N/A"
                    })
                    .ToList();

                // A DataGrid-et beállítjuk a lekért logok adatforrásaként
                LogsDataGrid.ItemsSource = logs;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();  // Visszatér a főablakhoz
        }
    }
}
