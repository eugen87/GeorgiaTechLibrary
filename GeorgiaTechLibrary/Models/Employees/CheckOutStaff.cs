using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class CheckOutStaff : Employee
    {
        public CheckOutStaff()
        {

        }
        public CheckOutStaff(PersonAPI person) : base(person)
        {
        }

        public override string Title => "CheckOut Staff";
    }
}
