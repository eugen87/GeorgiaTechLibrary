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

            context.Employees.Add(EmployeeFactory.Get(new PersonAPI() { Address = "Toldstrupsgade 20", Email = "dev1@test.com", Name = "Michael Schumacher", Password = "f1winner", Phone = "15486228", PictureId = "testpictureid1", Ssn = 999555111 }, EmployeeEnum.AssistentLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI() { Address = "Sofiendelsvej 16", Email = "trev@test.com", Name = "Maria Maria", Password = "fasdfhar", Phone = "11223344", PictureId = "testpictureid1", Ssn = 523641785 }, EmployeeEnum.ChiefLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI() { Address = "Vesterbro 12", Email = "rgdsa@tedfsst.com", Name = "Jhon Mc'gee", Password = "235rehgfh7", Phone = "7586752727", PictureId = "testpictureid1", Ssn = 8531478651 }, EmployeeEnum.CheckOutStaff));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI() { Address = "Jomfru Anne Gade 69", Email = "devf23@tedsafast.com", Name = "Daniel Cash", Password = "dsam789jf", Phone = "54282854", PictureId = "testpictureid1", Ssn = 325845125 }, EmployeeEnum.DepartmentLibrarian));
            context.Employees.Add(EmployeeFactory.Get(new PersonAPI() { Address = "Hobrovej 3", Email = "helo@23442test.com", Name = "Will Smith", Password = "sdafaw4hgfs", Phone = "11223344", PictureId = "testpictureid1", Ssn = 112596325 }, EmployeeEnum.ReferenceLibrarian));

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

        [Fact(DisplayName ="Get all employees")]
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

        [Fact(DisplayName = "Update Employee")]
        public async void Update_Employee()
        {
            using (var context = GetContextWithData())
            using (var controller = new EmployeesController(context))
            {
                Employee emp = context.Employees.First();
                PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 23", Email = "de1dfv@test.com", Name = "Michaelionm Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = emp.Ssn };

                await controller.PutEmployee(person);

                var updated = context.Employees.FirstOrDefault(e => e.Ssn == emp.Ssn);

                Assert.Equal(person.Name,updated.Name);
                Assert.Equal(person.Address, updated.Address);
                Assert.Equal(person.Email, updated.Email);
            }

        }

        //[Theory]
        //[InlineData(987654321, 987654321)]
        //[InlineData(987321654, 98732165)]
        //[InlineData(321654987, 32165498732)]
        //public void Test_Ssn_Length(long expected, long ssn)
        //{
        //    PersonAPI person = new PersonAPI() { Address = "Toldstrupsgade 20", Email = "dev@test.com", Name = "Michael Schumacher", Password = "f1winner", Phone = "11223344", PictureId = "testpictureid1", Ssn = ssn };
        //
        //    using (var context = GetContextWithData())
        //    using (var controller = new EmployeesController(context))
        //    {
        //        Employee emp = EmployeeFactory.Get(person, EmployeeEnum.AssistentLibrarian);
        //
        //        output.WriteLine(emp.ToString());
        //
        //        Assert.Equal(expected, emp.Ssn);
        //    }
        //}
    }
}
