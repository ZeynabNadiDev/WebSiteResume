using AutoMapper;
using MediatR;
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
    public record GetAllSkillsQuery() : IRequest<List<SkillViewModel>>;
    public class GetAllSkillsHandler : IRequestHandler<GetAllSkillsQuery, List<SkillViewModel>>
    {
        private readonly ISkillRepository _repo;
        private readonly IMapper _mapper;

        public GetAllSkillsHandler(ISkillRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<SkillViewModel>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
        {
            var skills = await _repo.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<SkillViewModel>>(skills);
        }
    }
}
