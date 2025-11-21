using CMCS.Data;
using ControlzEx.Standard;
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
    /// Interaction logic for HRClaimOverviewPage.xaml
    /// </summary>
    public partial class HRClaimOverviewPage : Window
    {
        private int _currentUserId;
        public HRClaimOverviewPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadLectures();
            LoadClaims();   
        }

        private void LoadLectures()
        {
            using (var db = new ApplicationDbContext())
            {
                var lectureres = db.Users.Where(u => u.Role == "Lecturer").Select(u => new { u.UserId, u.FullName }).ToList();

                cmbLecturer.ItemsSource = lectureres;
                cmbLecturer.DisplayMemberPath = "FullName";
                cmbLecturer.SelectedValuePath = "UserId";

                cmbLecturer.SelectedIndex = -1;
            }
        }

        private void LoadClaims(DateTime? start = null, DateTime? end = null, int ? lecturerId = null)
        {
            using (var db = new ApplicationDbContext())
            {
                var claims = db.Claims.Select(c => new
                {
                    c.ClaimId,
                    LectureName = c.User.FullName,
                    c.ModuleName,
                    c.HoursWorked,
                    c.TotalAmount,
                    status = c.status,
                    c.DateSubmitted
                })
                    .ToList();

                //Filter by dates
                if (start != null && end != null)
                {
                    claims = claims.Where(c => c.DateSubmitted >= start && c.DateSubmitted <= end).ToList();
                }

                //Filter by lecturer
                if (lecturerId != null)
                {
                    claims = claims.Where(c => c.LectureName != null && c.LectureName != null && db.Users.FirstOrDefault(u => u.UserId == lecturerId).FullName == c.LectureName).ToList();
                }

                ClaimGrid.ItemsSource = claims;

                //Summary Data
                txtTotal.Text = claims.Count.ToString();
                txtApproved.Text = claims.Count(c => c.status == "Approved").ToString();
                txtPending.Text = claims.Count(c => c.status == "Pending").ToString();
                txtRejected.Text = claims.Count(c => c.status == "Rejected").ToString();
                txtTotalAnount.Text = claims.Sum(c => c.TotalAmount).ToString("F2");
            }
        }

        //Apply filters
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            DateTime? start = dpStart.SelectedDate;
            DateTime? end = dpEnd.SelectedDate;
            int? lecturerId = cmbLecturer.SelectedValue as int?;

            LoadClaims(start, end, lecturerId);
        }
    }
}
