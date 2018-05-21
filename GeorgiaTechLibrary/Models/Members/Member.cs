using GeorgiaTechLibraryAPI.Models.APIModel;
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

        private long _ssn;
        private string _name;
        private string _address;
        private string _picId;
        private string _phone;
        private string _email;
        private string _password;

        public Member(PersonAPI person)
        {
            _address = person.Address;
            _email = person.Email;
            _name = person.Name;
            _password = person.Password;
            _phone = person.Phone;
            _picId = person.PictureId;
            _ssn = person.Ssn;
            CardExpirationDate = DateTime.Now.AddYears(4);
        }
        public Member()
        {
               
        }      

    }
}
