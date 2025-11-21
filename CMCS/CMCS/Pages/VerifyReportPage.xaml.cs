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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CMCS.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace CMCS.Pages
{
    /// <summary>
    /// Interaction logic for VerifyReportPage.xaml
    /// </summary>
    public partial class VerifyReportPage : Window
    {
        public VerifyReportPage()
        {
            InitializeComponent();
            LoadReportData();
        }

        private void LoadReportData()
        {
            using (var db = new ApplicationDbContext())
            {
                //Load Claims and user info
                var claim = db.Claims.Select(c => new
                {
                    c.ClaimId,
                    LectureName = c.User.FullName,
                    c.ModuleName,
                    c.FilePath,
                    c.TotalAmount,
                    c.status,
                    c.DateSubmitted
                })
                    .ToList();

                ReportGrid.ItemsSource = claim;

                //Calculate summary data
                int total = claim.Count;
                int approved = claim.Count(c => c.status == "Approved");
                int rejected = claim.Count(c => c.status == "Rejected");

                //Display on summary fields
                txtTotalClaims.Text = total.ToString();
                txtApproved.Text = approved.ToString();
                txtRejected.Text = rejected.ToString();
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"CMCS_Report_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                string fullPath = System.IO.Path.Combine(folderPath, fileName);

                //Create PDF Document 
                Document doc = new Document(PageSize.A4, 30, 30, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(fullPath, FileMode.Create));
                doc.Open();

                //Title
                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD, new BaseColor(38, 103, 110));
                doc.Add(new iTextSharp.text.Paragraph("Contract Monthly Claim System (CMCS)", titleFont));
                doc.Add(new iTextSharp.text.Paragraph(" "));

                //Summary Section 
                Font headerFont = FontFactory.GetFont("Arial", 14, Font.BOLD, new BaseColor(150, 192, 189));
                Font normalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.Black);
                doc.Add(new iTextSharp.text.Paragraph("Claim Summary", headerFont));
                doc.Add(new iTextSharp.text.Paragraph($"Total Claims: {txtTotalClaims.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph($"Approved Claims: {txtApproved.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph($"Rejected Claims: {txtRejected.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph(" "));

                //Table Section 
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 2f, 2f, 1.5f, 1.5f, });

                //Table Headers
                string[] headers = { "ClaimID", "Lecture", "Module", "Total (R)", "Status" };
                foreach(var  header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = new BaseColor(230, 180, 170);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                //Table Data
                foreach(dynamic claim in ReportGrid.ItemsSource)
                {
                    table.AddCell(new Phrase(claim.ClaimId.ToString(), normalFont));
                    table.AddCell(new Phrase(claim.LectureName, normalFont));
                    table.AddCell(new Phrase(claim.TotalAmount.ToString("F2"), normalFont));
                    table.AddCell(new Phrase(claim.status, normalFont));
                }

                doc.Add(table);
                doc.Close();

                MessageBox.Show($"Report exported successfully!!!");

                //Open the PDF after saving
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.InnerException?.Message?? ex.Message}");
            }
        }
    }
}
