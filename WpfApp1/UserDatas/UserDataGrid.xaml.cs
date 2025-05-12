using LoginOptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using WpfApp1.UserView;
using WpfApp1.AdminView;

namespace WpfApp1.UserDatas
{

    /// <summary>
    /// Interaction logic for UserDataGrid.xaml
    /// </summary>
    public partial class UserDataGrid : Window
    {
        public dynamic? SelectedUser { get; set; }
        public int Changed = 0;        

        public UserDataGrid()
        {
            InitializeComponent();
            LoadDataFromDatabase();
            this.Closing += Window_Closing;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SelectedCombo.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                var selectedValue = selectedItem.Content.ToString();
                int.TryParse(selectedValue, out int selectedValueInt);
                ItemsPerPage = selectedValueInt;
                LoadDataFromDatabase();
            }
        }

        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } 

        private void LoadDataFromDatabase()
        {
            
        }

        private void UsersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                SelectedUser = UsersDataGrid.SelectedItem as dynamic;
                MessageBox.Show($"Dupla kattintás történt: {SelectedUser.Username}");
            }
        }

        private void UpdateDataGrid()
        {
  
        }

        private void DataGrid_Value_Changed(Object sender, EventArgs e)
        {
            Changed += 1;
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = UsersDataGrid.SelectedItem as dynamic;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //Ablak zárással, vissza/per előre lépéssel kapcsolatosak

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
   
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateDataGrid();
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.MainWindow.Show();
        }

        private void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Changed > 0)
            {
                MessageBoxResult result = MessageBox.Show("Szeretni menteni kilépés előtt?", "Mentés", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    //BAN REASON CSAK AKKOR LEGYEN ADHATÓ HA ISBANNED = 1
                    // Mentés ki dolgozása
                }
                else if (result == MessageBoxResult.No)
                {
                    Application.Current.MainWindow.Show();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; // Ne záródjon be az ablak
                }
            }
            else
            {
                Application.Current.MainWindow.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }

}