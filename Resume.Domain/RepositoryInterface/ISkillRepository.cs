using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Domain.Repository
{
    public interface ISkillRepository:IGenericRepository<Skill>
    {
          Task<List<Skill>> GetAllOrderedAsync(CancellationToken cancellationToken);
    }
}
