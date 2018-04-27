using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public abstract class Member:Person
    {
        public DateTime CardExpirationDate { get; set; }
        public LoanRule LoanRule { get; set; }
    }
}
