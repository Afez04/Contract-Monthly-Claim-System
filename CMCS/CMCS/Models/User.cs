using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Models
{
    class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }

        public string Username {  get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public double HourlyRate { get; set; }
        public string Role { get; set; }

        //Navigation Property 
        public ICollection<Claim> Claims { get; set; } 
    }
}
