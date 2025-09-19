using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Skills
{
    public record GetAllSkillsQuery() : IRequest<List<SkillViewModel>>;
    public class GetAllSkillsHandler : IRequestHandler<GetAllSkillsQuery, List<SkillViewModel>>
    {
        private readonly ISkillRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllSkillsHandler(ISkillRepository repo, IMapper mapper,ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<SkillViewModel>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "skills:index:all";
            var cachedData= await _cacheService.GetAsync<List<SkillViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var skills = await _repo.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<SkillViewModel>>(skills);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
