using LoginOptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.UserView;
using WpfApp1.AdminView;
using Model;
using System.Security.Policy;

namespace WpfApp1.UserDatas
{
    public partial class UserDataGrid : Window
    {
        public dynamic? SelectedUser { get; set; }
        public int Changed = 0;

        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public int TotalPages { get; set; }

        public UserDataGrid()
        {
            InitializeComponent();
            this.Closing += Window_Closing;
            LoadDataFromDatabase();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = SelectedCombo.SelectedItem as ComboBoxItem;
            if (selectedItem != null && int.TryParse(selectedItem.Content.ToString(), out int selectedValueInt))
            {
                ItemsPerPage = selectedValueInt;
                CurrentPage = 1; // Reset to first page
                LoadDataFromDatabase();
            }
        }

        private void LoadDataFromDatabase()
        {
            using (var context = new MyDbContext())
            {
                var totalItems = context.Users
                    .Include(u => u.User_details)
                    .Where(u => u.User_details != null && u.access_id != null)
                    .Count();

                TotalPages = (int)Math.Ceiling((double)totalItems / ItemsPerPage);
                if (CurrentPage > TotalPages) CurrentPage = TotalPages;
                if (CurrentPage < 1) CurrentPage = 1;

                if(CurrentPage == 1)
                {
                    PreviousButton.IsEnabled = false;
                }
                else if(CurrentPage == TotalPages)
                {
                    NextButton.IsEnabled = false;
                    PreviousButton.IsEnabled = true;
                }
                else
                {
                    NextButton.IsEnabled = true;
                    PreviousButton.IsEnabled = true;
                }

                    int skipCount = (CurrentPage - 1) * ItemsPerPage;

                var users = context.Users
                    .Include(u => u.User_details)
                    .Where(u => u.User_details != null && u.access_id != null)
                    .OrderBy(u => u.user_id)
                    .Skip(skipCount)
                    .Take(ItemsPerPage)
                    .Select(u => new
                    {
                        username = u.username,
                        email = u.User_details.email,
                        AccessRole = u.access_id == 1 ? "Felhasználó" :
                                    (u.access_id == 2 ? "Doktor" :
                                    (u.access_id == 3 ? "Admin" : "Ismeretlen")),
                        first_name = u.User_details.first_name,
                        last_name = u.User_details.last_name,
                        phone_number = u.User_details.phone_Number,
                        taj_number = u.User_details.taj_Number,
                        is_banned = u.is_banned ? "Igen" : "Nem",
                        ban_reason = u.ban_reason,
                        gender = u.User_details.gender == 1 ? "Férfi" : "Nő"
                    })
                    .ToList();

                UsersDataGrid.ItemsSource = users;
            }
        }

        private void UpdateDataGrid()
        {
            LoadDataFromDatabase();
        }

        private void UsersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                SelectedUser = UsersDataGrid.SelectedItem as dynamic;
                MessageBox.Show($"Dupla kattintás történt: {SelectedUser.username}");
            }
        }

        private void DataGrid_Value_Changed(object sender, EventArgs e)
        {
            Changed += 1;
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedUser = UsersDataGrid.SelectedItem as dynamic;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mentés még nincs implementálva és nem is lesz, munkaerő hiányában.");
            // TODO: Mentés logikája
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
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
            if (Changed > 0)
            {
                MessageBoxResult result = MessageBox.Show("Szeretné menteni kilépés előtt?", "Mentés", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // TODO: Mentés implementálása
                }
                else if (result == MessageBoxResult.No)
                {
                    Application.Current.MainWindow.Show();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
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
