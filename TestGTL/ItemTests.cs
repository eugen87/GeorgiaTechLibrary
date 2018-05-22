using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Factories.Items;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibraryAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
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

            context.Items.AddRange(ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }, "978-3-16-148410-0"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }, "978-3-16-148410-1"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }, "978-3-16-148410-2"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }));

            context.SaveChanges();

            return context;
        }

        [Fact(DisplayName = "Factory Creates Book")]
        public void Factory_Create_Book()
        {
            ItemInfo info = new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" };
            string isbn = "978-3-16-148421-1";

            Book book = (Book)ItemFactory.Get(info, isbn);

            Assert.True(book is Book);
            Assert.Equal(isbn, book.ISBN);
        }

        [Fact(DisplayName = "Factory Creates Map")]
        public void Factory_Create_Map()
        {
            ItemInfo info = new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" };

            Item map = ItemFactory.Get(info);

            Assert.True(map is Map);
            Assert.Equal(info.Author, map.ItemInfo.Author);
        }

        [Fact(DisplayName = "Get all items")]
        public async void Get_All_Items()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var result = await controller.GetItems();
                output.WriteLine(result.AsEnumerable().FirstOrDefault().ItemInfo.Author);
                Assert.True(result.Count() != 0);
            }
        }

        [Fact(DisplayName = ("Add book"))]
        public async void Add_Book()
        {
            ItemInfo info = new ItemInfo()
            {
                Author = "Test Author 1",
                Description = "A very good testing book 1",
                Title = "The best book 1"
            };

            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var result = await controller.PostItem(info, "978-3-16-148410-6");
                var itms = await controller.GetItems();
                var itm = itms.Where(i => i.ItemInfo.Title == info.Title).FirstOrDefault();

                Assert.True(itm is Book);
            }
        }

        [Fact(DisplayName = ("Add map"))]
        public async void Add_Map()
        {
            ItemInfo info = new ItemInfo()
            {
                Author = "Test Author 1",
                Description = "A very good testing book 1",
                Title = "The best book 1"
            };

            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var result = await controller.PostItem(info, "");
                var itms = await controller.GetItems();
                var itm = itms.Where(i => i.ItemInfo.Title == info.Title).FirstOrDefault();

                Assert.True(itm is Map);
            }
        }
    }
}
