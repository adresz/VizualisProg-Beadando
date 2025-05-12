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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualBasic.ApplicationServices;
using Model;
using Microsoft.VisualBasic;


namespace LoginOptions;

public partial class MainWindow : Window
{
    public MainWindow()
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
        var username = Username.Text;
        var password = Password.Password;


        //1. id => User; 2 id => doctor; 3 id => admin
        // 0 female 1 male


        using (MyDbContext context = new MyDbContext())
        {

            bool userfound = context.Users.Any(User => User.username == username);
            if (userfound)
            {
                MessageBox.Show("Sikeres bejelentkezés");
                AdminV AdminWindow = new AdminV(2, Username.Text);
                AdminWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Felhasználó nem található");
            }
        }
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        Register RegisterWindow = new Register();
        RegisterWindow.Show();
        this.Hide();
    }
}