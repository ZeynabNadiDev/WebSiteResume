using Resume.Domain.ViewModels.Experience;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<List<ExperienceViewModel>> GetAllExperiences(CancellationToken cancellationToken);
    }
}
