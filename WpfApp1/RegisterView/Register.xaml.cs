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
            if (ValidateFields())
            {
                MessageBox.Show("Sikeres regisztrálás");
            }
        }

        private bool ValidateFields()
        {
            List<TextBox> textBoxes = new List<TextBox> { LastName, FirstName, Username, Email, Phone, ID};
            List<PasswordBox> passwordBoxes = new List<PasswordBox> { Password, Passwordconf};

            //ehhez már túl fáradt vagyok
            //hogy kell jelszavakat összehasonlítani?
            //Kb 20 percig szenvedtem, hogy rábirjam az igazra,
            //mostmár elvileg müködik ez a csodás bogár :D

            if (string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(Passwordconf.Password) || Password.Password != Passwordconf.Password)
            {
                passwordBoxes[0].BorderBrush = Brushes.Red;
                passwordBoxes[0].BorderThickness = new Thickness(2);
                passwordBoxes[1].BorderBrush = Brushes.Red;
                passwordBoxes[1].BorderThickness = new Thickness(2);
                return false;
            }
            else
            {
                passwordBoxes[0].BorderBrush = Brushes.Gray;
                passwordBoxes[0].BorderThickness = new Thickness(1);
                passwordBoxes[1].BorderBrush = Brushes.Gray;
                passwordBoxes[1].BorderThickness = new Thickness(1);
            }

            /*
            Itt meg nem akarja mindegyik mezőre kiirni
             csak a legfelsőre ami nincs kitöltve, igy kéne lennie?
            továbbá
            az eredeti kódod hibára futott ha hiányzott a második mezőből bármi, szóval kicsit átdolgoztam
            */
            foreach (var textBox in textBoxes)
            {
                string errorTextBlockName = textBox.Name + "_err";
                TextBlock errorTextBlock = (TextBlock)FindName(errorTextBlockName);

                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.BorderBrush = Brushes.Red;

                    if (errorTextBlock != null)
                    {
                        errorTextBlock.Visibility = Visibility.Visible;
                    }

                    
                    return false;
                }
                else
                {
                    textBox.ClearValue(BorderBrushProperty);
                    if (errorTextBlock != null)
                    {
                        errorTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
            }
            //Kell még dátum ellenőrzés majd, meg jelszó komplexitás ellenőrzés

            //ellenőrzés kell, hogy adatbázisban létezik-e
            return true;
        }
    }
}
