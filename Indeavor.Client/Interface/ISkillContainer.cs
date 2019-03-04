using Indeavor.Client.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indeavor.Client.Interface
{
    public static class ThisSkill
    {
        public static SkillResults SkillResults => ISkillContainer.Provider.GetService<SkillResults>();
    }

    public static class ISkillContainer
    {
        public static ServiceProvider Provider { get; set; }
    }
}
