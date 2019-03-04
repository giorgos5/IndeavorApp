using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indeavor.API.Entity;
using Indeavor.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Indeavor.API.Controllers
{
    [Route("api/")]
    public class EmployeesController : ControllerBase
    {
        //protected Employees _emp;
        //private readonly ISearchService _services;

        //public EmployeesController(ISearchService services, Employees emp)
        //{
        //    _services = services;
        //    _emp = emp;

        //    _emp.Database.EnsureCreated();

        //    if (_emp.employees == null || (_emp.employees != null && _emp.employees.Count() == 0))
        //    {
        //        LoadDefaultEmployees();
        //    }
        //}
        
        //private void LoadDefaultEmployees()
        //{
        //    _emp.employees.Add(new Employee() { Name = "John", Surname = "Kapkidis", HiringDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), skills = new List<Skill>() { _res.Skills.ToList().First() } });
        //    _emp.SaveChanges();
        //}
    }
}