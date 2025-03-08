using LoginOptions;
using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Controls.Primitives;
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
            PasswordHide();
            //Külön megnézve a 2 validáció, hogy egyszerre ha
            // több helyen is bajvan, több üzenet megjelenlhessen
            bool isValid = ValidateText();
            bool isAvailable = isNotTaken();

            if (isValid && isAvailable)
            {
                MessageBox.Show("Sikeres regisztráció");
            }
        }
        //
        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsDigitsOnly(e.Text);
        }

        private bool IsDigitsOnly(string text)
        {
            return text.All(char.IsDigit);
        }

        private bool isNotTaken()
        {
            bool available = true;
            var taken = "Hiba";

            try
            {
                using (var db = new AppDBContext())
                {

                    if (db.users.Any(u => u.email == Email.Text))//Email teszt
                    {
                        taken += ", az email cím";
                        Email.BorderBrush = Brushes.Red;
                        available = false;
                        Email_err.Visibility = Visibility.Visible;
                        Email_err.Text = "A megadott email foglalt";
                    }
                    else
                    {
                        Email.BorderBrush = Brushes.Gray;
                    }

                    if (db.user_details.Any(u => u.Phone_number == Phone.Text))//Telóteszt
                    {
                        taken += ", a telefonszám";
                        Phone.BorderBrush = Brushes.Red;
                        available = false;
                        Phone_err.Visibility = Visibility.Visible;
                        Phone_err.Text = "A megadott tel.szám foglalt";
                    }
                    else
                    {
                        Phone.BorderBrush = Brushes.Gray;
                    }

                    if (db.user_details.Any(u => u.TAJ_Number == ID.Text))//ID teszt
                    {
                        taken += ", a tajkártya szám";
                        ID.BorderBrush = Brushes.Red;
                        available = false;
                        ID_err.Visibility = Visibility.Visible;
                        ID_err.Text = "A megadott azonosito foglalt";
                    }
                    else
                    {
                        ID.BorderBrush = Brushes.Gray;
                    }

                    if (db.users.Any(u => u.Username == Username.Text))//Felhasználónév teszt
                    {
                        taken += ", a felhasználónév";
                        Username.BorderBrush = Brushes.Red;
                        available = false;
                        Username_err.Visibility = Visibility.Visible;
                        Username_err.Text = "A megadott név foglalt";
                    }
                    else
                    {
                        Username.BorderBrush = Brushes.Gray;
                    }

                }

                if (!available)
                {
                    taken = taken.TrimEnd(',', ' ') + " már foglalt.";
                    MessageBox.Show(taken);
                }
                else
                {
                    Username_err.Visibility = Visibility.Hidden;
                    Email_err.Visibility = Visibility.Hidden;
                    ID_err.Visibility = Visibility.Hidden;
                    Phone_err.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Váratlan hiba lépett fel, lépjen kapcsolatba az ügyfélszolgálattal.");
                return false;
            }

            return available;
        }

        private bool ValidateText()
        {
            bool hasError = false;

            foreach (var textBox in new List<TextBox> { LastName, FirstName, Username, Email, Phone, ID })
            {
                TextBlock errorTextBlock = (TextBlock)FindName(textBox.Name + "_err");
                bool empty = string.IsNullOrEmpty(textBox.Text);
                textBox.BorderBrush = empty ? Brushes.Red : Brushes.Gray;
                if (errorTextBlock != null) errorTextBlock.Visibility = empty ? Visibility.Visible : Visibility.Hidden;
                hasError |= empty;
            }
            //Kidolgozásra vár a mező megfelelő hosszána megnézése
            //illetve a mező megfelelő kezdésének feltétele

            return !hasError & ValidatePassword() & ValidateBirthday();
        }


        private bool ValidateBirthday()
        {
            bool hasDate = Birthday.SelectedDate.HasValue; // Átirtam erre, mert az előzőnél lehetséges volt bugoltatni, és dátum nélkül elfogadtatni
            Birthday.BorderBrush = hasDate ? Brushes.Gray : Brushes.Red;
            Birthday.BorderThickness = hasDate ? new Thickness(1) : new Thickness(2);
            Birthday_err.Visibility = hasDate ? Visibility.Hidden : Visibility.Visible;

            return hasDate;
        }

        private bool ValidatePassword()
        {
            bool isEmpty = string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(Passwordconf.Password);
            bool match = Password.Password == Passwordconf.Password && !isEmpty;
            bool isComplex = PasswordComplexity(Password.Password);

            Brush borderBrush = (match && isComplex) ? Brushes.Gray : Brushes.Red;
            Thickness borderThickness = (match && isComplex) ? new Thickness(1) : new Thickness(2);

            foreach (var box in new List<PasswordBox> { Password, Passwordconf })
            {
                box.BorderBrush = borderBrush;
                box.BorderThickness = borderThickness;
            }

            if (isEmpty)
            {
                Password_err.Text = "Kötelező mező";
                Passwordconf_err.Text = "Kötelező mező";
            }
            else if (!match)
            {
                Password_err.Text = "Nem megegyező jelszavak";
                Passwordconf_err.Text = "Nem megegyező jelszavak";
            }
            else if (!isComplex)
            {
                Password_err.Text = "Túl könnyű jelszó";
                Passwordconf_err.Text = "Túl könnyű jelszó";
                MessageBox.Show("A jelszónak legalább 8, legfeljebb 24 karakterből" +
                    "kell állnia, tartalmaznia kell egy kis-és nagybetűt, egy számot, " +
                    "valamint egy speciális karaktert.");

            }

            Password_err.Visibility = (match && isComplex) ? Visibility.Hidden : Visibility.Visible;
            Passwordconf_err.Visibility = (match && isComplex) ? Visibility.Hidden : Visibility.Visible;

            return match && isComplex;
        }


        static bool PasswordComplexity(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).{8,24}$";
            return Regex.IsMatch(password, pattern);
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Visibility == Visibility.Visible)
            {
                SeenPassword.Text = Password.Password;
                Password.Visibility = Visibility.Collapsed;
                SeenPassword.Visibility = Visibility.Visible;
            }
            else
            {
                Password.Password = SeenPassword.Text;
                Password.Visibility = Visibility.Visible;
                SeenPassword.Visibility = Visibility.Collapsed;
            }
        }

        private void TogglePasswordConf_Click(object sender, RoutedEventArgs e)
        {
            if (Passwordconf.Visibility == Visibility.Visible)
            {
                SeenConfPassword.Text = Passwordconf.Password;
                Passwordconf.Visibility = Visibility.Collapsed;
                SeenConfPassword.Visibility = Visibility.Visible;
            }
            else
            {
                Passwordconf.Password = SeenConfPassword.Text;
                Passwordconf.Visibility = Visibility.Visible;
                SeenConfPassword.Visibility = Visibility.Collapsed;
            }
        }
        //Jelszó elrejtése regisztrálás gomb nyomáskor, hogy ne kelljen
        // külön vizsgálni a Password text és jelszó állapotban lévő adatait
        private void PasswordHide()
        {
            if (SeenPassword.Visibility == Visibility.Visible)
            {
                Password.Password = SeenPassword.Text;
                SeenPassword.Visibility = Visibility.Collapsed;
                Password.Visibility = Visibility.Visible;
            }

            if (SeenConfPassword.Visibility == Visibility.Visible)
            {
                Passwordconf.Password = SeenConfPassword.Text;
                SeenConfPassword.Visibility = Visibility.Collapsed;
                Passwordconf.Visibility = Visibility.Visible;
            }
        }

    }
}