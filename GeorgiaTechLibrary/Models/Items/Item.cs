using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Items
{
    public abstract class Item
    {
        public abstract Guid Id { get; }
        public abstract ItemInfo ItemInfo { get; set; }
        public abstract RentStatus RentStatus { get; set; }
        public abstract ItemStatus ItemStatus { get; set; }
        public abstract ItemCondition ItemCondition { get; set; }
    }

    public enum RentStatus
    {
        AVAILABLE, UNAVAILABLE
    }

    public enum ItemStatus
    {
        RENTABLE, NONRENTABLE, WISHLIST
    }

    public enum ItemCondition
    {
        OK, DAMAGED, UNUSABLE, LOST
    }
}
