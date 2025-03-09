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
                int accessID = user?.AccessID ?? 0;
                var userD = db.user_details.FirstOrDefault(ud => ud.email == user.email);
                var banReason = userD?.Ban_Reason ?? "Ki lett tiltva";
                int isBanned = userD?.isBanned ?? 0;

                if (user != null && BCrypt.Net.BCrypt.Verify(Password.Password, user.Password))
                {
                    if(isBanned == 1)
                    {
                        MessageBox.Show($"A felhasználói fiókja tiltva van.\n Indok: {banReason}");
                    }
                    else if(accessID == 0)
                    {
                        MessageBox.Show("Sikeres bejelentkezés");
                        UserV UserWindow = new UserV();
                        UserWindow.Show();
                        this.Close();
                    }
                    //amíg accessID 1 és 2 között nincs különbség, addig ide nem kell az if
                    else
                    {
                        MessageBox.Show("Sikeres bejelentkezés");
                        AdminV AdminWindow = new AdminV();
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
    public required int AccessID { get; set; }

}

public class Users_details
{
    [Key]
    public required string email { get; set; }
    public required string First_Name { get; set; }
    public required string Last_Name { get; set; }
    public required string Phone_number { get; set; }
    public required string Taj_Number { get;set; }
    public required string Birth_Date { get; set; }
    public required int isBanned { get; set; }
    public required string? Ban_Reason { get; set; }
    public required string Gender { get; set; }
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