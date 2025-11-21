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
    /// Interaction logic for CoordinatorDashboard.xaml
    /// </summary>
    public partial class CoordinatorDashboard : Window
    {
        private readonly int _currentUserId;
        public CoordinatorDashboard(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void ReviewClaim_Click (object sender, RoutedEventArgs e)
        {
            ReviewClaimPage Rp = new ReviewClaimPage();
            Rp.Show();
            
        }

        private void ApproveOrReject_Click (object sender, RoutedEventArgs e)
        {
            ApproveOrRejectPage Rp = new ApproveOrRejectPage();
            Rp.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //Show confirmation Dialog
            //new CMCS.Dialogs.SuccessDialog("Please fill in module details.").ShowDialog();
            var confirmDialog = new CMCS.Dialogs.ConfirmDialog("Are you sure you want to logout");
            confirmDialog.ShowDialog();

            if (confirmDialog.IsConfirmed)
            {
                var successDialog = new CMCS.Dialogs.SuccessDialog("You have been logged out successfully");
                successDialog.ShowDialog();


                var MainWindow = new MainWindow(_currentUserId);
                MainWindow.Show();
                this.Close();
            }
            else
            {

            }
        }

        private void BtnMessages_Clicks(object sender, RoutedEventArgs e)
        {
            MessagesContactPage messagesContactPage = new MessagesContactPage(_currentUserId, "Lecturer");
            messagesContactPage.Show();
        }
    }
}
