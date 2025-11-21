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
using CMCS.Pages;
namespace CMCS.Users
{
    /// <summary>
    /// Interaction logic for LectureLoginPage.xaml
    /// </summary>
    public partial class LectureLoginPage : Window
    {
        private int _currentUserId;
        public LectureLoginPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void Lecture_Click (object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                new CMCS.Dialogs.ErrorDialog("Please enter both username and password.").ShowDialog();
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                //Find user by username
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Role == "Lecturer");

                if (user == null)
                {
                    new CMCS.Dialogs.ErrorDialog("Username not found").ShowDialog();
                    return;
                }

                if (user.PasswordHash != password)
                {
                    new CMCS.Dialogs.ErrorDialog("Incorrect Password. Please try again").ShowDialog();
                    return;
                }

                new CMCS.Dialogs.SuccessDialog($"Welcome Back, {user.FullName}").ShowDialog();
                LecturerDashboard lecturerDashboard = new LecturerDashboard(user.UserId);
                lecturerDashboard.Show();
                this.Close();
            }
        }

        private void Back_Click (object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(_currentUserId);
            mainWindow.Show();
            this.Close();
        }

        private void RegisterText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LectureRegistration registration = new LectureRegistration(_currentUserId);
            registration.Show();
            this.Close();
        }
    }
}
