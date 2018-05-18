using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Factories.Employees
{
    public class EmployeeFactory<T> where T : Employee, new()
    {
        public static Employee Get(Person person){
            return new T();
        }
    }
}
