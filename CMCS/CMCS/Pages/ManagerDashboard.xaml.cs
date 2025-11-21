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
    /// Interaction logic for ManagerDashboard.xaml
    /// </summary>
    public partial class ManagerDashboard : Window
    {
        private readonly int _currentUserId;
        public ManagerDashboard(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void VerifyReport_Click (object sender, RoutedEventArgs e)
        {
            VerifyReportPage Vrp = new VerifyReportPage();
            Vrp.Show();
            
        }

        private void SystemOverview_Click (object sender, RoutedEventArgs e)
        {
            SystemOverviewPage Sop = new SystemOverviewPage();
            Sop.Show();
            
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
            MessagesContactPage messagesContactPage = new MessagesContactPage(_currentUserId, "Manager");
            messagesContactPage.Show();
        }
    }
}
