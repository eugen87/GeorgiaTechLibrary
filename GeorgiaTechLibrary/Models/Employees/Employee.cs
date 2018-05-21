using GeorgiaTechLibraryAPI.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models.Employees
{
    public abstract class Employee : Person
    {

        public Employee(PersonAPI person) 
        {
            this.Address = person.Address;
            this.Email = person.Email;
            this.Name = person.Name;
            this.Password = person.Password;
            this.Phone = person.Phone;
            this.PictureId = person.PictureId;
            this.Ssn = person.Ssn;
        }
        public Employee()
        {

        }
        public abstract string Title { get; }
        
    }
}
