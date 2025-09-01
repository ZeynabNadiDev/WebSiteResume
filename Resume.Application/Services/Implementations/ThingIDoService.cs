using AutoMapper;
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
        private readonly IMapper _mapper;
        public ThingIDoService(IThingIDoRepository thingIDoRepository,IMapper mapper)
        {
            _thingIDoRepository = thingIDoRepository;
            _mapper = mapper;
        }
        #endregion

        public async Task<ThingIDo> GetThingIDoById(long id,CancellationToken cancellationToken)
        {
            return await _thingIDoRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<ThingIDoListViewModel>> GetAllThingIDoForIndex(CancellationToken cancellationToken)
        {
            var thingIDos = await _thingIDoRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<ThingIDoListViewModel>>(thingIDos);


        }

        public async Task<bool> CreateOrEditThingIDo(CreateOrEditThingIDoViewModel thingIDo,CancellationToken cancellationToken)
        {

            if (thingIDo.Id == 0)
            {
                // Create
                var newThingIDo = _mapper.Map<ThingIDo>(thingIDo);


                await _thingIDoRepository.AddAsync(newThingIDo,cancellationToken);
                await _thingIDoRepository.SaveChangeAsync(cancellationToken);

                return true;
            }

            // Edit
            ThingIDo currentThingIDo = await GetThingIDoById(thingIDo.Id,cancellationToken);

            if (currentThingIDo == null) return false;
            _mapper.Map(thingIDo, currentThingIDo);

            _thingIDoRepository.Update(currentThingIDo);
            await _thingIDoRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

        public async Task<CreateOrEditThingIDoViewModel> FillCreateOrEditThingIDoViewModel(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditThingIDoViewModel() { Id = 0 };

            ThingIDo thingIDo = await GetThingIDoById(id, cancellationToken);

            if (thingIDo == null) return new CreateOrEditThingIDoViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditThingIDoViewModel>(thingIDo);
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
