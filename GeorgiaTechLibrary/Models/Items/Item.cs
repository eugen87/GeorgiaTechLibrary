using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Items
{
    public abstract class Item
    {
        [Key]
        public abstract Guid Id { get; set; }
        [Required]
        public abstract ItemInfo ItemInfo { get; set; }
        [Required]
        public abstract RentStatus RentStatus { get; set; }
        [Required]
        public abstract ItemStatus ItemStatus { get; set; }
        [Required]
        public abstract ItemCondition ItemCondition { get; set; }

        public Item()
        {
        }
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
