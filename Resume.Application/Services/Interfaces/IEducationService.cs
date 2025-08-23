using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Education;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IEducationService
    {
        Task<Education> GetEducationById(long id,CancellationToken cancellationToken);
        Task<List<EducationViewModel>> GetAllEducations(CancellationToken cancellationToken);
        Task<CreateOrEditEducationViewModel> FillCreateOrEditEducationViewModel(long id,CancellationToken cancellationToken);
        Task<bool> CreateOrEditEducation(CreateOrEditEducationViewModel education,CancellationToken cancellationToken);

        Task<bool> DeleteEducation(long id, CancellationToken cancellationToken);

    }
}
