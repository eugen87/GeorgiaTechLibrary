using GeorgiaTechLibrary.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Factories.Items
{
    public abstract class ItemFactory
    {
        public abstract Item GetItem();
    }
}
