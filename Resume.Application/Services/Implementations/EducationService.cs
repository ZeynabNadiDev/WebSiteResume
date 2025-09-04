using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Education;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class EducationService : IEducationService
    {

        #region Constructor EducationRepository
        private readonly IEducationRepository _educationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public EducationService(IEducationRepository educationRepository,IMapper mapper,IUnitOfWork unit)
        {
            _educationRepository = educationRepository;
            _mapper = mapper;
            _uow = unit;
        }
        #endregion

        public async Task<Education> GetEducationById(long id,CancellationToken cancellationToken)
        {
            return await _educationRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<EducationViewModel>> GetAllEducations(CancellationToken cancellationToken)
        {
            var educations = await _educationRepository.GetAllOrderedAsync(cancellationToken);

            return _mapper.Map<List<EducationViewModel>>(educations);

        }

        public async Task<CreateOrEditEducationViewModel> FillCreateOrEditEducationViewModel(long id, CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditEducationViewModel() { Id = 0 };

            Education education = await GetEducationById(id,cancellationToken);

            if (education == null) return new CreateOrEditEducationViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditEducationViewModel>(education);
        }

        public async Task<bool> CreateOrEditEducation(CreateOrEditEducationViewModel education,CancellationToken cancellationToken)
        {
            if (education.Id == 0)
            {
                var newEducation = _mapper.Map<Education>(education);

                await _educationRepository.AddAsync(newEducation,cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            Education currentEducation = await GetEducationById(education.Id,cancellationToken);

            if (currentEducation == null) return false;

            _mapper.Map(education, currentEducation);

            _educationRepository.Update(currentEducation);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteEducation(long id, CancellationToken cancellationToken)
        {
            Education education = await GetEducationById(id, cancellationToken);

            if (education == null) return false;

            _educationRepository.Delete(education);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
