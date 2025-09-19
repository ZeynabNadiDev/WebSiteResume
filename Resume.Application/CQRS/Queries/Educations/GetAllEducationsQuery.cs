using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Educations
{
    public record GetAllEducationsQuery() : IRequest<List<EducationViewModel>>;
    public class GetAllEducationsHandler : IRequestHandler<GetAllEducationsQuery, List<EducationViewModel>>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllEducationsHandler(IEducationRepository repo, IMapper mapper,ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<EducationViewModel>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "educations:index:all";

            var cachedData = await _cacheService.GetAsync<List<EducationViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var educations = await _repo.GetAllOrderedAsync(cancellationToken);

            var mapped = _mapper.Map<List<EducationViewModel>>(educations);

            return mapped;
        }
    }
}
