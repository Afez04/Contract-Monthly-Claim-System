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
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace CMCS.Dialogs
{
    /// <summary>
    /// Interaction logic for SuccessDialog.xaml
    /// </summary>
    public partial class SuccessDialog : Window
    {
        private bool _isClossing = false;
        public SuccessDialog(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
    }
}
