﻿using LoginOptions;
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
                // SendData();
                UserV userView = new UserV(Username.Text);
                userView.Show();
                this.Close();
                Application.Current.MainWindow.Close();
            }
        }

        private void SendData()
        {
            try
            {
                using (var db = new AppDBContext())
                {
                    // Jelszó titkositás
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password.Password.Trim());

                    // Új user létrehozása
                    User newUser = new User
                    {
                        Username = Username.Text,
                        Password = hashedPassword,
                        email = Email.Text,
                        AccessID = 0 
                    };
                    string selectedGender = Gender_M.IsChecked == true ? "Male" : "Female"; // Gender kikérése
                    // Új user details
                    Users_details newUserDetails = new Users_details
                    {
                        email = Email.Text,
                        First_Name = FirstName.Text,
                        Last_Name = LastName.Text,
                        Phone_number = Phone.Text,
                        Taj_Number = ID.Text,
                        Birth_Date = DateTime.Now,
                        isBanned = 0,
                        Ban_Reason = null,
                        Gender = selectedGender
                    };

                    // Add the new user and user details to the database
                    db.users.Add(newUser);
                    db.user_details.Add(newUserDetails);

                    // Save changes to the database
                    db.SaveChanges();

                    MessageBox.Show("Sikeres regisztráció!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Váratlan hiba lépett fel, kérjük lépjen kapcsolatba az ügyfélszolgálattal.\nHiba kód:\n" + err.Message);
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
            var taken = "Hiba";
            //Else if feltételek kellenek, hogy mindenképp piros legyen
            //Ha nincs ott szürke marad mert üres karakter mindig szabad
            //az adatbázison belül
            try
            {
                List<TextBox> information = [Email, Phone, ID, Username];
                bool[] invalid = {};
                string[] errormsg = {", az email cím", ", a telefonszám", ", a tajkártya szám", ", a felhasználónév"};
                string[] postfix = { "email", "Phone_number", "TAJ_number"};
                string[] textBoxmsg = {"A megadott email foglalt", "A megadott tel.szám foglalt", "A megadott azonosító foglalt", "A megadott név foglalt"};
                using (var db = new AppDBContext())
                {
                    //a username-en kívül minden más a user_detailsben van, nem kell ciklus
                    //ha lesz idő, a usernameet ki lehet venni a listából és konkrétan ideírni
                    if (db.users.Any(u => u.Username == Username.Text))
                    {
                        taken += errormsg[3];
                        information[3].BorderBrush = Brushes.Red;
                        available = false;
                        TextBlock errorTextBlock = (TextBlock)FindName(information[3].Name + "_err");
                        errorTextBlock.Visibility = Visibility.Visible;
                        errorTextBlock.Text = textBoxmsg[3];
                    }

                    for (int i = 1; i < 3; i ++)
                    {
                        if (db.user_details.Any(BuildPredicate<Users_details>(postfix[i], (information[i]).Text)))
                        {
                            taken += errormsg[i];
                            information[i].BorderBrush = Brushes.Red;
                            available = false;
                            TextBlock errorTextBlock = (TextBlock)FindName(information[i].Name + "_err");
                            errorTextBlock.Visibility = Visibility.Visible;
                            errorTextBlock.Text = textBoxmsg[i];
                        }
                    }
                }

                if (!available)
                {
                    taken = taken.TrimEnd(',', ' ') + " már foglalt.";
                    MessageBox.Show(taken);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Váratlan hiba lépett fel, lépjen kapcsolatba az ügyfélszolgálattal\n." + err);
                return false;
            }

            return available;
        }
        //Átalakitás, hogy ne kelljen mindig irni hogy u => u.xy == "valami"
        static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "u"); // u =>
            var property = System.Linq.Expressions.Expression.Property(parameter, propertyName); // u.PropertyName
            var constant = System.Linq.Expressions.Expression.Constant(value); // value
            var equality = System.Linq.Expressions.Expression.Equal(property, constant); // u.PropertyName == value

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);
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

            bool notYoug = ((DateTime.Now.Year - Birthday.SelectedDate.Value.Year) >= 16) && (DateTime.Now.Month >= Birthday.SelectedDate.Value.Month) && (DateTime.Now.Day >= Birthday.SelectedDate.Value.Day);
            Birthday.BorderBrush = notYoug ? Brushes.Gray : Brushes.Red;
            Birthday.BorderThickness = notYoug ? new Thickness(1) : new Thickness(2);
            Birthday_err.Text = "Túl fiatal vagy fiacskám!";
            Birthday_err.Visibility = notYoug ? Visibility.Hidden : Visibility.Visible;

            return hasDate && notYoug;
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
