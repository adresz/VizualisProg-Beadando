using LoginOptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using BCrypt;
using System.Linq;
using System.Linq.Expressions;
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
using WpfApp1.UserView;
using Model;


namespace WpfApp1.RegisterView
{
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Birthday.DisplayDateEnd = DateTime.Today;
            Birthday.DisplayDateStart = DateTime.Parse("1900-01-01");
        }
        public int userID;

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            PasswordHide();
            //külön külön futtatva, hogy egyszerre több fajta hiba is kijöjjön
            bool isValidText = ValidateText();
            bool isCorrectFormEmail = ValidateEmail(Email.Text);
            bool isAvailable = isNotTaken();

            if (isValidText & isCorrectFormEmail & isAvailable)
            {
                MessageBox.Show("Sikeres regisztráció");
                SendData();
                UserV userView = new UserV(1, Username.Text, userID);
                userView.Show();
                this.Close();
                Application.Current.MainWindow.Close();
            }
        }

        private void SendData()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    // Jelszó hashelés
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password.Password);

                    // Új felhasználó létrehozása
                    var user = new Users
                    {
                        username = Username.Text,
                        password = hashedPassword,
                        access_id = 1, // Alapértelmezett hozzáférés pl. "User"
                        created_at = DateTime.Now,
                        is_banned = false,
                        ban_reason = null
                    };
                    userID = user.user_id;
                    context.Users.Add(user);
                    context.SaveChanges(); // Az ID generálódik itt

                    // Felhasználó részletei
                    var details = new User_details
                    {
                        user_id = user.user_id,
                        email = Email.Text,
                        first_name = FirstName.Text,
                        last_name = LastName.Text,
                        phone_Number = Phone.Text,
                        taj_Number = ID.Text,
                        gender = (bool)Gender_M.IsChecked ? 1 : 0
                    };

                    context.User_details.Add(details);

                    // Naplózás
                    var log = new Logs
                    {
                        user_id = user.user_id,
                        Action = $"Új regisztráció: {Username.Text}",
                        involved_user = null,
                        date = DateTime.Now
                    };

                    context.Logs.Add(log);

                    // Minden mentése
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt a regisztráció mentésekor:\n" + ex.Message);
            }
        }


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
            var taken = "Hiba:";

            try
            {
                List<TextBox> information = [Email, Phone, ID, Username];
                string[] errormsg = { ", az email cím", ", a telefonszám", ", a tajkártya szám", ", a felhasználónév" };
                string[] textBoxmsg = { "A megadott email foglalt", "A megadott tel.szám foglalt", "A megadott azonosító foglalt", "A megadott név foglalt" };

                using (var context = new MyDbContext())
                {
                    // Email
                    string email = Email.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(email) && context.User_details.Any(u => u.email == email))
                    {
                        Email.BorderBrush = Brushes.Red;
                        taken += errormsg[0];
                        available = false;
                    }

                    // Phone
                    string phone = Phone.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(phone) && context.User_details.Any(u => u.phone_Number == phone))
                    {
                        Phone.BorderBrush = Brushes.Red;
                        taken += errormsg[1];
                        available = false;
                    }

                    // TAJ
                    string taj = ID.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(taj) && context.User_details.Any(u => u.taj_Number == taj))
                    {
                        ID.BorderBrush = Brushes.Red;
                        taken += errormsg[2];
                        available = false;
                    }

                    // Username
                    string username = Username.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(username) && context.Users.Any(u => u.username == username))
                    {
                        Username.BorderBrush = Brushes.Red;
                        taken += errormsg[3];
                        available = false;
                    }
                }

                if (!available)
                {
                    taken = taken.TrimEnd(',', ' ') + " már foglalt.";
                    MessageBox.Show(taken, "Már létező adatok", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Váratlan hiba lépett fel, lépjen kapcsolatba az ügyfélszolgálattal.\n\n" + err.Message,
                                "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return available;
        }


        private bool ValidateUsername(string username)
        {
            string pattern = @"^[A-Za-z0-9_.]{6,20}$";
            return Regex.IsMatch(username, pattern);
        }

        private bool isCorrectLength()
        {
            bool valid = true;

            if (ID.Text.Length != 9)
            {
                ID.BorderBrush = Brushes.Red;
                ID_err.Visibility = Visibility.Visible;
                ID_err.Text = "Az azonosító 9 számjegy kell legyen";
                valid = false;
            }
            else
            {
                ID.BorderBrush = Brushes.Gray;
                ID_err.Visibility = Visibility.Hidden;
            }

            if (Phone.Text.Length != 11)
            {
                Phone.BorderBrush = Brushes.Red;
                Phone_err.Visibility = Visibility.Visible;
                Phone_err.Text = "A telefonszám 11 számjegy kell legyen";
                valid = false;
            }
            else
            {
                Phone.BorderBrush = Brushes.Gray;
                Phone_err.Visibility = Visibility.Hidden;
            }

            return valid;
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

            if (!ValidateUsername(Username.Text))
            {
                Username.BorderBrush = Brushes.Red;
                Username_err.Visibility = Visibility.Visible;
                Username_err.Text = "A felhasználó név legalább 6 karakter legyen";
                hasError = true;
            }
            else
            {
                Username.BorderBrush = Brushes.Gray;
                Username_err.Visibility = Visibility.Hidden;
                hasError = false;
            }

            return !hasError & isCorrectLength() & ValidatePassword() & ValidateBirthday();
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

        public bool ValidateEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrWhiteSpace(email))
            {
                Email.BorderBrush = Brushes.Red;
                Email_err.Visibility = Visibility.Visible;
                Email_err.Text = "Kötelező mező";
                return false;
            }
            else if (!Regex.IsMatch(email, pattern))
            {
                Email.BorderBrush = Brushes.Red;
                Email_err.Visibility = Visibility.Visible;
                Email_err.Text = "Hibás formátum";
                return false;
            }
            else
            {
                Email.BorderBrush = Brushes.Gray;
                Email_err.Visibility = Visibility.Hidden;
                Email_err.Text = "";
            }

            return true;
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

        //Ablaka bezárással/visszalépssel kapcsolatosak
        
        protected override void OnClosed(EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();
        }
    }
}
