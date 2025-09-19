using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Skills
{
    public record FillCreateOrEditSkillViewModelQuery(long Id) : IRequest<CreateOrEditSkillViewModel>;
    public class FillCreateOrEditSkillViewModelHandler
        : IRequestHandler<FillCreateOrEditSkillViewModelQuery, CreateOrEditSkillViewModel>
    {
        private readonly ISkillRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;


        public FillCreateOrEditSkillViewModelHandler(ISkillRepository repo, IMapper mapper, ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<CreateOrEditSkillViewModel> Handle(FillCreateOrEditSkillViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditSkillViewModel() { Id = 0 };

            var cacheKey = $"skill:{request.Id}";
            var cachedData = await _cacheService.GetAsync<CreateOrEditSkillViewModel>(cacheKey);
            if (cachedData != null)
                return cachedData;

            Skill skill = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (skill == null)
                return new CreateOrEditSkillViewModel() { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditSkillViewModel>(skill);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;     
        }
    }
}
