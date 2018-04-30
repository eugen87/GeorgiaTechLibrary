using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Items
{

    public class Map : Item
    {
        private readonly Guid _id;
        private ItemInfo _itemInfo;
        private RentStatus _rentStatus;
        private ItemStatus _itemStatus;
        private ItemCondition _itemCondition;

        public Map(ItemInfo itemInfo)
        {
            _id = new Guid();
            _itemInfo = itemInfo;
            _rentStatus = RentStatus.AVAILABLE;
            _itemStatus = ItemStatus.RENTABLE;
            _itemCondition = ItemCondition.OK;
        }

        public override Guid Id { get => _id; }
        public override ItemInfo ItemInfo { get => _itemInfo; set => _itemInfo = value; }
        public override RentStatus RentStatus { get => _rentStatus; set => _rentStatus = value; }
        public override ItemStatus ItemStatus { get => _itemStatus; set => _itemStatus = value; }
        public override ItemCondition ItemCondition { get => _itemCondition; set => _itemCondition = value; }
    }


}
