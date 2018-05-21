using GeorgiaTechLibrary.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.APIModel
{
    public class ItemAPI
    {
        public  Guid Id { get; set; }
        public  ItemInfo ItemInfo { get; set; }
        public  RentStatus RentStatus { get; set; }
        public  ItemStatus ItemStatus { get; set; }
        public  ItemCondition ItemCondition { get; set; }
    }
}
