using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.API.Entity
{
    public class DatabaseSet : DbContext
    {
        public DbSet<Skill> Skills { get; set; }

        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<AssignedSkill> AssignedSkills { get; set; }

        public IConfiguration Configuration { get; }

        public DatabaseSet(DbContextOptions<DatabaseSet> Skills, IConfiguration configuration) : base(Skills)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
