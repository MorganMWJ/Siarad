using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModuleRegistrationSiarad.Models
{
    public class Module
    {
        public virtual string module_id { get; set; }
        public virtual string year { get; set; }
        [Required]
        public virtual string coordinator_id { get; set; }//Coordinator
        [Required]
        public virtual string module_title { get; set; }
    }
   
}
