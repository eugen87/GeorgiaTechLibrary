using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class Student : Member
    {
        public Student(Person person) : base(person)
        {
            this.LoanRuleId = 1;
        }

        public override long Ssn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string PictureId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Phone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
