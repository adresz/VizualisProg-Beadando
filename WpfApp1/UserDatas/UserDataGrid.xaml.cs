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

namespace WpfApp1.UserDatas
{

    /// <summary>
    /// Interaction logic for UserDataGrid.xaml
    /// </summary>
    public partial class UserDataGrid : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Users_details> User_details { get; set; }
        public ObservableCollection<dynamic> UsersWithDetails { get; set; }
        public UserDataGrid()
        {
            InitializeComponent();
            User_details = new ObservableCollection<Users_details>();
            Users = new ObservableCollection<User>();
            
            LoadDataFromDatabase();
            

        }
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 30;

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
                    (u, ud) => new
                    {
                        u.Username,
                        u.email,
                        ud.Ban_Reason,
                        ud.Birth_Date,
                        ud.First_Name,
                        ud.Last_Name,
                        ud.Phone_number,
                        ud.Taj_Number,
                        ud.isBanned,
                        u.AccessID,
                        ud.Gender
                    }
                ).ToList();

                UsersWithDetails = new ObservableCollection<dynamic>(joinedData);
                UpdateDataGrid();
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


    }
}


