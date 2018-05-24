using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Repositories
{
    public class ItemRepositoryAsync<T> : RepositoryAsync<Item> where T : Item
    {
        private readonly LibraryContext _context;

        public ItemRepositoryAsync(DbContext dbContext) : base(dbContext)
        {
            _context = (LibraryContext)dbContext;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<IEnumerable<Item>> GetAsync() => _context.Items.Include(i => i.ItemInfo).AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<IEnumerable<Item>> GetAsync(Expression<Func<Item, bool>> predicate) => _context.Items.Include(i => i.ItemInfo).Where(predicate).AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public async override Task UpdateAsync(Item entity)
        {
            _context.ItemInfos.Update(entity.ItemInfo);
            await _context.SaveChangesAsync();
        }
    }
}
