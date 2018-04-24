using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Models
{
    public abstract class Person
    {
        [Key]
        public long Ssn { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PictureId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pasword { get; set; }

    }
}
