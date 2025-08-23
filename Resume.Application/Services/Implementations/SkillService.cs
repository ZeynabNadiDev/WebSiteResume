using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Skill;
using Resume.Infra.Data.Context;
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
        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        #endregion

        public async Task<Skill> GetSkillByIdAsync(long id,CancellationToken cancellationToken)
        {
            return await _skillRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<SkillViewModel>> GetAllSkillsAsync(CancellationToken cancellationToken)
        {
            var skills = await _skillRepository.GetAllOrderedAsync(cancellationToken);
              return skills.Select(s => new SkillViewModel()
                {
                    Id = s.Id,
                    Order = s.Order,
                    Percent = s.Percent,
                    Title = s.Title
                })
                 .ToList();

        }

        public async Task<CreateOrEditSkillViewModel> FillCreateOrEditSkillViewModelAsync(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditSkillViewModel() { Id = 0 };

            Skill skill = await GetSkillByIdAsync(id,cancellationToken);

            if (skill == null) return new CreateOrEditSkillViewModel() { Id = 0 };

            return new CreateOrEditSkillViewModel()
            {
                Id = skill.Id,
                Order = skill.Order,
                Percent = skill.Percent,
                Title = skill.Title
            };
        }

        public async Task<bool> CreateOrEditSkillAsync(CreateOrEditSkillViewModel skill,CancellationToken cancellationToken)
        {
            if (skill.Id == 0)
            {
                var newSkill = new Skill()
                {
                    Id = skill.Id,
                    Order = skill.Order,
                    Percent = skill.Percent,
                    Title = skill.Title
                };

                await _skillRepository.AddAsync(newSkill,cancellationToken);
                await _skillRepository.SaveChangeAsync(cancellationToken);
                return true;
            }

            Skill currentSkill = await GetSkillByIdAsync(skill.Id,cancellationToken);

            if (currentSkill == null) return false;

            currentSkill.Order = skill.Order;
            currentSkill.Percent = skill.Percent;
            currentSkill.Title = skill.Title;

            _skillRepository.Update(currentSkill);
            await _skillRepository.SaveChangeAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteSkillAsync(long id,CancellationToken cancellationToken)
        {
            Skill skill = await GetSkillByIdAsync(id,cancellationToken);

            if (skill == null) return false;

            _skillRepository.Delete(skill);
            await _skillRepository.SaveChangeAsync(cancellationToken);
            return true;
        }


    }
}
