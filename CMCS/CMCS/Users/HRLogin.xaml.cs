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
using CMCS.Dialogs;
using CMCS.Pages;

namespace CMCS.Users
{
    /// <summary>
    /// Interaction logic for HRLogin.xaml
    /// </summary>
    public partial class HRLogin : Window
    {
        // Default HR Credentials
        private const string DefaultHRUsername = "Andiswa39";
        private const string DefaultHRPassword = "Phewa#778P";
        public HRLogin()
        {
            InitializeComponent();
        }

        private void Coordinator_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                new ErrorDialog("Please enter both username and password.").ShowDialog();
                return;
            }

            // VALIDATION
            if (username != DefaultHRUsername)
            {
                new ErrorDialog("Incorrect HR username.").ShowDialog();
                return;
            }

            if (password != DefaultHRPassword)
            {
                new ErrorDialog("Incorrect HR password.").ShowDialog();
                return;
            }

            // SUCCESS
            new SuccessDialog("Welcome HR Manager!").ShowDialog();

            // Open HR Dashboard
            HRDashoard hr = new HRDashoard(999); // HR userId can be any dummy value
            hr.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(0);
            main.Show();
            this.Close();
        }

        private void RegisterText_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new ErrorDialog("HR registration is disabled — only system admin can add HR users.").ShowDialog();
        }


    }
}
