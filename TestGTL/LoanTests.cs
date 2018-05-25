using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Factories.Items;
using GeorgiaTechLibrary.Models.Items;
using GeorgiaTechLibrary.Models.Members;
using GeorgiaTechLibraryAPI.Controllers;
using GeorgiaTechLibraryAPI.Models.APIModel;
using GeorgiaTechLibraryAPI.Models.Factories.Members;
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
    public class LoanTests
    {
        private readonly ITestOutputHelper output;
        public LoanTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact(DisplayName = "Get all loans")]
        public async void Get_All_Loans()
        {
            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var result = await controller.GetLoans();
                output.WriteLine(result.AsEnumerable().FirstOrDefault().StartDate.ToLongDateString());
                Assert.True(result.Count() != 0);
            }
        }

        [Fact(DisplayName = "Get Loan")]
        public async void Get_Loan()
        {
            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var loanId = context.Loans.FirstOrDefault().LoanID;
                var result = await controller.GetLoan(loanId);

                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Fact(DisplayName = "Don't get Loan with wrong Id")]
        public async void Get_Loan_Wrong_Id()
        {
            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var loanId = 99999;
                var result = await controller.GetLoan(loanId);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact(DisplayName = "L1.1 Add Loan with ItemCondition OK")]
        public async void Add_Loan_OK()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);
                var loans = await controller.GetLoans();
                var loan = loans.Last();

                output.WriteLine(loan.Item.RentStatus.ToString());
                output.WriteLine(loan.Member.Ssn.ToString());

                Assert.IsType<CreatedAtActionResult>(result);
                Assert.True(loan is Loan);
                Assert.Equal(RentStatus.UNAVAILABLE, loan.Item.RentStatus);
            }
        }

        [Fact(DisplayName = "L1.2 Add Loan with ItemCondition DAMAGED")]
        public async void Add_Loan_DAMAGED()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.DAMAGED);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);
                var loans = await controller.GetLoans();
                var loan = loans.Last();

                output.WriteLine(loan.Item.RentStatus.ToString());
                output.WriteLine(loan.Member.Ssn.ToString());

                Assert.IsType<CreatedAtActionResult>(result);
                Assert.True(loan is Loan);
                Assert.Equal(RentStatus.UNAVAILABLE, loan.Item.RentStatus);
            }
        }

        [Fact(DisplayName = "L2.1 Don't add Loan with wrong RentStatus (UNAVAILABLE)")]
        public async void Add_Loan_OK_Wrong_RENTSTATUS()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.OK, RentStatus.UNAVAILABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L2.2 Don't add Loan with wrong RentStatus (UNAVAILABLE)")]
        public async void Add_Loan_DAMAGED_Wrong_RENTSTATUS()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.DAMAGED, RentStatus.UNAVAILABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L3.1 Don't add Loan with wrong ItemStatus (UNRENTABLE)")]
        public async void Add_Loan_OK_Wrong_ITEMSTATUS()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.OK, RentStatus.AVAILABLE, ItemStatus.NONRENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L3.2 Don't add Loan with wrong ItemStatus (UNRENTABLE)")]
        public async void Add_Loan_DAMAGED_Wrong_ITEMSTATUS()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.DAMAGED, RentStatus.AVAILABLE, ItemStatus.NONRENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L4.1 Don't add Loan with wrong ItemCondition (LOST)")]
        public async void Add_Loan_LOST_Wrong_ITEMCONDITION()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.LOST, RentStatus.AVAILABLE, ItemStatus.RENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L4.2 Don't add Loan with wrong ItemCondition (UNUSABLE)")]
        public async void Add_Loan_UNUSABLE_Wrong_ITEMCONDITION()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.UNUSABLE, RentStatus.AVAILABLE, ItemStatus.RENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L5.1 Don't add Loan which exceeds LoanLimit")]
        public async void Add_Loan_OK_Wrong_LOANLIMIT()
        {
            var memberSsn = 112233446; // already has 4 loans


            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId1 = AddItem(context);
                LoanAPI loanAPI1 = new LoanAPI() { ItemId = itemId1, MemberSsn = memberSsn };
                await controller.PostLoan(loanAPI1); // 5 loans LIMIT

                var itemId = AddItem(context, ItemCondition.OK, RentStatus.AVAILABLE, ItemStatus.RENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L5.2 Don't add Loan which exceeds LoanLimit")]
        public async void Add_Loan_DAMAGED_Wrong_LOANLIMIT()
        {
            var memberSsn = 112233446; // already has 4 loans


            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId1 = AddItem(context);
                LoanAPI loanAPI1 = new LoanAPI() { ItemId = itemId1, MemberSsn = memberSsn };
                await controller.PostLoan(loanAPI1); // 5 loans LIMIT

                var itemId = AddItem(context, ItemCondition.DAMAGED, RentStatus.AVAILABLE, ItemStatus.RENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L6.1 Don't add Loan with wrong MemberSSN")]
        public async void Add_Loan_OK_Wrong_MemberSSN()
        {
            var memberSsn = 123456789;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.OK, RentStatus.AVAILABLE, ItemStatus.NONRENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L6.2 Don't add Loan with wrong MemberSSN")]
        public async void Add_Loan_DAMAGED_Wrong_MemberSSN()
        {
            var memberSsn = 1122334466;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = AddItem(context, ItemCondition.DAMAGED, RentStatus.AVAILABLE, ItemStatus.NONRENTABLE);
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "L7.1 Don't add Loan with wrong ItemId")]
        public async void Add_Loan_OK_Wrong_ITEMID()
        {
            var memberSsn = 112233446;

            using (var context = GetContextWithData())
            using (var controller = new LoansController(context))
            {
                var itemId = new Guid();
                LoanAPI loanAPI = new LoanAPI() { ItemId = itemId, MemberSsn = memberSsn };
                var result = await controller.PostLoan(loanAPI);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Add_Loan_Over_Limit(int books)
        {
            var memberSsn = 112233446; // already has 4 loans

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
            var memberSsn = 112233446;
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
            var memberSsn = 112233446;

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

        private Guid AddItem(LibraryContext context, ItemCondition itemCondition = ItemCondition.OK, RentStatus rentStatus = RentStatus.AVAILABLE, ItemStatus itemStatus = ItemStatus.RENTABLE)
        {
            var item = ItemFactory.Get(new ItemInfo() { Author = "Test Author", Description = "A very good testing book", Title = "The best book" });
            item.ItemCondition = itemCondition;
            item.RentStatus = rentStatus;
            item.ItemStatus = itemStatus;
            context.Items.Add(item);
            context.SaveChanges();
            return context.Items.Last().Id;
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

            context.Members.AddRange(MemberFactory.Get(new PersonAPI() { Address = "Address 1", Email = "student1@test.com", Name = "Student 1", Password = "std1", Phone = "1111111111", PictureId = "std1", Ssn = 112233446 },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI() { Address = "Address 2", Email = "student2@test.com", Name = "Student 2", Password = "std2", Phone = "2222222222", PictureId = "std2", Ssn = 223344556 },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI() { Address = "Address 4", Email = "teacher1@test.com", Name = "Teacher 1", Password = "tch1", Phone = "4444444444", PictureId = "tch1", Ssn = 445566778 },
            MemberEnum.Teacher),
            MemberFactory.Get(new PersonAPI() { Address = "Address 5", Email = "teacher2@test.com", Name = "Teacher 2", Password = "tch2", Phone = "5555555555", PictureId = "tch2", Ssn = 556677889 },
            MemberEnum.Teacher));

            context.SaveChanges();

            var items = context.Items.ToList();
            context.Loans.AddRange(new Loan(items[4].Id, 112233446), new Loan(items[1].Id, 112233446), new Loan(items[2].Id, 112233446), new Loan(items[3].Id, 112233446));
            context.SaveChanges();
            // Member 112233445 has 4 loans
            return context;
        }
    }
}
