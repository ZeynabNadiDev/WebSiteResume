using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Experience;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Experiences
{
     public record GetAllExperiencesQuery():IRequest<List<ExperienceViewModel>>;
    public class GetAllExperiencesQueryHandler : IRequestHandler<GetAllExperiencesQuery, List<ExperienceViewModel>>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllExperiencesQueryHandler(
            IExperienceRepository experienceRepository,
            IMapper mapper, ICacheService cacheService)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<ExperienceViewModel>> Handle(
            GetAllExperiencesQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = "experiences:index:all";
            var cachedData = await _cacheService.GetAsync<List<ExperienceViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var experience = await _experienceRepository.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<ExperienceViewModel>>(experience);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }

}
