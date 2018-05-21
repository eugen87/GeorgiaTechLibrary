using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models
{
    public abstract class Person
    {
        private long _ssn;
        private string _name;
        private string _address;
        private string _picId;
        private string _phone;
        private string _email;
        private string _password;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(9)]
        [MinLength(9)]
        public long Ssn
        {
            get { return _ssn; }
            set { _ssn = (value.ToString().Length == 9) ? value : 111111111; }
        }
        [Required]
        public string Name { get => _name; set => _name = value; }
        [Required]
        public string Address { get => _address; set => _address = value; }
        public string PictureId { get => _picId; set => _picId = value; }        
        [Required]
        public string Email
        {
            get => _email;
            //set
            //{
            //    var addr = new System.Net.Mail.MailAddress(value);
            //    if (addr.Address == value)
            //    { _email = value; }
            //    else
            //    {
            //        _email = "";
            //    }
            //}
            set
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(value);
                    _email = addr.Address;
                }
                catch
                {
                    _email = "";
                }
            }
        }
        public string Phone { get => _phone; set => _phone = value; }
        [Required]
        public string Password { get => _password; set => _password = value; }
    }
}
