using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Education;
using Resume.Infra.Data.Context;
using Resume.Infra.Data.Repository;
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
        public EducationService(IEducationRepository educationRepository)
        {
            _educationRepository = educationRepository;
        }
        #endregion

        public async Task<Education> GetEducationById(long id,CancellationToken cancellationToken)
        {
            return await _educationRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<EducationViewModel>> GetAllEducations(CancellationToken cancellationToken)
        {
            var educations = await _educationRepository.GetAllOrderedAsync(cancellationToken);

              return educations.Select(c => new EducationViewModel()
                {
                    Description = c.Description,
                    EndDate = c.EndDate,
                    Id = c.Id,
                    StartDate = c.StartDate,
                    Title = c.Title,
                    Order = c.Order
                })
                .ToList();

        }

        public async Task<CreateOrEditEducationViewModel> FillCreateOrEditEducationViewModel(long id, CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditEducationViewModel() { Id = 0 };

            Education education = await GetEducationById(id,cancellationToken);

            if (education == null) return new CreateOrEditEducationViewModel() { Id = 0 };

            return new CreateOrEditEducationViewModel()
            {
                Id = education.Id,
                Description = education.Description,
                EndDate = education.EndDate,
                Order = education.Order,
                StartDate = education.StartDate,
                Title = education.Title
            };
        }

        public async Task<bool> CreateOrEditEducation(CreateOrEditEducationViewModel education,CancellationToken cancellationToken)
        {
            if (education.Id == 0)
            {
                var newEducation = new Education()
                {
                    Description = education.Description,
                    EndDate = education.EndDate,
                    Order = education.Order,
                    StartDate = education.StartDate,
                    Title = education.Title
                };

                await _educationRepository.AddAsync(newEducation,cancellationToken);
                await _educationRepository.SaveChangeAsync(cancellationToken);
                return true;
            }

            Education currentEducation = await GetEducationById(education.Id,cancellationToken);

            if (currentEducation == null) return false;

            currentEducation.Description = education.Description;
            currentEducation.EndDate = education.EndDate;
            currentEducation.Order = education.Order;
            currentEducation.StartDate = education.StartDate;
            currentEducation.Title = education.Title;

            _educationRepository.Update(currentEducation);
            await _educationRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteEducation(long id, CancellationToken cancellationToken)
        {
            Education education = await GetEducationById(id, cancellationToken);

            if (education == null) return false;

            _educationRepository.Delete(education);
            await _educationRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

    }
}
