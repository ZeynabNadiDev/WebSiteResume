using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Skills
{
    public record GetSkillByIdQuery(long Id) : IRequest<Skill>;
    public class GetSkillByIdHandler : IRequestHandler<GetSkillByIdQuery, Skill>
    {
        private readonly ISkillRepository _repo;
        private readonly ICacheService _cacheService;

        public GetSkillByIdHandler(ISkillRepository repo,ICacheService cacheService)
        {
            _repo = repo;
            _cacheService = cacheService;
        }

        public async Task<Skill> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"skill:{request.Id}:entity";
            var cachedEntity = await _cacheService.GetAsync<Skill>(cacheKey);
            if (cachedEntity != null)
                return cachedEntity;

            var skill = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (skill != null)
                await _cacheService.SetAsync(cacheKey, skill, TimeSpan.FromMinutes(10));

            return skill;
        }
    }
}
