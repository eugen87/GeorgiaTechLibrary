using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employee
{
    public class DepartmentLibrarian : Employee
    {
        public DepartmentLibrarian()
        {
            this.Title = (short)EmployeeEnum.DepartmentLibrarian;
        }
    }
}
