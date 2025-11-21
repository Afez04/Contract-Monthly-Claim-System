using CMCS.Data;
using CMCS.Models;
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

namespace CMCS.Pages
{
    /// <summary>
    /// Interaction logic for ManageCoordinatorPage.xaml
    /// </summary>
    public partial class ManageCoordinatorPage : Window
    {
        private int _currentUserId;
        public ManageCoordinatorPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadCoordinators();
        }

        //Load Coordinator into data grid
        private void LoadCoordinators()
        {
            using (var db = new ApplicationDbContext())
            {
                var coordinators = db.Users.Where(u => u.Role == "Coordinator").Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                CoordinatorTable.ItemsSource = coordinators;
            }
        }

        //Generate Username
        private void btnGenerateUsername_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int number = random.Next(1000, 9999);
            txtUsername.Text = $"CD{number}";
        }

        //Generate password
        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                new CMCS.Dialogs.ErrorDialog("Enter full name first.").ShowDialog();
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string firstName = fullName.Split(' ')[0];

            Random random = new Random();
            int num = random.Next(1000, 9999);

            txtPassword.Text = $"{firstName}{num}";
        }

        //Add Coordinator
        private void AddCoordinator_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                new CMCS.Dialogs.ErrorDialog("Fill in all required fields before adding coordinator.").ShowDialog();
                return;
            }

           

            using (var db = new ApplicationDbContext())
            {
                var coordinator = new User
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = "Coordinator"
                };
                db.Users.Add(coordinator);
                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Coordinator added successfully").ShowDialog();
            ClearFields();
            LoadCoordinators();
        }

        //Update Coordinator
        private void updateCoordinator_Click(object sender, RoutedEventArgs e)
        {
            if (CoordinatorTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a coordinator to update.").ShowDialog();
                return;
            }

            dynamic selected = CoordinatorTable.SelectedItem;
            int id = selected.UserId;

            using (var db = new ApplicationDbContext())
            {
                var coordinator = db.Users.Find(id);
                if (coordinator == null) return;

                coordinator.FullName = txtFullName.Text.Trim();
                coordinator.Email = txtEmail.Text.Trim();
                coordinator.Username = txtUsername.Text.Trim();
                coordinator.PasswordHash = txtPassword.Text.Trim();


                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Coordinator updated successfully").ShowDialog();
            LoadCoordinators();
        }

        //Delete Coordinator
        private void DeleteCoordinator_Click(object sender, RoutedEventArgs e)
        {
            if (CoordinatorTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a coordinator to delete").ShowDialog();
                return;
            }

            dynamic selected = CoordinatorTable.SelectedItem;
            int id = selected.UserId;

            var confirm = new CMCS.Dialogs.ConfirmDialog("Are you sure you want to delete this coordinator");
            confirm.ShowDialog();

            if (!confirm.IsConfirmed) return;

            using (var db = new ApplicationDbContext())
            {
                var coordinator = db.Users.Find(id);
                if (coordinator != null)
                {
                    db.Users.Remove(coordinator);
                    db.SaveChanges();
                }
            }

            new CMCS.Dialogs.SuccessDialog("Coordinator deleted successfully").ShowDialog();
            LoadCoordinators();
        }

        //Search Lectures
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            using (var db = new ApplicationDbContext())
            {
                var results = db.Users.Where(u => u.Role == "Coordinator" && (u.FullName.ToLower().Contains(searchText) || u.Username.ToLower().Contains(searchText))).Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                CoordinatorTable.ItemsSource = results;
            }
        }

        //Populate Fields when user clicks on datagrid
        private void LecturerTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoordinatorTable.SelectedItem == null) return;

            dynamic selected = CoordinatorTable.SelectedItem;

            txtFullName.Text = selected.FullName;
            txtEmail.Text = selected.Email;
            txtUsername.Text = selected.Username;
            txtPassword.Text = selected.PasswordHash;
        }

        //Clear fields
        private void ClearFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
