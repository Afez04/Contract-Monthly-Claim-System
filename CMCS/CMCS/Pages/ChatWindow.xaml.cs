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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly int _senderId;
        private readonly int _receiverId;
        public ChatWindow(int senderId, int receivedId)
        {
            InitializeComponent();
            _senderId = senderId;
            _receiverId = receivedId;
            LoadMessages();
        }

        private void LoadMessages()
        {
            using (var db = new ApplicationDbContext())
            {
                var sender = db.Users.Find(_senderId);
                var receiver = db.Users.Find(_receiverId);
                ChatTitle.Text = $"Chat with{receiver.FullName}";

                var dbmessages = db.Messages.Where(m => (m.SenderId == _senderId && m.RecieverId == _receiverId) || (m.SenderId == _receiverId && m.RecieverId == _senderId))
                    .OrderBy(m => m.DateSent).ToList();

                var messages = dbmessages.Select(m => new
                {
                    m.Content,
                    Alignment = m.SenderId == _senderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                    Backgroud = new SolidColorBrush(m.SenderId == _senderId ? Color.FromRgb(38, 103, 110) : Color.FromRgb(150, 192, 189)
                    )
                }).ToList();
                    

                MessagePanel.ItemsSource = messages;
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            var messageText = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(messageText) )
            {
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var senderUser = db.Users.Find(_senderId);
                var receiverUser = db.Users.Find(_receiverId);

                var message = new Message
                {
                    SenderId = _senderId,
                    RecieverId = _receiverId,
                    SenderName = senderUser.FullName,
                    RecieverName = receiverUser.FullName,
                    Content = messageText
                };
                db.Messages.Add(message);
                db.SaveChanges();
            }

            txtMessage.Clear();
            LoadMessages();
        }
    }
}
