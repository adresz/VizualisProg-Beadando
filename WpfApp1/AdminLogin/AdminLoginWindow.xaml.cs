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

namespace WpfApp1
{

   


    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        //Adatbhez csatlakozás paraméterek:
        private string DatabaseConnection = "server=localhost;database=userdatabase;uid=root;pwd=;";

        public AdminLoginWindow()
        {
            InitializeComponent();
        }

        private void AdminLogin(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;


            if (CheckLogin(username, password))
            {
                MessageBox.Show("Sikeres bejelentkezés!");
            }
            else
            {
                MessageBox.Show("Hibás felhasználónév vagy jelszó!");

            }
        }

        private bool CheckLogin(string username, string password)
        {
            using (MySqlConnection DBconnect = new MySqlConnection(DatabaseConnection))
            {
                try
                {
                   
                    DBconnect.Open();
                    string ExecutedQuery = "SELECT password FROM users WHERE username = @username";
                    MySqlCommand SelectUserPassword = new MySqlCommand(ExecutedQuery, DBconnect);
                    SelectUserPassword.Parameters.AddWithValue("@username", username);

                    object returnedData = SelectUserPassword.ExecuteScalar();
                    


                    if (returnedData != null)
                    {
                        string hashedPassword = returnedData.ToString(); //Titkositott jelszó megkapása
                        
                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword)) //jelszó össze hasonlitás
                        {
                            return true; //Bejelentkeztetés
                        }
                    }
                    return false; //Jelszó hiba
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Hiba történt a futás során: {e.Message}");
                    return false;
                }
            }
        }
    }
}