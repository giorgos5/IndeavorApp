using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.API.Entity
{
    public class Employees
    {
        public List<Employee> employees { get; set; }
    }

    public class Employee
    {
        [Key]
        public long EmployeeId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Surname { get; set; }

        [Required]
        public string HiringDate { get; set; }

        public List<AssignedSkill> AssignedSkills { get; set; }

        [NotMapped]
        public List<Skill> Skills { get; set; }

        [NotMapped]
        public List<Skill> AvailableSkills { get; set; }
    }
}
