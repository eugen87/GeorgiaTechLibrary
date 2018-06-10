using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class ReferenceLibrarian : Employee
    {
        public ReferenceLibrarian()
        {
                
        }
        public ReferenceLibrarian(PersonAPI person) : base(person)
        {
        }

        public override string Title => "Reference Librarian";
    }
}
