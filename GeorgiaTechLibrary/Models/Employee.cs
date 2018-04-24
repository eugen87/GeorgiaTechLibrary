using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models
{
    public abstract class Employee : Person
    {
        public string Title { get; set; }
    }
}
