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
    /// Interaction logic for ManageManagerPage.xaml
    /// </summary>
    public partial class ManageManagerPage : Window
    {
        private int _currentUserId;
        public ManageManagerPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadManagers();
        }


        //Load managers into data grid
        private void LoadManagers()
        {
            using (var db = new ApplicationDbContext())
            {
                var lectures = db.Users.Where(u => u.Role == "Manager").Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                ManagerTable.ItemsSource = lectures;
            }
        }

        //Generate Username
        private void btnGenerateUsername_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int number = random.Next(1000, 9999);
            txtUsername.Text = $"MG{number}";
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

        //Add Manager
        private void AddLecture_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                new CMCS.Dialogs.ErrorDialog("Fill in all required fields before adding manager.").ShowDialog();
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var manager = new User
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = "Manager"
                };
                db.Users.Add(manager);
                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Manager added successfully").ShowDialog();
            ClearFields();
            LoadManagers();
        }

        //Update lecturer
        private void updateLecturer_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a manager to update.").ShowDialog();
                return;
            }

            dynamic selected = ManagerTable.SelectedItem;
            int id = selected.UserId;

            using (var db = new ApplicationDbContext())
            {
                var manager = db.Users.Find(id);
                if (manager == null) return;

                manager.FullName = txtFullName.Text.Trim();
                manager.Email = txtEmail.Text.Trim();
                manager.Username = txtUsername.Text.Trim();
                manager.PasswordHash = txtPassword.Text.Trim();

                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Manager updated successfully").ShowDialog();
            LoadManagers();
        }

        //Delete Manager
        private void DeleteLecturer_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a manager to delete").ShowDialog();
                return;
            }

            dynamic selected = ManagerTable.SelectedItem;
            int id = selected.UserId;

            var confirm = new CMCS.Dialogs.ConfirmDialog("Are you sure you want to delete this manager");
            confirm.ShowDialog();

            if (!confirm.IsConfirmed) return;

            using (var db = new ApplicationDbContext())
            {
                var manager = db.Users.Find(id);
                if (manager != null)
                {
                    db.Users.Remove(manager);
                    db.SaveChanges();
                }
            }

            new CMCS.Dialogs.SuccessDialog("Manager deleted successfully").ShowDialog();
            LoadManagers();
        }

        //Search Lectures
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            using (var db = new ApplicationDbContext())
            {
                var results = db.Users.Where(u => u.Role == "Manager" && (u.FullName.ToLower().Contains(searchText) || u.Username.ToLower().Contains(searchText))).Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                ManagerTable.ItemsSource = results;
            }
        }

        //Populate Fields when user clicks on datagrid
        private void LecturerTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ManagerTable.SelectedItem == null) return;

            dynamic selected = ManagerTable.SelectedItem;

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


