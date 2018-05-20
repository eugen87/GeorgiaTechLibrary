using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class AssistantLibrarian : Employee
    {
        public AssistantLibrarian()
        {

        }
        public AssistantLibrarian(Person person) : base(person)
        {
        }

        public override string Title => "Assistant Librarian";

        
    }
}
