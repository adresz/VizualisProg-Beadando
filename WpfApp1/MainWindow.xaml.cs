using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCrypt.Net;
using WpfApp1;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using WpfApp1.RegisterView;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WpfApp1.AdminView;
using WpfApp1.UserView;
using System.Runtime.CompilerServices;

namespace LoginOptions;

public partial class MainWindow : Window
{



    public void LoginOptions()
    {
        InitializeComponent();

       
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        //Ne töröld, mert hibára fut a kód, ha bejelentkezéskor nincs kitöltve a textBox
        if (string.IsNullOrWhiteSpace(Username.Text) || string.IsNullOrWhiteSpace(Password.Password))
        {
            MessageBox.Show("Hiányzó felhasználónév és/vagy jelszó");
            return;
        }

        try
        {
            using (var db = new AppDBContext())
            {
                var user = db.users.FirstOrDefault(u => u.Username == Username.Text);
                int accessID = user.AccessID;
                int isBanned = user.isBanned;
                if (user != null && BCrypt.Net.BCrypt.Verify(Password.Password, user.Password))
                {
                    if (accessID == 2 || accessID == 3)
                    {
                        if (isBanned == 1)
                        {
                            MessageBox.Show("A felhasználói fiókja tiltva van.");
                        }
                        else
                        {
                            MessageBox.Show("Sikeres bejelentkezés");
                            AdminV AdminWindow = new AdminV();
                            AdminWindow.Show();
                            this.Close();
                        }

                    }
                    else if (accessID == 0)
                    {
                        if (isBanned == 1)
                        {
                            MessageBox.Show("A felhasználói fiókja tiltva van.");
                        }
                        else
                        {
                            MessageBox.Show("Sikeres bejelentkezés");
                            UserV UserWindow = new UserV();
                            UserWindow.Show();
                            this.Close();
                        }
                    }


                }
                else
                {
                    MessageBox.Show("Hibás felhasználónév vagy jelszó");
                }

            }

        }
        catch (Exception err)
        {
            MessageBox.Show("Hiba történt,kérjük lépjen kapcsolatba az ügyfélszolgálattal: +\n" + err);
        }
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        Register RegisterWindow = new Register();
        RegisterWindow.Show();
        this.Hide();
        
    }
    
}


public class User
{
    [Key] // Enélkül össze szarja magát az adatbázis, ne nyúlj hozzá 
    public required string Username { get; set; }
    public required string Password { get; set; }
    public int AccessID { get; set; }
    public int isBanned { get; set; }


    

}

public class AppDBContext : DbContext
{
    public DbSet<User> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var connectionString = "server=localhost;database=userdatabase;user=root;password=;";


        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));



    }
}