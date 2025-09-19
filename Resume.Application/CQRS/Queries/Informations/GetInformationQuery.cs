using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Information;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record GetInformationQuery() : IRequest<InformationViewModel>;
    public class GetInformationQueryHandler : IRequestHandler<GetInformationQuery, InformationViewModel>
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetInformationQueryHandler(IInformationRepository repository,
            IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<InformationViewModel> Handle(GetInformationQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "information:single:viewmodel";
            var cachedData = await _cacheService.GetAsync<InformationViewModel>(cacheKey);

            if (cachedData != null)
                return cachedData;

            var info = await _repository.GetEntities()
                .ProjectTo<InformationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            var result = info ?? new InformationViewModel();
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
    }
}
