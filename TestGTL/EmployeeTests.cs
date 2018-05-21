using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Models.Employees;
using GeorgiaTechLibraryAPI.Controllers;
using GeorgiaTechLibraryAPI.Models.APIModel;
using GeorgiaTechLibraryAPI.Models.Factories.Employees;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace TestGTL
{
    public class EmployeeTests
    {
        private readonly ITestOutputHelper output;
        public EmployeeTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private LibraryContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new LibraryContext(options);

            context.Employees.Add(EmployeeFactory.Get(new PersonAPI()
            {
                Address = "Toldstrupsgade 20",
                Email = "dev1@test.com",
                Name = "Michael Schumacher",
                Password = "f1winner",
                Phone = "15486228",
                PictureId = "testpictureid1",
                Ssn = 999555111
            }, EmployeeEnum.AssistentLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI()
            {
                Address = "Sofiendelsvej 16",
                Email = "trev@test.com",
                Name = "Maria Maria",
                Password = "fasdfhar",
                Phone = "11223344",
                PictureId = "testpictureid1",
                Ssn = 523641785
            }, EmployeeEnum.ChiefLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI()
            {
                Address = "Vesterbro 12",
                Email = "rgdsa@tedfsst.com",
                Name = "Jhon Mc'gee",
                Password = "235rehgfh7",
                Phone = "7586752727",
                PictureId = "testpictureid1",
                Ssn = 853147865
            }, EmployeeEnum.CheckOutStaff));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI()
            {
                Address = "Jomfru Anne Gade 69",
                Email = "devf23@tedsafast.com",
                Name = "Daniel Cash",
                Password = "dsam789jf",
                Phone = "54282854",
                PictureId = "testpictureid1",
                Ssn = 325845125
            }, EmployeeEnum.DepartmentLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI()
            {
                Address = "Hobrovej 3",
                Email = "helo@23442test.com",
                Name = "Will Smith",
                Password = "sdafaw4hgfs",
                Phone = "11223344",
                PictureId = "testpictureid1",
                Ssn = 112596325
            }, EmployeeEnum.ReferenceLibrarian));

            context.SaveChanges();

            return context;
        }


        [Theory]
        [InlineData("Chief Librarian", EmployeeEnum.ChiefLibrarian)]
        [InlineData("Department Librarian", EmployeeEnum.DepartmentLibrarian)]
        [InlineData("Reference Librarian", EmployeeEnum.ReferenceLibrarian)]
        [InlineData("Assistant Librarian", EmployeeEnum.AssistentLibrarian)]
        [InlineData("CheckOut Staff", EmployeeEnum.CheckOutStaff)]

        public void Factory_Create_Employee(string expected, EmployeeEnum empType)
        {
            //Arrange
            PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = "dev@test.com", Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = 999555111 };

            //Act
            Employee emp = EmployeeFactory.Get(person, empType);

            //Assert
            Assert.Equal(expected, emp.Title);
        }

        [Fact(DisplayName = "Get all employees")]
        public async void Get_All_Employees()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                var result = await controller.GetEmployees();
                output.WriteLine(result.Count().ToString());
                Assert.True(result.Count() != 0);
            }
        }

        [Theory]
        [InlineData("Chief Librarian", 523641785)]
        [InlineData("Department Librarian", 325845125)]
        [InlineData("Reference Librarian", 112596325)]
        [InlineData("Assistant Librarian", 999555111)]
        [InlineData("CheckOut Staff", 853147865)]
        public async void Get_Employee_With_Ssn(string expected, long ssn)
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                var result = await controller.GetEmployee(ssn);

                var emp = context.Employees.FirstOrDefault(e => e.Ssn == ssn);

                Assert.Equal(expected, emp.Title);
            }
        }

        [Theory]
        [InlineData("Chief Librarian", EmployeeEnum.ChiefLibrarian)]
        [InlineData("Department Librarian", EmployeeEnum.DepartmentLibrarian)]
        [InlineData("Reference Librarian", EmployeeEnum.ReferenceLibrarian)]
        [InlineData("Assistant Librarian", EmployeeEnum.AssistentLibrarian)]
        [InlineData("CheckOut Staff", EmployeeEnum.CheckOutStaff)]
        public void Add_Employee(string expected, EmployeeEnum empType)
        {
            PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = "dev@test.com", Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = 999557811 };

            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                var result = controller.PostEmployee(person, (int)empType);

                var emp = context.Employees.FirstOrDefault(e => e.Ssn == person.Ssn);
                Assert.Equal(expected, emp.Title);
                output.WriteLine(context.Employees.Count().ToString());
            }
        }

        [Fact(DisplayName = "Delete Employee")]
        public void Delete_Employee()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();

                var result = controller.DeleteEmployee(emp.Ssn);

                var del = context.Employees.FirstOrDefault(e => e.Ssn == emp.Ssn);
                Assert.False(del != null);
            }

        }

        [Fact(DisplayName = "Update Employee Name")]
        public async void Update_Employee_Name()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                context.Entry(emp).State = EntityState.Detached;

                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 23", Email = "de1dfv@test.com", Name = "Michaelionm Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var actual = context.Employees.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Name, actual.Name);
            }
        }

        [Fact(DisplayName = "Update Employee Phone number")]
        public async void Update_Employee_Phone()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                context.Entry(emp).State = EntityState.Detached;

                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 23", Email = "de1dfv@test.com", Name = "Michaelionm Schumacher", Password = "f1winner", Phone = "11223366", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var actual = context.Employees.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Phone, actual.Phone);
            }
        }

        [Fact(DisplayName = "Update Employee Address")]
        public async void Update_Employee_Address()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                context.Entry(emp).State = EntityState.Detached;

                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 450", Email = "de1dfv@test.com", Name = "Michaelionm Schumacher", Password = "f1winner", Phone = "11223366", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var actual = context.Employees.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Address, actual.Address);
            }
        }

        [Fact(DisplayName = "Update Employee Email")]
        public async void Update_Employee_Email()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                context.Entry(emp).State = EntityState.Detached;

                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 450", Email = "12345@test.com", Name = "Michaelionm Schumacher", Password = "f1winner", Phone = "11223366", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var actual = context.Employees.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Email, actual.Email);
            }
        }

        [Fact(DisplayName = "Update Employee Password")]
        public async void Update_Employee_Password()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                context.Entry(emp).State = EntityState.Detached;

                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 450", Email = "12345@test.com", Name = "Michaelionm Schumacher", Password = "bestpasswordever", Phone = "11223366", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var actual = context.Employees.AsNoTracking().Where(e => e.Ssn == person.Ssn).FirstOrDefault();

                Assert.Equal(person.Password, actual.Password);
            }
        }

        [Theory]
        [InlineData(true, 1234567890)]
        [InlineData(true, 12345678)]
        [InlineData(false, 123456789)]
        public void Valid_Ssn(bool expected, long ssn)
        {

            //Arrange
            PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = "dev@test.com", Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = ssn };

            //Act
            Employee emp = EmployeeFactory.Get(person, EmployeeEnum.DepartmentLibrarian);

            //Assert
            Assert.Equal(expected, (emp==null));
        }

        [Theory]
        [InlineData(true, "email.com")]
        [InlineData(false, "dev@ucn.dk")]
        [InlineData(true, "abscddss")]
        [InlineData(true, "1234567")]
        public void Valid_Email(bool expected,string email)
        {
            //Arrange
            PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = email, Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = 123456789 };

            //Act
            Employee emp = EmployeeFactory.Get(person, EmployeeEnum.DepartmentLibrarian);

            //Assert
            Assert.Equal(expected, (emp == null));
        }

        [Theory]
        [InlineData("email.com")]
        [InlineData("abscddss")]
        [InlineData("1234567")]
        public void Add_Employee_With_Invalid_Email(string email)
        {
            PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = email, Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = 123456788 };

            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                var result = controller.PostEmployee(person, 2);

                var emp = context.Employees.FirstOrDefault(e => e.Ssn == person.Ssn);
                Employee expected = null;

                Assert.Equal(expected, emp);
            }
        }

    }
}
