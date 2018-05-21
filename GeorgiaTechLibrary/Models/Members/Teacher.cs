using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Members
{
    public class Teacher : Member
    {
        public Teacher(MemberAPI memberAPI) : base(memberAPI)
        {
            this.LoanRule = new LoanRule(2);
        }

        public Teacher()
        {
                
        }
       
    }
}
