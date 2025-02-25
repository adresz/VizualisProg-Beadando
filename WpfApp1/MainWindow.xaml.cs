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
    //Csatlakozás a helyi SQL adatbázishoz
    private string connectionString = "server=localhost;database=userdatabase;uid=root;pwd=;";
    private string username, password;

    public MainWindow()
    {
        InitializeComponent();
    }



}