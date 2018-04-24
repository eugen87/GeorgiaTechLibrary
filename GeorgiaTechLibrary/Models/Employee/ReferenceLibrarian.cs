using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employee
{
    public class ReferenceLibrarian : Employee
    {
        public ReferenceLibrarian()
        {
            this.Title = (short)EmployeeEnum.ReferanceLibrarian;
        }
    }
}
