using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Experience;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Experiences
{
     public record GetAllExperiencesQuery():IRequest<List<ExperienceViewModel>>;
    public class GetAllExperiencesQueryHandler : IRequestHandler<GetAllExperiencesQuery, List<ExperienceViewModel>>
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly IMapper _mapper;

        public GetAllExperiencesQueryHandler(
            IExperienceRepository experienceRepository,
            IMapper mapper)
        {
            _experienceRepository = experienceRepository;
            _mapper = mapper;
        }

        public async Task<List<ExperienceViewModel>> Handle(
            GetAllExperiencesQuery request,
            CancellationToken cancellationToken)
        {
            var experience = await _experienceRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<ExperienceViewModel>>(experience);
        }
    }

}
