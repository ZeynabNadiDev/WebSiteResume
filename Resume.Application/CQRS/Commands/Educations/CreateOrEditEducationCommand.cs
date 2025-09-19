using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
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
        private readonly ICacheService _cacheService;

        public CreateOrEditEducationHandler(IEducationRepository repo, IMapper mapper, 
            IUnitOfWork uow, ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle(CreateOrEditEducationCommand request, CancellationToken cancellationToken)
        {
            if (request.Education.Id == 0)
            {
                var newEducation = _mapper.Map<Education>(request.Education);
                await _repo.AddAsync(newEducation, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                await _cacheService.RemoveAsync("educations:index:all");
                return true;
            }
            else
            {
                var existingEducation = await _repo.GetByIdAsync(request.Education.Id, cancellationToken);
                if (existingEducation == null) return false;
                _mapper.Map(request.Education, existingEducation);
                _repo.Update(existingEducation);

                await _uow.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveAsync($"education:{request.Education.Id}:entity");
                await _cacheService.RemoveAsync("educations:index:all");

                return true;
            }

          
        }
    }

}
