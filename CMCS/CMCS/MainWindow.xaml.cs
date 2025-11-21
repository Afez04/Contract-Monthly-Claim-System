
using CMCS.Pages;
using CMCS.Users;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currentUserId;

        public MainWindow()
        {
            InitializeComponent();
            _currentUserId = 0;
        }
        public MainWindow(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        private void Lecture_Click(object sender, RoutedEventArgs e)
        {
            //new LecturerDashboard().Show();
            LectureLoginPage loginPage = new LectureLoginPage(_currentUserId);
            loginPage.Show();
            this.Close();
        }

        private void Coordinator_Click(object sender, RoutedEventArgs e)
        {
            //new CoordinatorDashboard().Show();
            CordinatorLogin cordinatorLogin = new CordinatorLogin(_currentUserId);
            cordinatorLogin.Show();
            this.Close();
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            //new ManagerDashboard().Show();
            ManagerLogin managerLogin = new ManagerLogin(_currentUserId);
            managerLogin.Show();
            this.Close();
        }

        private void HR_Click(object sender, RoutedEventArgs e)
        {
            HRLogin hR = new HRLogin();
            hR.Show();
            this.Close();
        }
    }
}