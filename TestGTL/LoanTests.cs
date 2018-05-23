using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Factories.Items;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibrary.Models.Members;
using GeorgiaTechLibraryAPI.Controllers;
using GeorgiaTechLibraryAPI.Models.APIModel;
using GeorgiaTechLibraryAPI.Models.Factories.Members;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace TestGTL
{
    public class LoanTests
    {
        private readonly ITestOutputHelper output;
        public LoanTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact(DisplayName = "Add Loan")]
        public async void Add_Loan()
        {
            var memberSsn = 112233445;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = context.Items.FirstOrDefault().Id;
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);
                var loans = await controller.GetLoans();
                var loan = loans.FirstOrDefault();

                output.WriteLine(loan.Member.Email);
                output.WriteLine(loan.Item.RentStatus.ToString());

                Assert.True(loan is Loan);
                Assert.Equal(RentStatus.UNAVAILABLE, loan.Item.RentStatus);
            }
        }

        private Guid AddItem(LibraryContext context)
        {
            var item = ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" });
            context.Items.Add(item);
            context.SaveChanges();
            return context.Items.Last().Id;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Add_Loan_Over_Limit(int books)
        {
            var memberSsn = 112233445; // already has 4 loans

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                for (int i = 0; i < books; i++)
                {
                    var itemId = AddItem(context);
                    LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                    await controller.PostLoan(loanAPI);
                }

                var loans = context.Members.FirstOrDefault(m => m.Ssn == memberSsn).Loans;
                Assert.Equal(5, loans.Count);
            }
        }

        [Fact(DisplayName = "Don't Add Unavailable Loan")]
        public async void Add_Unavailable_Loan()
        {
            var memberSsn = 112233445;
            var memberSsn2 = 556677889;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = context.Items.FirstOrDefault().Id;
                var loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);


                var loanAPI2 = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn2 };
                var result2 = await controller.PostLoan(loanAPI2);

                var loans = await controller.GetLoans();
                var loanz = loans.Where(l => l.Item.Id == itemId && l.IsReturned == false);

                Assert.True(loanz.Count() == 1);
            }
        }

        [Theory]
        [InlineData(ItemCondition.OK)]
        [InlineData(ItemCondition.DAMAGED)]
        [InlineData(ItemCondition.LOST)]
        [InlineData(ItemCondition.UNUSABLE)]
        public async void Return_Loan(ItemCondition condition)
        {
            var memberSsn = 112233445;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = context.Items.FirstOrDefault().Id;
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                await controller.PostLoan(loanAPI);

                await controller.ReturnLoan(loanAPI, (int)condition);

                var loans = await controller.GetLoans();
                var loanz = loans.Where(l => l.Item.Id == itemId && l.Member.Ssn == memberSsn && l.IsReturned == false);

                output.WriteLine(loanz.Count().ToString());
                Assert.True(loanz.Count() == 0);
            }
        }

        [Theory]
        [InlineData(ItemStatus.RENTABLE, ItemCondition.OK)]
        [InlineData(ItemStatus.RENTABLE, ItemCondition.DAMAGED)]
        [InlineData(ItemStatus.NONRENTABLE, ItemCondition.LOST)]
        [InlineData(ItemStatus.NONRENTABLE, ItemCondition.UNUSABLE)]
        public async void Update_ItemStatus(ItemStatus status, ItemCondition condition)
        {
            var memberSsn = 112233445;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = context.Items.FirstOrDefault().Id;
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                await controller.PostLoan(loanAPI);

                await controller.ReturnLoan(loanAPI, (int)condition);

                var loans = await controller.GetLoans();
                var loan = loans.Where(l => l.Item.Id == itemId && l.Member.Ssn == memberSsn).FirstOrDefault();

                Assert.Equal(status, loan.Item.ItemStatus);
            }
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

            context.LoanRules.AddRange(new LoanRule() { Id = 1, LoanTime = 5, BookLimit = 5, GracePeriod = 20 },
                new LoanRule() { Id = 2, LoanTime = 5, BookLimit = 5, GracePeriod = 20 });

            context.Members.AddRange(MemberFactory.Get(new PersonAPI() { Address = "Address 1", Email = "student1@test.com", Name = "Student 1", Password = "std1", Phone = "11111111", PictureId = "std1", Ssn = 112233445 },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI() { Address = "Address 2", Email = "student2@test.com", Name = "Student 2", Password = "std2", Phone = "22222222", PictureId = "std2", Ssn = 223344556 },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI() { Address = "Address 4", Email = "teacher1@test.com", Name = "Teacher 1", Password = "tch1", Phone = "44444444", PictureId = "tch1", Ssn = 445566778 },
            MemberEnum.Teacher),
            MemberFactory.Get(new PersonAPI() { Address = "Address 5", Email = "teacher2@test.com", Name = "Teacher 2", Password = "tch2", Phone = "55555555", PictureId = "tch2", Ssn = 556677889 },
            MemberEnum.Teacher));

            context.SaveChanges();

            var items = context.Items.ToList();
            context.Loans.AddRange(new Loan(items[4].Id, 112233445), new Loan(items[1].Id, 112233445), new Loan(items[2].Id, 112233445), new Loan(items[3].Id, 112233445));
            context.SaveChanges();
            // Member 112233445 has 4 loans
            return context;
        }
    }
}
