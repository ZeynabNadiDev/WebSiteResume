using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Information;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Resume.Application.Services.Implementations
{
    public class InformationService : IInformationService
    {

        #region Constructor InformationRepository
       private IInformationRepository _informationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public InformationService(IInformationRepository informationRepository,IMapper mapper,IUnitOfWork unit)
        {
            _informationRepository = informationRepository;
            _mapper = mapper;
            _uow = unit;
        }


        #endregion

        public async Task<InformationViewModel> GetInformation(CancellationToken cancellationToken)
        {
            InformationViewModel information = await _informationRepository.GetEntities()
              .ProjectTo<InformationViewModel>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

            return information ?? new InformationViewModel();
        }

        public async Task<Information?> GetInformationModel(CancellationToken cancellationToken)
        {
            return await _informationRepository.GetSingleAsync(cancellationToken);
        }

        public async Task<CreateOrEditInformationViewModel> FillCreateOrEditInformationViewModel(CancellationToken cancellationToken)
        {
            Information information = await GetInformationModel(cancellationToken);

            if (information == null) return new CreateOrEditInformationViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditInformationViewModel>(information);

        }

        public async Task<bool> CreateOrEditInformation(CreateOrEditInformationViewModel information,CancellationToken cancellationToken)
        {
            if (information.Id == 0)
            {
                var newInformation = _mapper.Map<Information>(information);

                await _informationRepository.AddAsync(newInformation,cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            Information currentInformation = await GetInformationModel(cancellationToken);


            if (currentInformation == null)
                return false;

            _mapper.Map(information, currentInformation);

            _informationRepository.Update(currentInformation);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;

        }


    }
}
