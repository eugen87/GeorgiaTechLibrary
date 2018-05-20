using GeorgiaTechLibraryAPI.Models.APIModel;
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
        public AssistantLibrarian(PersonAPI person) : base(person)
        {
        }

        public override string Title => "Assistant Librarian";

        
    }
}
