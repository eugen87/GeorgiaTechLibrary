using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class DepartmentLibrarian : Employee
    {
        public DepartmentLibrarian(Person person) : base(person)
        {
        }

        public override string Title => "Department Librarian";
    }
}
