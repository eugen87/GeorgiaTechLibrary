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
    public class MemberRepositoryAsync<T> : RepositoryAsync<Member> where T: Member
    {
        private readonly LibraryContext _context;

        public MemberRepositoryAsync(DbContext dbContext) : base(dbContext)
        {
            _context = (LibraryContext)dbContext;
        }

        public override async Task<IEnumerable<Member>> GetAsync() => _context.Members.Include(l => l.LoanRule).AsEnumerable();
    }
}
