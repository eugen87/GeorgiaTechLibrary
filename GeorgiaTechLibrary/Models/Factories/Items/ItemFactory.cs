using GeorgiaTechLibrary.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Factories.Items
{
    public static class ItemFactory
    {
        public static Item Get(ItemInfo info, string ISBN = "")
        {
            if (ISBN.Equals(""))
                return new Map(info);
            else
                return new Book(info, ISBN);
        }
    }
}
