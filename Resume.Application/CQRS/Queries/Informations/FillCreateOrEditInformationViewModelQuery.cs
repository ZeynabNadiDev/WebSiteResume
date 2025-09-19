using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record FillCreateOrEditInformationViewModelQuery():IRequest<CreateOrEditInformationViewModel>;
    public class FillCreateOrEditInformationViewModelQueryHandler : IRequestHandler<FillCreateOrEditInformationViewModelQuery, CreateOrEditInformationViewModel>
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrEditInformationViewModelQueryHandler(IInformationRepository repository,
            IMapper mapper,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<CreateOrEditInformationViewModel> Handle(FillCreateOrEditInformationViewModelQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "information:single";
            var cachedData = await _cacheService.GetAsync<CreateOrEditInformationViewModel>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var info = await _repository.GetSingleAsync(cancellationToken);
            if (info == null)
                return new CreateOrEditInformationViewModel() { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditInformationViewModel>(info);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }

}
