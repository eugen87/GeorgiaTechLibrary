using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class CheckOutStaff : Employee
    {
        public CheckOutStaff(Person person) : base(person)
        {
        }

        public override string Title => "CheckOut Staff";
    }
}
