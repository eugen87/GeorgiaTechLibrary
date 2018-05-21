using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class LoanRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public short LoanTime { get; set; } // loan max period in days
        [Required]
        public short GracePeriod { get; set; } // grace period in days
        [Required]
        public short BookLimit { get; set; }    // max number of open loans at the same time

        public LoanRule()
        {
        }

        public LoanRule(int id, short loanTime, short garcePeriod, short bookLimit)
        {
            this.Id = id;
            this.LoanTime = loanTime;
            this.GracePeriod = garcePeriod;
            this.BookLimit = bookLimit;

        }


        public LoanRule(int id)
        {
            this.Id = id;
        }
    }
}
