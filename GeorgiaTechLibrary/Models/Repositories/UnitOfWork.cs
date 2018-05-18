using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

        }
    }
}
