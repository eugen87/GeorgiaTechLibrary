﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class ChiefLibrarian : Employee
    {
        public ChiefLibrarian()
        {
            this.Title = (short) EmployeeEnum.ChiefLibrarian;
        }
    }
}