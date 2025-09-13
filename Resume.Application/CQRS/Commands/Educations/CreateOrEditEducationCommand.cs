using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Educations
{
    public record CreateOrEditEducationCommand(CreateOrEditEducationViewModel Education) : IRequest<bool>;
    public class CreateOrEditEducationHandler : IRequestHandler<CreateOrEditEducationCommand, bool>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateOrEditEducationHandler(IEducationRepository repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateOrEditEducationCommand request, CancellationToken cancellationToken)
        {
            if (request.Education.Id == 0)
            {
                var newEducation = _mapper.Map<Education>(request.Education);
                await _repo.AddAsync(newEducation, cancellationToken);
            }
            else
            {
                var existingEducation = await _repo.GetByIdAsync(request.Education.Id, cancellationToken);
                if (existingEducation == null) return false;
                _mapper.Map(request.Education, existingEducation);
                _repo.Update(existingEducation);
            }

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}
