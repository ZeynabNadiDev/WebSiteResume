using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record GetInformationModelQuery() : IRequest<Information?>;
    public class GetInformationModelQueryHandler : IRequestHandler<GetInformationModelQuery, Information?>
    {
        private readonly IInformationRepository _repository;
        private readonly ICacheService _cacheService;

        public GetInformationModelQueryHandler(IInformationRepository repository,ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }
        public async Task<Information?> Handle(GetInformationModelQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "information:single:entity";
            var cachedData = await _cacheService.GetAsync<Information>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var info = await _repository.GetSingleAsync(cancellationToken);
            if (info == null)
                return null;

            await _cacheService.SetAsync(cacheKey, info, TimeSpan.FromMinutes(10));

            return info;
        }
    }
}
