using MediatR;
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

        public DeleteEducationHandler(IEducationRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            var education = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (education == null) return false;

            _repo.Delete(education);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
