using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Items
{
    public class Book : Item
    {

        // private readonly Guid _id;   //for entity framework will not work. Entity Framework ignores read-only properties and this is a primary key
        private Guid _id;
        private ItemInfo _itemInfo;
        private RentStatus _rentStatus;
        private ItemStatus _itemStatus;
        private ItemCondition _itemCondition;
        [Required]
        private string _isbn;

        public Book(ItemInfo itemInfo, string ISBN)
        {
            _id = new Guid();
            _itemInfo = itemInfo;
            _isbn = ISBN;
            _rentStatus = RentStatus.AVAILABLE;
            _itemStatus = ItemStatus.RENTABLE;
            _itemCondition = ItemCondition.OK;
        }

        public string ISBN { get => _isbn; set => _isbn = value; }
        public override Guid Id { get => _id; set => _id = value; }
        public override ItemInfo ItemInfo { get => _itemInfo; set => _itemInfo = value; }
        public override RentStatus RentStatus { get => _rentStatus; set => _rentStatus = value; }
        public override ItemStatus ItemStatus { get => _itemStatus; set => _itemStatus = value; }
        public override ItemCondition ItemCondition { get => _itemCondition; set => _itemCondition = value; }
    }
}
