using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Models
{
    class Claim
    {
        public int ClaimId { get; set; }

        public int UserId { get; set; }
        //public int LectureId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }  
        public double TotalAmount { get; set; }
        public string Notes { get; set; }
        public string FilePath { get; set; }
        public string status { get; set; }
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        //Navigation property
        public User User {  get; set; }
    }
}
