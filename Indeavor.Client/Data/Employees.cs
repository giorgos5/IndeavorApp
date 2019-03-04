using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.Client.Data
{
    public class Employees
    {
        public List<Employee> employees { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int SortMode { get; set; }
    }

    public class Employee
    {
        public long EmployeeId { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string HiringDate { get; set; }

        public List<AssignedSkill> AssignedSkills { get; set; }
        
        public List<Skill> Skills { get; set; }

        public List<Skill> AvailableSkills { get; set; }
    }
}
