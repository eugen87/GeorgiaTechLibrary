using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Commit();
    }
}
