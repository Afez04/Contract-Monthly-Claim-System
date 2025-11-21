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

namespace CMCS.Pages
{
    /// <summary>
    /// Interaction logic for ReviewClaimPage.xaml
    /// </summary>
    public partial class ReviewClaimPage : Window
    {
        public ReviewClaimPage()
        {
            InitializeComponent();
            LoadClaims();
        }

        private void LoadClaims()
        {
            using (var db = new ApplicationDbContext())
            {
                //Get claims that are pending
                var claims = db.Claims.Where(c => c.status == "Pending").Select(c => new
                {
                    c.ClaimId,
                    LectureName = c.User.FullName,
                    c.ModuleName,
                    c.ModuleCode,
                    c.HoursWorked,
                    c.TotalAmount,
                    c.status
                })
                    .ToList();

                ClaimsGrid.ItemsSource = claims;
            }
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = ((FrameworkElement)sender).DataContext;
            if (selected == null)
                return;

            int claimId = selected.ClaimId;

            using (var db = new ApplicationDbContext())
            {
                var claim = db.Claims.FirstOrDefault(c => c.ClaimId == claimId);
                if (claim != null)
                {
                    claim.status = "Approved";
                    db.SaveChanges();
                    new CMCS.Dialogs.SuccessDialog($"Claim #{claimId} approved");
                }
            }

            LoadClaims();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = ((FrameworkElement)sender).DataContext;
            if (selected == null) return;

            int claimId = selected.ClaimId;

            using (var db = new ApplicationDbContext())
            {
                var claim = db.Claims.FirstOrDefault(c => c.ClaimId == claimId);
                if(claim != null)
                {
                    claim.status = "Rejected";
                    db.SaveChanges();
                    MessageBox.Show($"Claim #{claimId} rejected");
                    new CMCS.Dialogs.ErrorDialog($"Claim #{claimId} rejected");
                }
            }

            LoadClaims();
        }
    }
}
