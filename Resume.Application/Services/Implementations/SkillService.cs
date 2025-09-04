using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Skill;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class SkillService : ISkillService
    {

        #region Constructor SkillRepository
        private readonly ISkillRepository _skillRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public SkillService(ISkillRepository skillRepository,IMapper mapper,IUnitOfWork unit)
        {
            _skillRepository = skillRepository;
            _mapper = mapper;
            _uow = unit;
        }
        #endregion

        public async Task<Skill> GetSkillByIdAsync(long id,CancellationToken cancellationToken)
        {
            return await _skillRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<SkillViewModel>> GetAllSkillsAsync(CancellationToken cancellationToken)
        {
            var skills = await _skillRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<SkillViewModel>>(skills);

        }

        public async Task<CreateOrEditSkillViewModel> FillCreateOrEditSkillViewModelAsync(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditSkillViewModel() { Id = 0 };

            Skill skill = await GetSkillByIdAsync(id,cancellationToken);

            if (skill == null) return new CreateOrEditSkillViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditSkillViewModel>(skill);
        }

        public async Task<bool> CreateOrEditSkillAsync(CreateOrEditSkillViewModel skill,CancellationToken cancellationToken)
        {
            if (skill.Id == 0)
            {
                var newSkill = _mapper.Map<Skill>(skill);

                await _skillRepository.AddAsync(newSkill,cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            Skill currentSkill = await GetSkillByIdAsync(skill.Id,cancellationToken);

            if (currentSkill == null) return false;

            _mapper.Map(skill, currentSkill);
            _skillRepository.Update(currentSkill);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteSkillAsync(long id,CancellationToken cancellationToken)
        {
            Skill skill = await GetSkillByIdAsync(id,cancellationToken);

            if (skill == null) return false;

            _skillRepository.Delete(skill);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }


    }
}
