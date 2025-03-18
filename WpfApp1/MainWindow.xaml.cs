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
using System.Configuration;
using System.Dynamic;

namespace LoginOptions;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        LoadUserSavedData();
    }

    private void LoadUserSavedData()
    {
        Settings1.Default.Reload();
        RememberMe.IsChecked = Settings1.Default.RememberMe;
        Username.Text = Settings1.Default.RememberMe ? Settings1.Default.Username : "";
        Password.Password = Settings1.Default.RememberMe ? Settings1.Default.Password : "";
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        Settings1.Default.RememberMe = RememberMe.IsChecked == true;
        Settings1.Default.Username = RememberMe.IsChecked == true ? Username.Text : "";
        Settings1.Default.Password = RememberMe.IsChecked == true ? Password.Password : "";
        Settings1.Default.Save();
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
                if (user != null && BCrypt.Net.BCrypt.Verify(Password.Password, user.Password))
                {
                    // Proceed with the login process
                    var userD = db.user_details.FirstOrDefault(ud => ud.email == user.email);
                    int accessID = user?.AccessID ?? 0;
                    var banReason = userD?.Ban_Reason ?? "A felhasználói fiókja ki lett tiltva";
                    int isBanned = userD?.isBanned ?? 0;

                    if (isBanned == 1)
                    {
                        MessageBox.Show($"A felhasználói fiókja tiltva van.\nIndok:\n{banReason}");
                    }
                    else if (accessID == 0)
                    {
                        MessageBox.Show("Sikeres bejelentkezés");
                        UserV UserWindow = new UserV(Username.Text);
                        UserWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sikeres bejelentkezés");
                        AdminV AdminWindow = new AdminV(accessID, Username.Text);
                        AdminWindow.Show();
                        this.Close();
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
    public required string email { get; set; }
    public int? AccessID { get; set; }

}

public class Users_details
{
    [Key]
    public required string email { get; set; }
    public required string First_Name { get; set; }
    public required string Last_Name { get; set; }
    public required string Phone_number { get; set; }
    public required string Taj_Number { get;set; }
    public required DateTime Birth_Date { get; set; }
    public required int isBanned { get; set; }
    public string Ban_Reason { get; set; }
    public required string? Gender { get; set; }
}

public class AppDBContext : DbContext
{
    public DbSet<User> users { get; set; }
    public DbSet<Users_details> user_details{ get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;database=userdatabase;user=root;password=;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}