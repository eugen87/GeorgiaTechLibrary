using GeorgiaTechLibrary.Models;
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
    public class MemberTests
    {
        private readonly ITestOutputHelper output;
        public MemberTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private LibraryContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new LibraryContext(options);

            context.Members.Add(MemberFactory.Get(new PersonAPI() {
                Address = "Address 1",
                Email = "student1@test.com",
                Name = "Student 1",
                Password = "std1",
                Phone = "11111111",
                PictureId = "std1",
                Ssn = 112233445
            },
            MemberEnum.Student ));
            context.Members.Add(MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 2",
                Email = "student2@test.com",
                Name = "Student 2",
                Password = "std2",
                Phone = "22222222",
                PictureId = "std2",
                Ssn = 223344556
            },
            MemberEnum.Student));
            context.Members.Add(MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 3",
                Email = "student3@test.com",
                Name = "Student 3",
                Password = "std3",
                Phone = "33333333",
                PictureId = "std3",
                Ssn = 334455667
            },
            MemberEnum.Student));
            context.Members.Add(MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 4",
                Email = "teacher1@test.com",
                Name = "Teacher 1",
                Password = "tch1",
                Phone = "44444444",
                PictureId = "tch1",
                Ssn = 445566778
            },
            MemberEnum.Teacher));
            context.Members.Add(MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 5",
                Email = "teacher2@test.com",
                Name = "Teacher 2",
                Password = "tch2",
                Phone = "55555555",
                PictureId = "tch2",
                Ssn = 556677889
            },
            MemberEnum.Teacher));
            context.Members.Add(MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 6",
                Email = "teacher3@test.com",
                Name = "Teacher 3",
                Password = "tch3",
                Phone = "66666666",
                PictureId = "tch3",
                Ssn = 667788990
            },
            MemberEnum.Teacher));

            context.SaveChanges();

            return context;
        }

        [Theory]
        [InlineData(1, MemberEnum.Student)]
        [InlineData(2, MemberEnum.Teacher)]
        public void Factory_Create_Member(int expected, MemberEnum memType)
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "Address 2",
                Email = "student2@test.com",
                Name = "Student 2",
                Password = "std2",
                Phone = "22222222",
                PictureId = "std2",
                Ssn = 223344556
            };

            Member mem = MemberFactory.Get(person, memType);

            Assert.Equal(expected, mem.LoanRule.Id);
        }

        [Fact(DisplayName = "Get all members")]
        public async void Get_All_Members()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.GetMembers();
                output.WriteLine(result.Count().ToString());
                Assert.True(result.Count() != 0);
            }
        }

        [Theory]
        [InlineData(1, MemberEnum.Student)]
        [InlineData(2, MemberEnum.Teacher)]
        public void Add_Member(int expected, MemberEnum memType)
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "Address 2",
                Email = "student2@test.com",
                Name = "Student 2",
                Password = "std2",
                Phone = "22222222",
                PictureId = "std2",
                Ssn = 223344556
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = controller.PostMember(person, (int)memType);
                output.WriteLine(memType.ToString());

                var mem = context.Members.FirstOrDefault(m => m.Ssn == person.Ssn);
                output.WriteLine(mem.LoanRule.Id.ToString());
                Assert.Equal(expected, mem.LoanRuleid);
                output.WriteLine(context.Members.Count().ToString());
            }
        }
    }
}
