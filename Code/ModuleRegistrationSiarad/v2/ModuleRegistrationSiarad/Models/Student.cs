using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleRegistrationSiarad.Models
{
    public class Student
    {
        [Key]
        public virtual string user_id { get; set; }
        [Required]
        public virtual string forename { get; set; }
        [Required]
        public virtual string surname { get; set; }
    }
}
