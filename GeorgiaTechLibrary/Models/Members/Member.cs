using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public abstract class Member:Person
    {
        [Required]
        public DateTime CardExpirationDate { get; set; }
        [Required]
        public LoanRule LoanRule { get; set; }
    }
}
