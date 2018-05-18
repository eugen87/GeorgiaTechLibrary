using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public abstract class Employee : Person
    {
        public Employee(Person person)
        {
            this.Address = person.Address;
            this.Email = person.Email;
            this.Name = person.Name;
            this.Password = person.Password;
            this.Phone = person.Phone;
            this.PictureId = person.PictureId;
            this.Ssn = person.Ssn;
        }
        public abstract string Title { get; }
        public override long Ssn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string PictureId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Phone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
