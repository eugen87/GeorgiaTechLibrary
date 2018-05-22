using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibrary.Models.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.APIModel
{
    public class LoanAPI
    {
        public Guid ItemId { get; set; }
        public long MemberSsn { get; set; }
    }
}
