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
    /// Interaction logic for ApproveOrRejectPage.xaml
    /// </summary>
    public partial class ApproveOrRejectPage : Window
    {
        public ApproveOrRejectPage()
        {
            InitializeComponent();
            LoadProcessedClaims();
        }

        private void LoadProcessedClaims()
        {
            using (var db = new ApplicationDbContext())
            {
                var processedClaims = db.Claims.Where(c => c.status == "Approved" || c.status == "Rejected").Select(c => new
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
                ProcessedClaimsGrid.ItemsSource = processedClaims;
            }
        }
    }
}
