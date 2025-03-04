using LoginOptions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            Birthday.SelectedDate = DateTime.Now;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();

        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            ValidateFields();

            
        }

        private void Field_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateFields();
        }

        private void Field_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateFields();
        }

        private void ValidateFields()
        {
          

            List<TextBox> textBoxes = new List<TextBox> { lastname, firstname, Username, email, phone_number, id_number};
            List<PasswordBox> passwordBoxes = new List<PasswordBox> { Password, Passwordconf};
           
            foreach (var textBox in textBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {

                    textBox.BorderBrush = Brushes.Red;
                    textBox.BorderThickness = new Thickness(2);
                    switch (textBox.Name)
                    {
                        case "lastname":
                            LastName_err.Visibility = Visibility.Visible;
                            break;
                        case "firstname":
                            firstname_err.Visibility = Visibility.Visible;
                            break;
                        case "Username":
                            Username_err.Visibility = Visibility.Visible;
                            break;
                        case "email":
                            Email_err.Visibility = Visibility.Visible;
                            break;
                        case "phone_number":
                            Phone_err.Visibility = Visibility.Visible;
                            break;
                        case "id_number":
                            ID_err.Visibility = Visibility.Visible;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    textBox.BorderBrush = Brushes.Gray;
                    textBox.BorderThickness = new Thickness(1);

                    switch (textBox.Name)
                    {
                        case "lastname":
                            LastName_err.Visibility = Visibility.Collapsed;
                            break;
                        case "firstname":
                            firstname_err.Visibility = Visibility.Collapsed;
                            break;
                        case "Username":
                            Username_err.Visibility = Visibility.Collapsed;
                            break;
                        case "email":
                            Email_err.Visibility = Visibility.Collapsed;
                            break;
                        case "phone_number":
                            Phone_err.Visibility = Visibility.Collapsed;
                            break;
                        case "id_number":
                            ID_err.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (var passwordBox in passwordBoxes)
            {
                if (string.IsNullOrEmpty(passwordBox.Password))
                {

                    passwordBox.BorderBrush = Brushes.Red;
                    passwordBox.BorderThickness = new Thickness(2);
                    switch (passwordBox.Name)
                    {
                        case "Password":
                            Password_err.Visibility = Visibility.Visible;
                            break;
                        case "Passwordconf":
                            Passwordconf_err.Visibility = Visibility.Visible;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    passwordBox.BorderBrush = Brushes.Gray;
                    passwordBox.BorderThickness = new Thickness(1);
                    switch (passwordBox.Name)
                    {
                        case "Password":
                            Password_err.Visibility = Visibility.Collapsed;
                            break;
                        case "Passwordconf":
                            Passwordconf_err.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        


    }
}
