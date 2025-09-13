using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Skills
{
    public record CreateOrEditSkillCommand(CreateOrEditSkillViewModel Skill) : IRequest<bool>;
    public class CreateOrEditSkillHandler : IRequestHandler<CreateOrEditSkillCommand, bool>
    {
        private readonly ISkillRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateOrEditSkillHandler(ISkillRepository repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateOrEditSkillCommand request, CancellationToken cancellationToken)
        {
            if (request.Skill.Id == 0) // Create
            {
                var newSkill = _mapper.Map<Skill>(request.Skill);
                await _repo.AddAsync(newSkill, cancellationToken);
            }
            else // Update
            {
                var existingSkill = await _repo.GetByIdAsync(request.Skill.Id, cancellationToken);
                if (existingSkill == null)
                    return false;

                _mapper.Map(request.Skill, existingSkill);
                _repo.Update(existingSkill);
            }

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

}

