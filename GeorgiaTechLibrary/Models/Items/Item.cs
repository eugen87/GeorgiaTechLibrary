using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Items
{
    public abstract class Item
    {
        public Guid Id { get; set; }
        public ItemInfo ItemInfo { get; set; }
        public RentStatus RentStatus { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public ItemCondition ItemCondition { get; set; }
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
