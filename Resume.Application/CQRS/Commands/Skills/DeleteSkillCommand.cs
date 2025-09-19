using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
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
        private readonly ICacheService _cacheService;

        public DeleteSkillHandler(ISkillRepository repo, IUnitOfWork uow,ICacheService cacheService)
        {
            _repo = repo;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            var skill = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null) return false;

            _repo.Delete(skill);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"skill:{request.Id}:entity");
            await _cacheService.RemoveAsync("skills:index:all");

            return true;
        }
    }
}
