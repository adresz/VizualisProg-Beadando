using LoginOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Birthday.DisplayDateEnd = DateTime.Today;
            Birthday.DisplayDateStart = DateTime.Parse("1900.01.01");
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateText())
            {
                MessageBox.Show("Sikeres regisztrálás");
            }
        }
        private bool ValidateText()
        {
            bool hiba = false;
            foreach (var textBox in new List<TextBox> { LastName, FirstName, Username, Email, Phone, ID })
            {
                TextBlock errorTextBlock = (TextBlock)FindName(textBox.Name + "_err");
                bool empty = string.IsNullOrEmpty(textBox.Text);
                textBox.BorderBrush = empty ? Brushes.Red : Brushes.Gray;
                if (errorTextBlock != null) errorTextBlock.Visibility = empty ? Visibility.Visible : Visibility.Hidden;
                hiba |= empty;
            }

            return !hiba & ValidatePassword() & ValidateBirthday();
        }

        private bool ValidateBirthday()
        {
            bool van = Birthday.Text != "";
            Birthday.BorderBrush = van ? Brushes.Gray : Brushes.Red;
            Birthday.BorderThickness = van ? new Thickness(1) : new Thickness(2);
            Birthday_err.Visibility = van ? Visibility.Hidden : Visibility.Visible;

            return van;
        }

        private bool ValidatePassword()
        {
            bool isEmpty = string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(Passwordconf.Password);
            bool match = Password.Password == Passwordconf.Password && !isEmpty;
            Brush borderBrush = match ? Brushes.Gray : Brushes.Red;
            Thickness borderThickness = match ? new Thickness(1) : new Thickness(2);

            foreach (var box in new List<PasswordBox> { Password, Passwordconf })
            {
                box.BorderBrush = borderBrush;
                box.BorderThickness = borderThickness;
            }

            if(!match)
            {
                Password_err.Text = "Nem megegyező jelszavak";
                Passwordconf_err.Text = "Nem megegyező jelszavak";
            }

            if (isEmpty)
            {
                Password_err.Text = "Kötelező mező";
                Passwordconf_err.Text = "Kötelező mező";
            }

            Password_err.Visibility = match ? Visibility.Hidden : Visibility.Visible;
            Passwordconf_err.Visibility = match ? Visibility.Hidden : Visibility.Visible;

            return match;
        }
    }
}