using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employee
{
    public abstract class Employee : Person
    {
        public short Title { get; set; }
    }
}
