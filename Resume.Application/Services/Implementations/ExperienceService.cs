using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Education;
using Resume.Domain.ViewModels.Experience;

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
        private readonly IMapper _mapper;
        public ExperienceService(IExperienceRepository experienceRepository,IMapper mapper)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
        }
        #endregion


        public async Task<List<ExperienceViewModel>> GetAllExperiences(CancellationToken cancellationToken)
        {
            var experiences = await _experienceRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<ExperienceViewModel>>(experiences);
        }
    }
}
