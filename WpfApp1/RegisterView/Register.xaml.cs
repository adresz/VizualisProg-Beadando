using LoginOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.RegisterView
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
<<<<<<< Updated upstream
        }

=======
            Birthday.DisplayDateEnd = DateTime.Today;
            Birthday.DisplayDateStart = DateTime.Parse("1900.01.01");
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

>>>>>>> Stashed changes
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< Updated upstream

        }


=======
            if (ValidateFields())
            {
                MessageBox.Show("Sikeres bejelentkezés");
            }
        }

        private bool ValidateFields()
        {
            List<TextBox> textBoxes = new List<TextBox> { LastName, FirstName, Username, Email, Phone, ID};
            List<PasswordBox> passwordBoxes = new List<PasswordBox> { Password, Passwordconf};
            
            if(Password.Name == Passwordconf.Name)
            {
                passwordBoxes[0].BorderBrush = Brushes.Red;
                passwordBoxes[1].BorderBrush = Brushes.Red;
                return false;
            }

            string tmp = "";
            foreach (var textBox in textBoxes)
            {
                tmp = "";
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.BorderBrush = Brushes.Red;
                    tmp = (string)textBox.GetValue(FrameworkElement.NameProperty) + "_err";
                    TextBlock targetTextBox = (TextBlock)FindName(tmp);
                    targetTextBox.Visibility = Visibility.Visible;

                    return false;
                }
            }
            
            //ellenőrzés kell, hogy adatbázisban létezik-e
            return true;
        }
>>>>>>> Stashed changes
    }
}
