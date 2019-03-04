using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.API.Entity
{
    public class AssignedSkill
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("SkillId")]
        public long SkillId { get; set; }

        [ForeignKey("EmployeeId")]
        public long EmployeeId { get; set; }
    }
    
}
