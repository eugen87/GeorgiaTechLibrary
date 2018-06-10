using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class Teacher : Member
    {
        public Teacher(PersonAPI person) : base(person)
        {
            this.LoanRuleId = 2;
        }
        public Teacher()
        {
        }
    }
}
