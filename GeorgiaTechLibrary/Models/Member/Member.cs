﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Member
{
    public abstract class Member:Person
    {
        public DateTime CardExpirationDate { get; set; }
        public int LoanRule { get; set; }
    }
}
