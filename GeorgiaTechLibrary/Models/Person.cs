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
        public abstract long Ssn { get; set; }
        [Required]
        public abstract string Name { get; set; }
        [Required]
        public abstract string Address { get; set; }
        public abstract string PictureId { get; set; }
        [Required]
        public abstract string Email { get; set; }
        [Required]
        public abstract string Phone { get; set; }
        [Required]
        public abstract string Password { get; set; }

    }
}
