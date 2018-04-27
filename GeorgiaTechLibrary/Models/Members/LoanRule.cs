using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class LoanRule
    {
        public int Id { get; set; }
        public short LoanTime { get; set; } // loan max period in days
        public short GracePeriod { get; set; } // grace period in days
        public short BookLimit { get; set; }    // max number of open loans at the same time

    }
}
