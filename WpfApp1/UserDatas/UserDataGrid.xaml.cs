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
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Users_details> User_details { get; set; }
        public ObservableCollection<UserDetailsViewModel> UsersWithDetails { get; set; }
        

        public UserDataGrid()
        {
            InitializeComponent();
            User_details = new ObservableCollection<Users_details>();
            Users = new ObservableCollection<User>();
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
            using (var db = new AppDBContext())
            {
                var users = db.users.ToList();
                var userDetails = db.user_details.ToList();

                var joinedData = users.Join(
                    userDetails,
                    u => u.email,
                    ud => ud.email,
                    (u, ud) => new UserDetailsViewModel
                    {
                        Username = u.Username,
                        Email = u.email,
                        BanReason = ud.Ban_Reason == "Nincs tiltva." ? "-" : ud.Ban_Reason,
                        Birth_Date = ud.Birth_Date,
                        First_Name = ud.First_Name,
                        Last_Name = ud.Last_Name,
                        Phone_number = ud.Phone_number,
                        Taj_Number = ud.Taj_Number,
                        Banned = ud.isBanned == 1 ? "Igen" : "Nem",
                        AccessRole = u.AccessID == 2 ? "Owner" : u.AccessID == 1 ? "Admin" : "User",
                        Gender = ud.Gender
                    }
                ).ToList();

                UsersWithDetails = new ObservableCollection<UserDetailsViewModel>(joinedData);
                UpdateDataGrid();
            }
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
            var pagedData = UsersWithDetails.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
            UsersDataGrid.ItemsSource = pagedData;

            // Gombok engedélyezése/letiltása
            PreviousButton.IsEnabled = CurrentPage > 1;
            NextButton.IsEnabled = (CurrentPage * ItemsPerPage) < UsersWithDetails.Count;
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if ((CurrentPage * ItemsPerPage) < UsersWithDetails.Count)
            {
                CurrentPage++;
                UpdateDataGrid();
            }
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = UsersDataGrid.SelectedItem as dynamic;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }

    public class UserDetailsViewModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? BanReason { get; set; }
        public DateTime Birth_Date { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Phone_number { get; set; }
        public string? Taj_Number { get; set; }
        public string? Banned { get; set; }
        public string? AccessRole { get; set; }
        public string? Gender { get; set; }
    }

}

