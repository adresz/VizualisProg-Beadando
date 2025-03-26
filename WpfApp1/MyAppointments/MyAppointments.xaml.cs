using System;
using System.Collections.Generic;
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

namespace WpfApp1.MyAppointments
{
    /// <summary>
    /// Interaction logic for MyAppointments.xaml
    /// </summary>
    public partial class MyAppointments : Window
    {
        public MyAppointments(string Username, string Rank)
        {
            InitializeComponent();
            this.Title = $"{Rank} {Username} foglalásai";
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }
    }
}
