using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeorgiaTechLibrary.Models.Items;

namespace GeorgiaTechLibrary.Models.Factories.Items
{
    public class MapFactory : ItemFactory
    {
        private ItemInfo _itemInfo;

        public MapFactory(ItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
        }

        public override Item GetItem() => new Map(_itemInfo);
    }
}
