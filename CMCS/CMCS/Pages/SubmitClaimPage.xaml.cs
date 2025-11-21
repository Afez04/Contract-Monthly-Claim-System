using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.Security.Claims;
using CMCS.Data;
using CMCS.Dialogs;

namespace CMCS.Pages
{
    /// <summary>
    /// Interaction logic for SubmitClaimPage.xaml
    /// </summary>
    public partial class SubmitClaimPage : Window
    {
        private string _uploadFilePath = null;
        private readonly int _currentUserId;

        public SubmitClaimPage(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
        }

        public SubmitClaimPage() : this(50) { }
        

        private void txtHours_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txtHours.Text, out double hours))
            {
                using (var db = new ApplicationDbContext())
                {
                    var lecture = db.Users.FirstOrDefault(u => u.UserId == _currentUserId);
                    if(lecture != null)
                    {
                        double rate = lecture.HourlyRate;
                        txtTotal.Text = (hours * rate).ToString("F2");
                    }
                }
            }
            else
            {
                txtTotal.Text = "0";
            }
        }
        //Upload Supported Document 
        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Documents|*.pdf;*.docx;*.xlsx"; //Check for supported document 
            if (dlg.ShowDialog() == true)
            {
                string uploadsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                if (!Directory.Exists(uploadsDir)) 
                    Directory.CreateDirectory(uploadsDir);

                string destFile = Path.Combine(uploadsDir, Path.GetFileName(dlg.FileName));
                File.Copy(dlg.FileName, destFile, true);

                _uploadFilePath = destFile;
                txtFileName.Text = Path.GetFileName(dlg.FileName);
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check for required fields
                if (string.IsNullOrWhiteSpace(txtModule.Text) || string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    //MessageBox.Show("Please fill in module details.");
                   // new CMCS.Dialogs.SuccessDialog("Please fill in module details.").ShowDialog();
                   new CMCS.Dialogs.ErrorDialog("Please fill in module details.").ShowDialog();
                    return;
                }
                //Check for numeric numbers
                 if (!double.TryParse(txtHours.Text, out double hours))// || !double.TryParse(txtRate.Text, out double rate))
                 {
                     new CMCS.Dialogs.ErrorDialog("Please enter a valid numbers for hours.").ShowDialog();
                     return;
                 }
                
                double rate;
                using (var db = new ApplicationDbContext())
                {
                    var lecture = db.Users.FirstOrDefault(u => u.UserId == _currentUserId);
                    if (lecture == null)
                    {
                        new ErrorDialog("User not found").ShowDialog();
                        return;
                    }

                    rate = lecture.HourlyRate;
                }
                    //Calculate tota Amount
                    double total = hours * rate;
                //Display Data table
                var claim = new CMCS.Models.Claim
                {
                    UserId = _currentUserId,
                    ModuleName = txtModule.Text.Trim(),
                    ModuleCode = txtCode.Text.Trim(),
                    HoursWorked = hours,
                    HourlyRate = rate,
                    TotalAmount = total,
                    Notes = txtNotes.Text.Trim(),
                    FilePath = _uploadFilePath,
                    status = "Pending",
                    DateSubmitted = DateTime.Now,
                };
                //Calling the database
                using (var db = new CMCS.Data.ApplicationDbContext())
                {
                    db.Claims.Add(claim);
                    db.SaveChanges(); //Save
                }

                new CMCS.Dialogs.SuccessDialog("Claim submitted successfully.").ShowDialog();
                this.Close(); //Close the form
            }
            catch (Exception ex)
            {
                new CMCS.Dialogs.ErrorDialog("Error submitting claim: " + ex.InnerException?.Message).ShowDialog(); //Full error message
            }
        }
    }
}
