using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Member
{
    public class Student : Member
    {
        public Student()
        {
            this.LoanRule = (short)MemberEnum.Student;
        }
    }
}
