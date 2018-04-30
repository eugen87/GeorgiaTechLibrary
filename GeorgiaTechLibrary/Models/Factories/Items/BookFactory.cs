using GeorgiaTechLibrary.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Factories.Items
{
    public class BookFactory : ItemFactory
    {
        private ItemInfo _itemInfo;
        private string _isbn;

        public BookFactory(ItemInfo itemInfo, string ISBN)
        {
            _itemInfo = itemInfo;
            _isbn = ISBN;
        }

        public override Item GetItem() => new Book(_itemInfo, _isbn);
    }
}
