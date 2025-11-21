using CMCS.Data;
using CMCS.Models;
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
    /// Interaction logic for MessagesContactPage.xaml
    /// </summary>
    public partial class MessagesContactPage : Window
    {
        private readonly int _currentUserId;
        private readonly string _currentRole;
        public MessagesContactPage(int userId, string role)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentRole = role;
            LoadContacts();
        }

        /*private void LoadContacts()
        {
            using (var db = new ApplicationDbContext())
            {
                List<User> contacts;

                if (_currentRole == "Lecturer")
                    contacts = db.Users.Where(u => u.Role == "Coordinator").ToList();
                else if (_currentRole == "Coordinator")
                    contacts = db.Users.Where(u => u.Role == "Lecturer" || u.Role == "Lecture").ToList();
                else
                    contacts = db.Users.Where(u => u.Role == "Coordinator").ToList();

                ContactList.ItemsSource = contacts;
            }
        }
       */

        /*private void LoadContacts()
        {
            using (var db = new ApplicationDbContext())
            {
                List<User> contacts;

                string role = _currentRole;

                if (role == "Coordinator")
                {
                    contacts = db.Users.Where(u => u.Role == "Lecturer").ToList();
                }
                else if (role == "Lecturer")
                {
                    contacts = db.Users.Where(u => u.Role == "Coordinator").ToList();
                }

                else if (role == "Manager")
                {
                    contacts = db.Users.Where(u => u.Role == "Coordinator" || u.Role == "Lecturer").ToList();
                }

                else
                {
                    contacts = new List<User>();
                }

                ContactList.ItemsSource = contacts;
            }
        }
        */
        private void LoadContacts()
        {
            List<User> contacts;
            using (var db = new ApplicationDbContext())
            {
                
                 contacts = db.Users.Where(u => u.UserId != _currentUserId).ToList();
            }

            ContactList.ItemsSource = contacts;
        }
       /* private void LoadContacts()
        {
            using (var db = new ApplicationDbContext())
            {
                var contacts = new List<object>();

                if (_currentRole == "Coordinator")
                {
                    contacts = db.Users.Where(u => u.Role == "Lecturer").Select(u => new
                    {
                        ContactList = u.UserId,
                        Con
                    })
                }
            }
        }
       */
        private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContactList.SelectedItem is User selectdUser)
            {
                var chatWindow = new ChatWindow(_currentUserId, selectdUser.UserId);
                chatWindow.Show();
            }
        }
    }
}
