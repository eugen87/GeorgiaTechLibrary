using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Employees;
using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Factories.Employees
{
    public static class EmployeeFactory
    {
        public static Employee Get(PersonAPI person, EmployeeEnum empType){
            switch (empType)
            {
                case EmployeeEnum.AssistentLibrarian:
                    return new AssistantLibrarian(person);
                case EmployeeEnum.CheckOutStaff:
                    return new CheckOutStaff(person);
                case EmployeeEnum.ChiefLibrarian:
                    return new ChiefLibrarian(person);
                case EmployeeEnum.DepartmentLibrarian:
                    return new DepartmentLibrarian(person);
                case EmployeeEnum.ReferenceLibrarian:
                    return new ReferenceLibrarian(person);
            }
            return null;
        }
    }
}
