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
        public static Employee Get(PersonAPI person, EmployeeEnum empType)
        {
            switch (empType)
            {
                case EmployeeEnum.AssistentLibrarian:
                   var assistantLibrarian = new AssistantLibrarian(person);
                    return (assistantLibrarian.IsValid()) ? assistantLibrarian : null;
                case EmployeeEnum.CheckOutStaff:
                    var checkOutStaff = new CheckOutStaff(person);
                    return (checkOutStaff.IsValid()) ? checkOutStaff : null;
                case EmployeeEnum.ChiefLibrarian:
                    var chiefLibrarianemp = new ChiefLibrarian(person);
                    return (chiefLibrarianemp.IsValid()) ? chiefLibrarianemp : null;
                case EmployeeEnum.DepartmentLibrarian:
                    var departmentLibrarian = new DepartmentLibrarian(person);
                    return (departmentLibrarian.IsValid()) ? departmentLibrarian : null;
                case EmployeeEnum.ReferenceLibrarian:
                    var referenceLibrarian = new ReferenceLibrarian(person);
                    return (referenceLibrarian.IsValid()) ? referenceLibrarian : null;
            }
            return null;
        }
    }
}
