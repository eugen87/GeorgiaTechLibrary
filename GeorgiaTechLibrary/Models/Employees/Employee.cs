using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public abstract class Employee : Person
    {
        private long _ssn;
        private string _name;
        private string _address;
        private string _picId;
        private string _phone;
        private string _email;
        private string _password;

        public Employee(Person person)
        {
            _address = person.Address;
            _email = person.Email;
            _name = person.Name;
            _password = person.Password;
            _phone = person.Phone;
            _picId = person.PictureId;
            _ssn = person.Ssn;
        }
        public Employee()
        {

        }
        public abstract string Title { get; }
        public override long Ssn { get => _ssn; set => _ssn = value; }
        public override string Name { get => _name; set => _name = value; }
        public override string Address { get => _address; set => _address = value; }
        public override string PictureId { get => _picId; set => _picId = value; }
        public override string Email { get => _email; set => _email = value; }
        public override string Phone { get => _phone; set => _phone = value; }
        public override string Password { get => _password; set => _password = value; }
    }
}
