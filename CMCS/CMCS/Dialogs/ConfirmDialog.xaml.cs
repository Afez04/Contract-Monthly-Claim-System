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

namespace CMCS.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : Window
    {
        public bool IsConfirmed { get; private set; } = false;
        public ConfirmDialog(String message)
        {
            InitializeComponent();
            MessageText.Text = message; 
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            this.Close();
        }

        private void No_Click (object sender, RoutedEventArgs e)
        {
            IsConfirmed=  false;
            this.Close();
        }
    }
}
