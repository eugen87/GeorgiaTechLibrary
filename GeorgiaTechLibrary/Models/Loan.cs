using GeorgiaTechLibrary.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace GeorgiaTechLibrary.Models
{
    public class Loan
    {
        public Item Item { get; set; }
        public Member Member { get; set; }

    }
}
