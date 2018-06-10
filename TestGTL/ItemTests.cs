using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Factories.Items;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibraryAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
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

        [Fact(DisplayName = "Factory Creates Map")]
        public void Factory_Create_Map()
        {
            ItemInfo info = new ItemInfo() { Author = "Test Author", Description = "A very good testing map", Title = "The best map" };

            Item map = ItemFactory.Get(info);

            Assert.True(map is Map);
            Assert.Equal(info.Author, map.ItemInfo.Author);
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

        [Fact(DisplayName = "Delete Item")]
        public void Delete_Employee()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var item = context.Items.First();

                var result = controller.DeleteItem(item.Id);

                var del = context.Items.FirstOrDefault(i => i.Id == item.Id);
                Assert.False(del != null);
            }
        }

        [Fact(DisplayName = "Get Book")]
        public async void Get_Book()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var book = context.Items.FirstOrDefault(i => i is Book);
                var result = await controller.GetItem(book.Id).ToAsyncEnumerable().FirstOrDefault();

                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact(DisplayName = "Get Map")]
        public async void Get_Map()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var map = context.Items.FirstOrDefault(i => i is Map);
                var result = await controller.GetItem(map.Id).ToAsyncEnumerable().FirstOrDefault();

                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact(DisplayName = "Don't get Item with wrong GUID")]
        public async void Get_Item_Wrong_GUID()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var mapId = Guid.NewGuid();
                var result = await controller.GetItem(mapId).ToAsyncEnumerable().FirstOrDefault();

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact(DisplayName = "Don't update Item with wrong GUID")]
        public async void Update_Item_Wrong_GUID()
        {
            using (var context = GetContextWithData())
            using (var controller = new ItemsController(context))
            {
                var guid = Guid.NewGuid();
                ItemInfo info = new ItemInfo()
                {
                    Author = "Test Author 1",
                    Description = "A very good testing book 1",
                    Title = "The best book adf1"
                };

                var result = await controller.PutItem(guid, info);
                Assert.IsType<NotFoundResult>(result);
            }
        }

        private LibraryContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new LibraryContext(options);

            context.Items.AddRange(ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" }, "978-3-16-148410-0"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book for Children", Title = "The best book 4 kids" }, "978-3-16-148410-1"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book of God", Title = "The best book" }, "978-3-16-148410-2"),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing map", Title = "The best MAP" }),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing map of CHINA!", Title = "The best map of CHINA!" }),
                ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good map", Title = "The best map" }));

            context.SaveChanges();

            return context;
        }
    }
}
