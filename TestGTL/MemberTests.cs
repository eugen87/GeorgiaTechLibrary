using GeorgiaTechLibrary.Models;
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
    public class MemberTests
    {
        private readonly ITestOutputHelper output;
        public MemberTests(ITestOutputHelper output)
        {
            this.output = output;
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
                Phone = "2222222222",
                PictureId = "std2",
                Ssn = 223344556
            };

            Member mem = MemberFactory.Get(person, memType);

            Assert.Equal(expected, mem.LoanRuleId);
        }

        [Fact(DisplayName = "Get all members")]
        public async void Get_All_Members()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.GetMembers();
                output.WriteLine(result.AsEnumerable().FirstOrDefault().LoanRule.GracePeriod.ToString());
                Assert.True(result.Count() != 0);
            }
        }

        [Theory]
        [InlineData(1, 112233445)]
        [InlineData(2, 445566778)]
        public async void Get_Members_With_Ssn(int expected, long ssn)
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.GetMember(ssn);

                var mem = context.Members.FirstOrDefault(m => m.Ssn == ssn);
                Assert.Equal(expected, mem.LoanRuleId);
            }
        }

        [Theory]
        [InlineData(2, MemberEnum.Teacher)]
        [InlineData(1, MemberEnum.Student)]
        public async void Add_Member(int expected, MemberEnum memType)
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "Address 2",
                Email = "student2@test.com",
                Name = "Student 2",
                Password = "std2",
                Phone = "1234567896",
                PictureId = "std2",
                Ssn = 123852357
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)memType);
                var mems = await controller.GetMembers();
                var mem = mems.Where(m => m.Ssn == person.Ssn).FirstOrDefault();
                Assert.Equal(expected, mem.LoanRuleId);
            }
        }

        [Fact(DisplayName = "CS1 Add Student")]
        public async void Add_Student()
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "New York",
                Email = "johndoe@test.com",
                Name = "John Doe",
                Password = "Std1",
                Phone = "1199887766",
                PictureId = "pcId",
                Ssn = 112233445
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)MemberEnum.Student);
                var mems = await controller.GetMembers();
                var mem = mems.Where(m => m.Ssn == person.Ssn).FirstOrDefault();
                Assert.True(mem is Student);
            }
        }

        [Fact(DisplayName = "CS2.1 Don't add Student with wrong SSN")]
        public async void Add_Student_Wrong_SSN()
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "New York",
                Email = "johndoe@test.com",
                Name = "John Doe",
                Password = "Std1",
                Phone = "99887766",
                PictureId = "pcId",
                Ssn = 22334455
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)MemberEnum.Student);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "CS2.2 Don't add Student with wrong EMAIL")]
        public async void Add_Student_Wrong_EMAIL()
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "New York",
                Email = "janedoe .com",
                Name = "John Doe",
                Password = "Std1",
                Phone = "99887766",
                PictureId = "pcId",
                Ssn = 225566778
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)MemberEnum.Student);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "CS2.3 Don't add Student with wrong PHONE")]
        public async void Add_Student_Wrong_PHONE()
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "New York",
                Email = "johndoe@test.com",
                Name = "John Doe",
                Password = "Std1",
                Phone = "887766",
                PictureId = "pcId",
                Ssn = 225566778
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)MemberEnum.Student);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "CS3 Don't add Student who exists")]
        public async void Add_Student_Existing()
        {
            PersonAPI person = new PersonAPI()
            {
                Address = "Los Angeles",
                Email = "jackdoe@test.com",
                Name = "John Doe",
                Password = "Std3",
                Phone = "77665544",
                PictureId = "pcId",
                Ssn = 334455667
            };

            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                var result = await controller.PostMember(person, (int)MemberEnum.Student);

                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact(DisplayName = "Delete Member")]
        public void Delete_Member()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();

                var result = controller.DeleteMember(mem.Ssn);

                var del = context.Members.FirstOrDefault(e => e.Ssn == mem.Ssn);
                Assert.False(del != null);
            }

        }

        [Fact(DisplayName = "Update Member Name")]
        public async void Update_Member_Name()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();
                context.Entry(mem).State = EntityState.Detached;

                PersonAPI person = new PersonAPI()
                {
                    Address = "Address 4",
                    Email = "teacher1@test.com",
                    Name = "Teacher 1",
                    Password = "tch1",
                    Phone = "4444444444",
                    PictureId = "tch1",
                    Ssn = mem.Ssn
                };

                await controller.PutMember(person);

                var actual = context.Members.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Name, actual.Name);
            }
        }

        [Fact(DisplayName = "Update Member Address")]
        public async void Update_Member_Address()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();
                context.Entry(mem).State = EntityState.Detached;

                PersonAPI person = new PersonAPI()
                {
                    Address = "Address 10",
                    Email = "teacher1@test.com",
                    Name = "Teacher 1",
                    Password = "tch1",
                    Phone = "4444444444",
                    PictureId = "tch1",
                    Ssn = mem.Ssn
                };

                await controller.PutMember(person);

                var actual = context.Members.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Address, actual.Address);
            }
        }

        [Fact(DisplayName = "Update Member Email")]
        public async void Update_Member_Email()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();
                context.Entry(mem).State = EntityState.Detached;

                PersonAPI person = new PersonAPI()
                {
                    Address = "Address 10",
                    Email = "teacher10@test.com",
                    Name = "Teacher 1",
                    Password = "tch1",
                    Phone = "4444444444",
                    PictureId = "tch1",
                    Ssn = mem.Ssn
                };

                await controller.PutMember(person);

                var actual = context.Members.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Email, actual.Email);
            }
        }

        [Fact(DisplayName = "Update Member Password")]
        public async void Update_Member_Password()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();
                context.Entry(mem).State = EntityState.Detached;

                PersonAPI person = new PersonAPI()
                {
                    Address = "Address 10",
                    Email = "teacher1@test.com",
                    Name = "Teacher 1",
                    Password = "tch10",
                    Phone = "4444444444",
                    PictureId = "tch1",
                    Ssn = mem.Ssn
                };

                await controller.PutMember(person);

                var actual = context.Members.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Password, actual.Password);
            }
        }

        [Fact(DisplayName = "Update Member Phone")]
        public async void Update_Member_Phone()
        {
            using (var context = GetContextWithData())
            using (var controller = new MembersController(context))
            {
                Member mem = context.Members.First();
                context.Entry(mem).State = EntityState.Detached;

                PersonAPI person = new PersonAPI()
                {
                    Address = "Address 10",
                    Email = "teacher1@test.com",
                    Name = "Teacher 1",
                    Password = "tch1",
                    Phone = "1010101010",
                    PictureId = "tch1",
                    Ssn = mem.Ssn
                };

                await controller.PutMember(person);

                var actual = context.Members.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Phone, actual.Phone);
            }
        }


        private LibraryContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new LibraryContext(options);

            context.LoanRules.AddRange(new LoanRule() { Id = 1, LoanTime = 5, BookLimit = 5, GracePeriod = 20 },
                new LoanRule() { Id = 2, LoanTime = 5, BookLimit = 5, GracePeriod = 20 });
            context.Members.AddRange(MemberFactory.Get(new PersonAPI()
            {
                Password = "std1",
<<<<<<< HEAD
                Phone = "1111111111",
=======
>>>>>>> 60c2f738967e56837bae2b1e1721e9c52ba63fc6
                PictureId = "std1",
                Ssn = 112233445
            },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI()
            {
                Password = "std2",
<<<<<<< HEAD
                Phone = "2222222222",
=======
                Phone = "88776655",
>>>>>>> 60c2f738967e56837bae2b1e1721e9c52ba63fc6
                PictureId = "std2",
                Ssn = 223344556
            },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI()
            {
                Password = "std3",
<<<<<<< HEAD
                Phone = "3333333333",
=======
                Phone = "77665544",
>>>>>>> 60c2f738967e56837bae2b1e1721e9c52ba63fc6
                PictureId = "std3",
                Ssn = 334455667
            },
            MemberEnum.Student),
            MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 4",
                Email = "teacher1@test.com",
                Name = "Teacher 1",
                Password = "tch1",
                Phone = "4444444444",
                PictureId = "tch1",
                Ssn = 445566778
            },
            MemberEnum.Teacher),
            MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 5",
                Email = "teacher2@test.com",
                Name = "Teacher 2",
                Password = "tch2",
                Phone = "5555555555",
                PictureId = "tch2",
                Ssn = 556677889
            },
            MemberEnum.Teacher),
            MemberFactory.Get(new PersonAPI()
            {
                Address = "Address 6",
                Email = "teacher3@test.com",
                Name = "Teacher 3",
                Password = "tch3",
                Phone = "6666666666",
                PictureId = "tch3",
                Ssn = 667788990
            },
            MemberEnum.Teacher));

            context.SaveChanges();

            return context;
        }

    }
}
