using LoginOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
<<<<<<< Updated upstream
                passwordBoxes[0].BorderBrush = Brushes.Red;
                passwordBoxes[1].BorderBrush = Brushes.Red;
                return false;
            }

            string tmp = "";
            foreach (var textBox in textBoxes)
=======
                List<TextBox> information = [Email, Phone, ID, Username];
                bool[] invalid = {};
                string[] errormsg = {", az email cím", ", a telefonszám", ", a tajkártya szám", ", a felhasználónév"};
                string[] postfix = { "email", "Phone_number", "TAJ_number", "Username" };
                string[] textBoxmsg = {"A megadott email foglalt", "A megadott tel.szám foglalt", "A megadott azonosító foglalt", "A megadott név foglalt"};
                using (var db = new AppDBContext())
                {
                    //csak emailt és username-et ellenőriz, a user_details táblát most nem írtam bele
                    //arra chatgpt dobott egy még vadabb függvényt, és nem számítottam rá hogy ez eddig elhúzódik
                    //szólj és átdobom amit írt
                    for(int i = 0; i < 4; i=i+3)
                    {
                        if (db.users.Any(BuildPredicate<User>(postfix[i], (information[i]).Text)))
                        {
                            taken += errormsg[i];
                            information[i].BorderBrush = Brushes.Red;
                            available = false;
                            TextBlock errorTextBlock = (TextBlock)FindName(information[i].Name + "_err");
                            errorTextBlock.Visibility = Visibility.Visible;
                            errorTextBlock.Text = "A megadott email foglalt";
                        }
                        else if (information[i].BorderBrush != Brushes.Red)
                        {
                            information[i].BorderBrush = Brushes.Gray;
                        }
                    }
                }

                if (!available)
                {
                    taken = taken.TrimEnd(',', ' ') + " már foglalt.";
                    MessageBox.Show(taken);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Váratlan hiba lépett fel, lépjen kapcsolatba az ügyfélszolgálattal.");
                return false;
            }

            return available;
        }

        static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string value)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "u"); // u =>
            var property = System.Linq.Expressions.Expression.Property(parameter, propertyName); // u.PropertyName
            var constant = System.Linq.Expressions.Expression.Constant(value); // value
            var equality = System.Linq.Expressions.Expression.Equal(property, constant); // u.PropertyName == value

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);
        }

        private bool isCorrectLength()
        {
            bool valid = true;

            if (ID.Text.Length != 9)
>>>>>>> Stashed changes
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
