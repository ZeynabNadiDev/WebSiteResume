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

namespace Resume.Application.CQRS.Commands.Educations
{
    public record DeleteEducationCommand(long Id) : IRequest<bool>;
    public class DeleteEducationHandler : IRequestHandler<DeleteEducationCommand, bool>
    {
        private readonly IEducationRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;


        public DeleteEducationHandler(IEducationRepository repo, 
            IUnitOfWork uow,ICacheService cacheService)
        {
            _repo = repo;
            _uow = uow;
            _cacheService = cacheService;   
        }

        public async Task<bool> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            var education = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (education == null) return false;

            _repo.Delete(education);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"education:{request.Id}:entity");
            await _cacheService.RemoveAsync("educations:index:all");

            return true;
        }
    }

}
