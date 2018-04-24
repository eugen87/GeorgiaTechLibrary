using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class Teacher : Member
    {
        public Teacher()
        {
            this.LoanRule = (short)MemberEnum.Teacher;
        }
    }
}
