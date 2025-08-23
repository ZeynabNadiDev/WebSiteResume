using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Skill;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface ISkillService
    {
        Task<Skill> GetSkillByIdAsync(long id,CancellationToken cancellationToken);
        Task<List<SkillViewModel>> GetAllSkillsAsync(CancellationToken cancellationToken);
        Task<CreateOrEditSkillViewModel> FillCreateOrEditSkillViewModelAsync(long id,CancellationToken cancellationToken);
        Task<bool> CreateOrEditSkillAsync(CreateOrEditSkillViewModel skill,CancellationToken cancellationToken);
        Task<bool> DeleteSkillAsync(long id, CancellationToken cancellationToken);

    }
}
