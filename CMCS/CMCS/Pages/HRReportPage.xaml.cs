using CMCS.Data;
using ControlzEx.Standard;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for HRReportPage.xaml
    /// </summary>
    public partial class HRReportPage : Window
    {
        private int _currentUserId;
        public HRReportPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            LoadReport();
        }

        private void LoadReport(DateTime? start = null, DateTime? end = null)
        {
            using (var db = new ApplicationDbContext())
            {
                var data = db.Claims.Select(c => new
                {
                    c.ClaimId,
                    LectureName = c.User.FullName,
                    c.ModuleName,
                    c.TotalAmount,
                    status = c.status,
                    c.DateSubmitted

                })
                    .ToList();

                //Apply date filter
                if (start != null && end != null)
                {
                    data = data.Where(c => c.DateSubmitted >= start && c.DateSubmitted <= end).ToList();
                }

                ReportGrid.ItemsSource = data;

                //Summary
                txtTotal.Text = data.Count.ToString();
                txtApproved.Text = data.Count(c => c.status == "Approved").ToString();
                txtRejected.Text = data.Count(c => c.status == "Rejected").ToString();
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
            {
                new CMCS.Dialogs.ErrorDialog("Select a date range.");
                return;
            }

            LoadReport(dpStart.SelectedDate, dpEnd.SelectedDate);
        }
        private void ExportPDF_Click(object sender, RoutedEventArgs e)
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
                doc.Add(new iTextSharp.text.Paragraph($"Total Claims: {txtTotal.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph($"Approved Claims: {txtApproved.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph($"Rejected Claims: {txtRejected.Text}", normalFont));
                doc.Add(new iTextSharp.text.Paragraph(" "));

                //Table Section 
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 2f, 2f, 1.5f, 1.5f, });

                //Table Headers
                string[] headers = { "ClaimID", "Lecture", "Module", "Total (R)", "Status" };
                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                    cell.BackgroundColor = new BaseColor(230, 180, 170);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                //Table Data
                foreach (dynamic claim in ReportGrid.ItemsSource)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
