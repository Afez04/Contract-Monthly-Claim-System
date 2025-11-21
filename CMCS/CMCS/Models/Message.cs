using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Models
{
     class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int RecieverId {  get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public string Content {  get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;

        public int? ClaimId { get; set; }
        public virtual Claim Claim { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Reciever { get; set; }
    }
}
