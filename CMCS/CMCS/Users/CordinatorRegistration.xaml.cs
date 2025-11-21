using CMCS.Data;
using CMCS.Models;
using CMCS.Pages;
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


namespace CMCS.Users
{
    /// <summary>
    /// Interaction logic for CordinatorRegistration.xaml
    /// </summary>
    public partial class CordinatorRegistration : Window
    {
        private int _currentUserId;
        public CordinatorRegistration(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void btnGenerateUsername_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            string username = $"CD{random.Next(1000, 9999)}";
            txtUsername.Text = username;
        }

        //Register Coordinator
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))

                {
                    new CMCS.Dialogs.ErrorDialog("Please fill all fields to proceed.").ShowDialog();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    new CMCS.Dialogs.ErrorDialog("Please Genereate username, you can't proceed without\n generating username").ShowDialog();
                    return;
                }

                using (var db = new ApplicationDbContext())
                {
                    var exists = db.Users.Any(u => u.Email == txtEmail.Text);

                    if (exists)
                    {
                        new CMCS.Dialogs.SuccessDialog("This email is already registered").ShowDialog();
                        return;
                    }

                    var coordinator = new User
                    {
                        FullName = txtFullName.Text.Trim(),
                        Username = txtUsername.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        PasswordHash = txtPassword.Password,
                        Role = "Coordinator"
                    };

                    db.Users.Add(coordinator);
                    db.SaveChanges();
                }

                new CMCS.Dialogs.SuccessDialog("Coordinator registered successfully.").ShowDialog();

                CordinatorLogin cordinatorLogin = new CordinatorLogin(_currentUserId);
                cordinatorLogin.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                new CMCS.Dialogs.ErrorDialog($"Error registering Coordinator: {ex.Message}");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            CordinatorLogin cordinatorLogin = new CordinatorLogin(_currentUserId);
            cordinatorLogin.Show();
            this.Close();
        }
    }
}
