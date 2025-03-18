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

        private void LoadDataFromDatabase()
        {
            using (var db = new AppDBContext())
            {
                var users = db.users.ToList();
                var userDetails = db.user_details.ToList();

                // Perform the join and create ObservableCollection with anonymous type
                UsersWithDetails = new ObservableCollection<dynamic>(
                    users.Join(
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
                    )
                );

                // Bind to DataGrid
                UsersDataGrid.ItemsSource = UsersWithDetails;
            }
        }
    }
}


