using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class Student : Member
    {
        public Student(PersonAPI person) : base(person)
        {
            this.LoanRuleId = 1; // hard coded --- to be remove from here
        }

        public Student()
        {
        }
    }
}
