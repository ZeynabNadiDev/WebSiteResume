using MediatR;
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

        public GetSkillByIdHandler(ISkillRepository repo)
        {
            _repo = repo;
        }

        public async Task<Skill> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
