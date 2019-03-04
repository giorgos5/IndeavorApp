using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.API.Entity
{
    public class SkillResults
    {
        public List<Skill> Skills = new List<Skill>();
    }

    public class Skill
    {
        [Key]
        public long SkillId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public string CreationDate { get; set; }

        [MaxLength(1000)]
        public string Details { get; set; }
    }
}
