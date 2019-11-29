using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleRegistrationSiarad.Models
{
    public class StudentsRegistreredForSpecificModules
    {
        [Key]
        public virtual int auto_id { get; set; }
        [ForeignKey("module_id, year")]
        [NotMapped]
        public virtual Module Module { get; set; }
        [Required]
        public virtual string module_id { get; set; }
        [Required]
        public virtual string year { get; set; }
        [ForeignKey("user_id")]
        [NotMapped]
        public virtual Student Student { get; set; }
        [Required]
        public virtual string user_id { get; set; }
    }
}
