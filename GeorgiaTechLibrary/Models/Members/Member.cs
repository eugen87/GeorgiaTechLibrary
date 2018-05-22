using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public abstract class Member : Person
    {
        [Required]
        public DateTime CardExpirationDate { get; set; }
        public virtual LoanRule LoanRule { get; set; }
        public int? LoanRuleId { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }

        public Member(PersonAPI person)
        {
            this.Address = person.Address;
            this.Email = person.Email;
            this.Name = person.Name;
            this.Password = person.Password;
            this.Phone = person.Phone;
            this.PictureId = person.PictureId;
            this.Ssn = person.Ssn;
            CardExpirationDate = DateTime.Now.AddYears(4);
        }

        public Member()
        {
        }
    }
}
