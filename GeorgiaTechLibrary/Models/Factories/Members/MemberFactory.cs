using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Members;
using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Factories.Members
{
    public class MemberFactory { 
        public static Member Get(MemberAPI memberAPI, MemberEnum member)
        {
            switch (member)
            {
                case MemberEnum.Student:
                    return new Student(memberAPI);
                case MemberEnum.Teacher:
                    return new Teacher(memberAPI);
                default:
                    return null; 
            }
        }
    }
}
