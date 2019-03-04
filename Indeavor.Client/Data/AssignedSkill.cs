using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.Client.Data
{
    public class AssignedSkill
    {
        public long Id { get; set; }
        
        public long SkillId { get; set; }
        
        public long EmployeeId { get; set; }
    }
}
