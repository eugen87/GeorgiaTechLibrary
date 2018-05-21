using GeorgiaTechLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace TestGTL
{
    public class ItemTests
    {
        private readonly ITestOutputHelper output;
        public ItemTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private LibraryContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new LibraryContext(options);

            context.Items.AddRange();

            context.SaveChanges();

            return context;
        }
    }
}
