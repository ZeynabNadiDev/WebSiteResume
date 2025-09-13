using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Educations
{
    public record GetAllEducationsQuery() : IRequest<List<EducationViewModel>>;
    public class GetAllEducationsHandler : IRequestHandler<GetAllEducationsQuery, List<EducationViewModel>>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;

        public GetAllEducationsHandler(IEducationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<EducationViewModel>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
        {
            var educations = await _repo.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<EducationViewModel>>(educations);
        }
    }
}
