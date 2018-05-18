using GeorgiaTechLibrary.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Factories.Employees
{
    public class EmployeeFactory
    {
        public static Employee Get(EmployeeEnum employeeEnum){
            switch (employeeEnum)
            {
                case EmployeeEnum.ChiefLibrarian:
                    return new ChiefLibrarian();
                case EmployeeEnum.DepartmentLibrarian:
                    return new DepartmentLibrarian();
                case EmployeeEnum.ReferenceLibrarian:
                    return new ReferenceLibrarian();
                case EmployeeEnum.CheckOutStaff:
                    return new CheckOutStaff();
                case EmployeeEnum.AssistentLibrarian:
                    return new AssistantLibrarian();
                default:
                    return new DepartmentLibrarian();
            }
        }
    }
}
