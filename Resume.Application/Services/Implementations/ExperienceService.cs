using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Experience;
using Resume.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class ExperienceService : IExperienceService
    {

        #region Constructor ExperienceRepository
        private readonly IExperienceRepository _experienceRepository;
        public ExperienceService(IExperienceRepository experienceRepository)
        {
            _experienceRepository = experienceRepository;
        }
        #endregion


        public async Task<List<ExperienceViewModel>> GetAllExperiences(CancellationToken cancellationToken)
        {
            var experiences = await _experienceRepository.GetAllOrderedAsync(cancellationToken); 
                  return experiences.Select(c => new ExperienceViewModel()
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
    }
}
