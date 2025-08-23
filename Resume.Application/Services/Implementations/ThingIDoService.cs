using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.ThingIDo;
using Resume.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class ThingIDoService : IThingIDoService
    {

        #region Constructor ThingIDoRepository
        private readonly IThingIDoRepository _thingIDoRepository;

        public ThingIDoService(IThingIDoRepository thingIDoRepository)
        {
            _thingIDoRepository = thingIDoRepository;
        }
        #endregion

        public async Task<ThingIDo> GetThingIDoById(long id,CancellationToken cancellationToken)
        {
            return await _thingIDoRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<ThingIDoListViewModel>> GetAllThingIDoForIndex(CancellationToken cancellationToken)
        {
            var thingIDos = await _thingIDoRepository.GetAllOrderedAsync(cancellationToken);
            return thingIDos.Select(t => new ThingIDoListViewModel()
                {
                    Id = t.Id,
                    ColumnLg = t.ColumnLg,
                    Description = t.Description,
                    Order = t.Order,
                    Icon = t.Icon,
                    Title = t.Title
                })
                .ToList();

            
        }

        public async Task<bool> CreateOrEditThingIDo(CreateOrEditThingIDoViewModel thingIDo,CancellationToken cancellationToken)
        {

            if (thingIDo.Id == 0)
            {
                // Create
                var newThingIDo = new ThingIDo()
                {
                    ColumnLg = thingIDo.ColumnLg,
                    Description = thingIDo.Description,
                    Icon = thingIDo.Icon,
                    Order = thingIDo.Order,
                    Title = thingIDo.Title
                };

                await _thingIDoRepository.AddAsync(newThingIDo,cancellationToken);
                await _thingIDoRepository.SaveChangeAsync(cancellationToken);

                return true;
            }

            // Edit
            ThingIDo currentThingIDo = await GetThingIDoById(thingIDo.Id,cancellationToken);

            if (currentThingIDo == null) return false;

            currentThingIDo.ColumnLg = thingIDo.ColumnLg;
            currentThingIDo.Description = thingIDo.Description;
            currentThingIDo.Icon = thingIDo.Icon;
            currentThingIDo.Order = thingIDo.Order;
            currentThingIDo.Title = thingIDo.Title;

            _thingIDoRepository.Update(currentThingIDo);
            await _thingIDoRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

        public async Task<CreateOrEditThingIDoViewModel> FillCreateOrEditThingIDoViewModel(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditThingIDoViewModel() { Id = 0 };

            ThingIDo thingIDo = await GetThingIDoById(id, cancellationToken);

            if (thingIDo == null) return new CreateOrEditThingIDoViewModel() { Id = 0 };

            return new CreateOrEditThingIDoViewModel()
            {
                Id = thingIDo.Id,
                ColumnLg = thingIDo.ColumnLg,
                Description = thingIDo.Description,
                Icon = thingIDo.Icon,
                Order = thingIDo.Order,
                Title = thingIDo.Title
            };
        }

        public async Task<bool> DeleteThingIDo(long id, CancellationToken cancellationToken)
        {
            ThingIDo thingIDo = await GetThingIDoById(id, cancellationToken);

            if (thingIDo == null) return false;

            _thingIDoRepository.Delete(thingIDo);
            await _thingIDoRepository.SaveChangeAsync(cancellationToken);

            return true;
        }


    }
}
