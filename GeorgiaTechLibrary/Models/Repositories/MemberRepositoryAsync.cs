using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Members;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeorgiaTechLibraryAPI.Models.Repositories
{
    public class MemberRepositoryAsync<T> : RepositoryAsync<Member> where T : Member
    {
        private readonly LibraryContext _context;

        public MemberRepositoryAsync(DbContext dbContext) : base(dbContext)
        {
            _context = (LibraryContext)dbContext;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<IEnumerable<Member>> GetAsync() => _context.Members.Include(l => l.LoanRule).AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<IEnumerable<Member>> GetAsync(Expression<Func<Member, bool>> predicate) => _context.Members.Include(l => l.LoanRule).Where(predicate).AsEnumerable();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

    }
}
