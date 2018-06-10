using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Members;
using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Factories.Members
{
    public class MemberFactory
    {
        public static Member Get(PersonAPI person, MemberEnum member)
        {
            switch (member)
            {
                case MemberEnum.Student:
                    var student = new Student(person);
                    if (student.IsValid())
                        return student;
                    return null;
                case MemberEnum.Teacher:
                    var teacher = new Teacher(person);
                    if (teacher.IsValid())
                        return teacher;
                    return null;
                default:
                    return null;
            }
        }
    }
}
