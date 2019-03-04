using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.Client.Data
{
    public class Skill
    {
        public long SkillId { get; set; }
        
        public string Name { get; set; }

        public string CreationDate { get; set; }

        public string Details { get; set; }
    }
}
