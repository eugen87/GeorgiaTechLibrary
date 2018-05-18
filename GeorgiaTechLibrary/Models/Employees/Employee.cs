using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public abstract class Employee : Person
    {
        public abstract string Title { get; }
    }
}
