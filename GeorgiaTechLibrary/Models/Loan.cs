using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibrary.Models.Members;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace GeorgiaTechLibrary.Models
{
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanID { get; set; }
        [Required]
        public Item Item { get; set; }
        [Required]
        public Member Member { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsReturned { get; set; }

    }
}
