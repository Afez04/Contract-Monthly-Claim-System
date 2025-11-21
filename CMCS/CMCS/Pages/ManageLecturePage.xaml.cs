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
using CMCS.Data;
using CMCS.Models;

namespace CMCS.Pages
{
    /// <summary>
    /// Interaction logic for ManageLecturePage.xaml
    /// </summary>
    public partial class ManageLecturePage : Window
    {
        private int _currentUserId;
        public ManageLecturePage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadLecturers();
        }

        //Load Lectures into data grid
        private void LoadLecturers()
        {
            using (var db = new ApplicationDbContext())
            {
                var lectures = db.Users.Where(u => u.Role == "Lecturer").Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    u.HourlyRate,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                LectureTable.ItemsSource = lectures;
            }
        }

        //Generate Username
        private void btnGenerateUsername_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int number = random.Next(1000, 9999);
            txtUsername.Text = $"LT{number}";
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

        //Add Lecture
        private void AddLecture_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                new CMCS.Dialogs.ErrorDialog("Fill in all required fields before adding lecturer.").ShowDialog();
                return;
            }

            if (!double.TryParse(txtHourlyRate.Text, out double rate))
            {
                new CMCS.Dialogs.ErrorDialog("Hourly Rate must be a valid number").ShowDialog();
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var lecturer = new User
                {
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    HourlyRate = rate,
                    Role = "Lecturer"
                };
                db.Users.Add(lecturer);
                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Lecturer added successfully").ShowDialog();
            ClearFields();
            LoadLecturers();
        }

        //Update lecturer
        private void updateLecturer_Click(object sender, RoutedEventArgs e)
        {
            if (LectureTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a lecturer to update.").ShowDialog();
                return;
            }

            dynamic selected = LectureTable.SelectedItem;
            int id = selected.UserId;

            using (var db = new ApplicationDbContext())
            {
                var lecturer = db.Users.Find(id);
                if (lecturer == null) return;

                lecturer.FullName = txtFullName.Text.Trim();
                lecturer.Email = txtEmail.Text.Trim();
                lecturer.Username = txtUsername.Text.Trim();
                lecturer.PasswordHash = txtPassword.Text.Trim();

                //update hourly rate
                if (double.TryParse(txtHourlyRate.Text, out double rate))
                lecturer.HourlyRate = rate;

                db.SaveChanges();
            }

            new CMCS.Dialogs.SuccessDialog("Lecturer updated successfully").ShowDialog();
            LoadLecturers() ;
        }

        //Delete lecturer
        private void DeleteLecturer_Click(object sender, RoutedEventArgs e)
        {
            if (LectureTable.SelectedItem == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a lecturer to delete").ShowDialog();
                return;
            }

            dynamic selected = LectureTable.SelectedItem;
            int id = selected.UserId;

            var confirm = new CMCS.Dialogs.ConfirmDialog("Are you sure you want to delete this lecturer");
            confirm.ShowDialog();

            if(!confirm.IsConfirmed) return;

            using (var db = new ApplicationDbContext())
            {
                var lecturer = db.Users.Find(id);
                if (lecturer != null)
                {
                    db.Users.Remove(lecturer);
                    db.SaveChanges();
                }
            }

            new CMCS.Dialogs.SuccessDialog("Lecturer deleted successfully").ShowDialog();
            LoadLecturers();
        }

        //Search Lectures
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            using (var db = new ApplicationDbContext())
            {
                var results = db.Users.Where(u => u.Role == "Lecturer" && (u.FullName.ToLower().Contains(searchText) || u.Username.ToLower().Contains(searchText))).Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    u.HourlyRate,
                    PasswordHash = u.PasswordHash
                })
                    .ToList();
                LectureTable.ItemsSource = results; 
            }
        }

        //Populate Fields when user clicks on datagrid
        private void LecturerTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LectureTable.SelectedItem == null) return;

            dynamic selected = LectureTable.SelectedItem;

            txtFullName.Text = selected.FullName;
            txtEmail.Text = selected.Email;
            txtHourlyRate.Text = selected.HourlyRate.ToString();
            txtUsername.Text = selected.Username;
            txtPassword.Text = selected.PasswordHash;
        }

        //Clear fields
        private void ClearFields()
        {
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtHourlyRate.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
