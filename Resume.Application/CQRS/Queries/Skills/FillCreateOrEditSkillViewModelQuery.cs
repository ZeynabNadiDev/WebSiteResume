using AutoMapper;
using MediatR;
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

        public FillCreateOrEditSkillViewModelHandler(ISkillRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreateOrEditSkillViewModel> Handle(FillCreateOrEditSkillViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditSkillViewModel() { Id = 0 };

            Skill skill = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (skill == null)
                return new CreateOrEditSkillViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditSkillViewModel>(skill);
        }
    }
}
