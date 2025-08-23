using Resume.Domain.Entity;
using Resume.Domain.ViewModels.ThingIDo;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IThingIDoService
    {
        Task<ThingIDo> GetThingIDoById(long id,CancellationToken cancellationToken);
        Task<List<ThingIDoListViewModel>> GetAllThingIDoForIndex(CancellationToken cancellationToken);
        Task<bool> CreateOrEditThingIDo(CreateOrEditThingIDoViewModel thingIDo,CancellationToken cancellationToken);
        Task<CreateOrEditThingIDoViewModel> FillCreateOrEditThingIDoViewModel(long id, CancellationToken cancellationToken);
        Task<bool> DeleteThingIDo(long id, CancellationToken cancellationToken);

    }
}
