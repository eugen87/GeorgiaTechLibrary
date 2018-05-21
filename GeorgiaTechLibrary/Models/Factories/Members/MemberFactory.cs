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
        public static Member Get(PersonAPI person, MemberEnum member)
        {
            switch (member)
            {
                case MemberEnum.Student:
                    return new Student(person);
                case MemberEnum.Teacher:
                    return new Teacher(person);
                default:
                    return null; 
            }
        }
    }
}
