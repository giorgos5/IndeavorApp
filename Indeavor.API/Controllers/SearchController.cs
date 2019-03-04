using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indeavor.API.Entity;
using Indeavor.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Indeavor.API.Controllers
{
    [Route("api/")]
    public class SearchController : ControllerBase
    {
        protected DatabaseSet _res;
        private readonly ISearchService _services;

        public SearchController(ISearchService services, DatabaseSet res)
        {
            _services = services;
            _res = res;
            
            _res.Database.EnsureCreated();
            if(_res.Skills == null || (_res.Skills != null && _res.Skills.Count() == 0))
            {
                LoadDefaultSkills();
            }

            if (_res.Employees == null || (_res.Employees != null && _res.Employees.Count() == 0))
            {
                LoadDefaultEmployees();
            }
        }

        #region Skills

        [HttpGet]
        [Route("GetSkills")]
        public SkillResults GetSkills()

        {
            SkillResults results = new SkillResults();
            results.Skills = _res.Skills.ToList();

            return results;
        }

        [HttpGet]
        [Route("GetSkillDetails")]
        public Skill GetSkillDetails(string id)
        {
            return _res.Skills.ToList().First(x => x.SkillId.ToString() == id);
        }

        [HttpPost]
        [Route("UpdateSkill")]
        public Response UpdateSkill([FromBody]Skill skill)
        {
            Response resp = new Response();
            try
            {
                _res.Entry(skill).State = EntityState.Modified;
                _res.SaveChanges();

                resp.ErrorCode = "0";

                return resp;
            }
            catch (Exception ex)
            {
                resp.ErrorCode = "-999";
                resp.ErrorLabel = ex.ToString();
                return resp;
            }
        }

        [HttpPost]
        [Route("AddSkill")]
        public Response AddSkill([FromBody]Skill skill)
        {
            Response resp = new Response();
            try
            {
                _res.Skills.Add(skill);
                _res.SaveChanges();

                resp.ErrorCode = "0";
                resp.Id = _res.Skills.ToList().Last().SkillId.ToString();

                return resp;
            }
            catch (Exception ex)
            {
                resp.ErrorCode = "-999";
                resp.ErrorLabel = ex.ToString();
                return resp;
            }
        }
        
        [HttpPost]
        [Route("DeleteSkill")]
        public string DeleteSkill(string id)
        {
            try
            {
                Skill skill = new Skill() { SkillId = long.Parse(id) };
                _res.Skills.Attach(skill);
                _res.Skills.Remove(skill);
                _res.SaveChanges();

                return "1";
            }
            catch (Exception ex)
            {
                return "-999";
            }
        }

        private void LoadDefaultSkills()
        {
            _res.Skills.Add(new Skill() { Name = "Skill 1", Details = "Test Skill 1", CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") });
            _res.Skills.Add(new Skill() { Name = "Skill 2", Details = "Test Skill 2", CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") });
            _res.Skills.Add(new Skill() { Name = "Skill 3", Details = "Test Skill 3", CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") });
            _res.Skills.Add(new Skill() { Name = "Skill 4", Details = "Test Skill 4", CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") });
            _res.Skills.Add(new Skill() { Name = "Skill 5", Details = "Test Skill 5", CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") });
            _res.SaveChanges();
        }

        private void LoadDefaultEmployees()
        {
            _res.Employees.Add(new Employee() { Name = "John", Surname = "Parker", HiringDate = DateTime.Now.ToString("dd/MM/yyyy"), AssignedSkills = new List<AssignedSkill>() { new AssignedSkill() { SkillId = 1, EmployeeId = 1 } } });
            _res.Employees.Add(new Employee() { Name = "Tom", Surname = "James", HiringDate = DateTime.Now.ToString("dd/MM/yyyy"), AssignedSkills = new List<AssignedSkill>() { new AssignedSkill() { SkillId = 1, EmployeeId = 2} } });
            _res.SaveChanges();
        }
        #endregion

        #region Employees
        [HttpGet]
        [Route("GetEmployees")]
        public Employees GetEmployees()
        {
            Employees employees = new Employees();
            employees.employees = _res.Employees.ToList();

            SkillResults results = new SkillResults();
            results.Skills = _res.Skills.ToList();

            foreach(Employee emp in employees.employees)
            {
                emp.AssignedSkills = _res.AssignedSkills.ToList().Where(x => x.EmployeeId == emp.EmployeeId).ToList();
                emp.Skills = new List<Skill>();
                foreach (AssignedSkill assignedSkill in emp.AssignedSkills)
                {
                    Skill sk = _res.Skills.ToList().FirstOrDefault(x => x.SkillId == assignedSkill.SkillId);
                    emp.Skills.Add(sk);
                }
            }

            return employees;
        }

        [HttpGet]
        [Route("GetEmployeeDetails")]
        public Employee GetEmployeeDetails(string id)
        {
            Employee emp = _res.Employees.ToList().First(x => x.EmployeeId.ToString() == id);
            emp.AssignedSkills = _res.AssignedSkills.ToList().Where(x => x.EmployeeId == emp.EmployeeId).ToList();
            emp.Skills = new List<Skill>();
            foreach (AssignedSkill assignedSkill in emp.AssignedSkills)
            {
                Skill sk = _res.Skills.ToList().FirstOrDefault(x => x.SkillId == assignedSkill.SkillId);
                emp.Skills.Add(sk);
            }
            emp.AvailableSkills = _res.Skills.ToList().Where(x => !emp.AssignedSkills.Exists(d => d.SkillId == x.SkillId)).ToList();


            return emp;
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        public Response UpdateEmployee([FromBody]Employee emp)
        {
            Response resp = new Response();
            try
            {
                _res.Entry(emp).State = EntityState.Modified;
                _res.SaveChanges();

                resp.ErrorCode = "0";

                return resp;
            }
            catch (Exception ex)
            {
                resp.ErrorCode = "-999";
                resp.ErrorLabel = ex.ToString();
                return resp;
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public Response AddEmployee([FromBody]Employee emp)
        {
            Response resp = new Response();
            try
            {
                _res.Employees.Add(emp);
                _res.SaveChanges();

                resp.ErrorCode = "0";
                resp.Id = _res.Employees.ToList().Last().EmployeeId.ToString();

                return resp;
            }
            catch (Exception ex)
            {
                resp.ErrorCode = "-999";
                resp.ErrorLabel = ex.ToString();
                return resp;
            }
        }

        [HttpPost]
        [Route("DeleteEmployees")]
        public string DeleteEmployees(string ids)
        {
            try
            {
                List<string> IDs = !string.IsNullOrEmpty(ids) ? ids.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
                foreach (string id in IDs)
                {
                    Employee emp = new Employee() { EmployeeId = long.Parse(id) };
                    _res.Employees.Attach(emp);
                    _res.Employees.Remove(emp);
                }

                _res.SaveChanges();

                return "1";
            }
            catch (Exception ex)
            {
                return "-999";
            }
        }


        [HttpPost]
        [Route("AddEmployeeSkill")]
        public string AddEmployeeSkill(string employeeid, string skillid)
        {
            try
            {
                _res.AssignedSkills.Add(new AssignedSkill() { EmployeeId = long.Parse(employeeid), SkillId = long.Parse(skillid) });
                _res.SaveChanges();

                return "1";
            }
            catch (Exception ex)
            {
                return "-999";
            }
        }


        [HttpPost]
        [Route("DeleteEmployeeSkill")]
        public string DeleteEmployeeSkill(string employeeid, string skillid)
        {
            try
            {
                AssignedSkill skill = _res.AssignedSkills.ToList().First(x => x.EmployeeId == long.Parse(employeeid) && x.SkillId == long.Parse(skillid));
                _res.AssignedSkills.Attach(skill);
                _res.AssignedSkills.Remove(skill);
                _res.SaveChanges();

                return "1";
            }
            catch (Exception ex)
            {
                return "-999";
            }
        }
        #endregion
    }
}