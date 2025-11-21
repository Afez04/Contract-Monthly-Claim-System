using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMCS;
namespace CMCS.Helper
{
    public class ClaimCalculator
    {
        public double? CalculateTotal(String hoursText, string rateText)
        {
            if (!double.TryParse(hoursText, out double hours) || !double.TryParse(rateText, out double rate))   
                return null;

            return hours * rate;
        }
    }
}
