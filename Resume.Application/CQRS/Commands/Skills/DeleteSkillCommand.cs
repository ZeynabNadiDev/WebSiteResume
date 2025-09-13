using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Skills
{
    public record DeleteSkillCommand(long Id) : IRequest<bool>;
    public class DeleteSkillHandler : IRequestHandler<DeleteSkillCommand, bool>
    {
        private readonly ISkillRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteSkillHandler(ISkillRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null) return false;

            _repo.Delete(skill);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
