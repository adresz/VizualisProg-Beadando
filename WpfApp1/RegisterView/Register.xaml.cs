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
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
            this.Close();

        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {

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
            //last name mezőre való hiba jelzés, ha üres lenne a mező
            if (string.IsNullOrEmpty(lastname.Text))
            {
                lastname.BorderBrush = Brushes.Red;
                lastname.BorderThickness = new Thickness(2);
                if (lastname.Name == "lastname")
                    LastName_err.Visibility = Visibility.Visible;
            }
            else
            {
                lastname.BorderBrush = Brushes.Gray;
                lastname.BorderThickness = new Thickness(1);
                LastName_err.Visibility = Visibility.Collapsed;
            }
        }

    }
}
