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

namespace LoginOptions;

public partial class MainWindow : Window
{



    public void LoginOptions()
    {
        InitializeComponent();
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (var db = new AppDBContext())
            {
                var user = db.users.FirstOrDefault(u => u.Username == Username.Text);

                if (user != null && BCrypt.Net.BCrypt.Verify(Password.Password, user.Password) && (user.AccessID == 3 || user.AccessID == 2))
                {

                    MessageBox.Show("Sikeres bejelentkezés");

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
        this.Close();
    }
}


public class User
{
    [Key] // Enélkül össze szarja magát az adatbázis, ne nyúlj hozzá 
    public string Username { get; set; }
    public string Password { get; set; }
    public int AccessID { get; set; }
    



    
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