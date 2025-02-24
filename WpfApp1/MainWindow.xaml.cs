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
using MySql.Data.MySqlClient;


namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string connectionString = "server=localhost;database=userdatabase;uid=root;pwd=;";
    private string username, password;


    public MainWindow()
    {
        InitializeComponent();
    }

    private void Login_click(object sender, RoutedEventArgs e)
    {
        User();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void User()
    {
        username = UsernameTbox.Text;
        password = PasswordTbox.Password;

        if (AuthenticateUser(username, password))
        {
            MessageBox.Show("Sikeres bejelentkezés!", "Belépés", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("Hibás felhasználónév vagy jelszó!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Register_click(object sender, RoutedEventArgs e)
    {

    }

    private bool AuthenticateUser(string username, string password)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT password FROM users WHERE username = @username LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string hashedPassword = result.ToString();
                        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Hiba történt az adatbázis kapcsolat során: " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        return false;
    }


    }