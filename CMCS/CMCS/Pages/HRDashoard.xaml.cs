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
    /// Interaction logic for HRDashoard.xaml
    /// </summary>
    public partial class HRDashoard : Window
    {
        private int _currentUserId;
        public HRDashoard(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void ManageLectures_Click(object sender, RoutedEventArgs e)
        {
            ManageLecturePage manageLecturePage = new ManageLecturePage(_currentUserId);
            manageLecturePage.Show();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            HRReportPage hrReportPage = new HRReportPage(_currentUserId);
            hrReportPage.Show();
        }

        private void ClaimsOverview_Click(object sender, RoutedEventArgs e)
        {
            HRClaimOverviewPage overviewPage = new HRClaimOverviewPage(_currentUserId);
            overviewPage.Show();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            HRSettingsPage settingsPage = new HRSettingsPage(_currentUserId);
            settingsPage.Show();
        }

        private void ManageCoordinator_Click(object sender, RoutedEventArgs e)
        {
            ManageCoordinatorPage manageCoordinatorPage = new ManageCoordinatorPage(_currentUserId);
            manageCoordinatorPage.Show();
        }

        private void manageManagers_Click(object sender, RoutedEventArgs e)
        {
            ManageManagerPage manageManagerPage = new ManageManagerPage(_currentUserId);
            manageManagerPage.Show();
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
