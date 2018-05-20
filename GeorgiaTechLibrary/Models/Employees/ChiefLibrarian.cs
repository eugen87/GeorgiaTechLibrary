using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class ChiefLibrarian : Employee
    {
        public ChiefLibrarian()
        {

        }
        public ChiefLibrarian(Person person) : base(person)
        {
        }

        public override string Title => "Chief Librarian";
    }
}
