﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public class AssistantLibrarian : Employee
    {
        public AssistantLibrarian()
        {
            this.Title = (short)EmployeeEnum.AssistentLibrarian;
        }
    }
}