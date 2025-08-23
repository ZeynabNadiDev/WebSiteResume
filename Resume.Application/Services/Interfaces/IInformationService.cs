using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Information;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IInformationService
    {
        Task<InformationViewModel> GetInformation(CancellationToken cancellationToken);
        Task<Information> GetInformationModel(CancellationToken cancellationToken);
        Task<CreateOrEditInformationViewModel> FillCreateOrEditInformationViewModel(CancellationToken cancellationToken);
        Task<bool> CreateOrEditInformation(CreateOrEditInformationViewModel information,CancellationToken cancellationToken);
    }
}
