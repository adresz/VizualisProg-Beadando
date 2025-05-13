using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Linq;
using System.Windows;
using WpfApp1.AdminView;
using WpfApp1.DoctorView;
using WpfApp1.RegisterView;
using WpfApp1.UserView;

namespace LoginOptions
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Ne töröld, ha a textBoxok üresek, mert hibát okoz
            if (string.IsNullOrWhiteSpace(Username.Text) || string.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Hiányzó felhasználónév és/vagy jelszó");
                return;
            }

            var username = Username.Text;
            var password = Password.Password;
            var user_id = 0;
            using (var context = new MyDbContext())
            {
                // Keresés a felhasználó adatbázisban
                var user = context.Users
                    .Where(u => u.username == username)
                    .FirstOrDefault(); // Az első megtalált felhasználó, vagy null, ha nem található

                if (user != null)
                {
                    // Ellenőrizzük, hogy a jelszó nem NULL
                    if (!string.IsNullOrEmpty(user.password))
                    {
                        // Ha a felhasználó létezik, ellenőrizzük a jelszót
                        bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, user.password);
                        if (passwordMatches)
                        {
                            user_id = user.user_id;
                            // Sikeres bejelentkezés
                            MessageBox.Show("Sikeres bejelentkezés");

                            // Logolás: sikeres bejelentkezés
                            var log = new Logs
                            {
                                user_id = user.user_id,
                                Action = $"Sikeres bejelentkezés {username} felhasználónéven",
                                involved_user = null, // Az aktuális felhasználó próbálkozása
                                date = DateTime.Now
                            };
                            context.Logs.Add(log);
                            context.SaveChanges();

                            if (user.access_id == 1)
                            {
                                UserV userview = new UserV(1,username, user_id);
                                userview.Show();
                            }
                            if (user.access_id == 2)
                            {
                                DoctorV doctorview = new DoctorV(2, username, user_id);
                                doctorview.Show();
                            }
                            if (user.access_id == 3)
                            {
                                AdminV adminview = new AdminV(3, username, user_id);
                                adminview.Show();
                            }
                            this.Close();
                        }
                        else
                        {
                            // Sikertelen bejelentkezés
                            MessageBox.Show("Hibás jelszó!");

                            // Logolás: sikertelen bejelentkezés
                            var log = new Logs
                            {
                                user_id = user.user_id,
                                Action = $"Sikertelen bejelentkezés: Hibás jelszó {username} felhasználónál.",
                                involved_user = null, // Az aktuális felhasználó próbálkozása
                                date = DateTime.Now
                            };
                            context.Logs.Add(log);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        MessageBox.Show("A felhasználó jelszava nem elérhető.");
                    }
                }
                else
                {
                    // Ha nem található felhasználó
                    MessageBox.Show("Felhasználó nem található");

                    // Logolás: sikertelen bejelentkezés
                    var log = new Logs
                    {
                        user_id = 0, // Nem létező felhasználó
                        Action = "Sikertelen bejelentkezés: Felhasználó nem található",
                        involved_user = null, // Az aktuális felhasználó próbálkozása
                        date = DateTime.Now
                    };
                    context.Logs.Add(log);
                    context.SaveChanges();
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Regisztráció ablak megnyitása
            Register RegisterWindow = new Register();
            RegisterWindow.Show();
            this.Hide();
        }
    }
}
