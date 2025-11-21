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
    /// Interaction logic for ViewClaimPage.xaml
    /// </summary>
    public partial class ViewClaimPage : Window
    {
        public ViewClaimPage()
        {
            InitializeComponent();
            LoadClaims();
        }

        private void LoadClaims()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    int currentUserId = 1;

                    var claims = db.Claims.Where(c => c.UserId == currentUserId).Select(c => new
                    {
                        c.ClaimId,
                        c.ModuleName,
                        c.ModuleCode,
                        c.HoursWorked,
                        c.HourlyRate,
                        c.TotalAmount,
                        c.status,
                        c.DateSubmitted,
                    })
                        .ToList();

                    ClaimsGrid.ItemsSource = claims;
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading claims: " + ex.Message);
            }
        }
    }
}
