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
            this.LoanRule = new LoanRule(1,5,7,21); // hard coded --- to be remove from here
        }

        public Student()
        {
                
        }
    }
}
