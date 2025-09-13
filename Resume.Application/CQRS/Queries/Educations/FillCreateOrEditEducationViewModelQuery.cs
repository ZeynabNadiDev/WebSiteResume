using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
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
    public record FillCreateOrEditEducationViewModelQuery(long Id) : IRequest<CreateOrEditEducationViewModel>;
    public class FillCreateOrEditEducationViewModelHandler
       : IRequestHandler<FillCreateOrEditEducationViewModelQuery, CreateOrEditEducationViewModel>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;

        public FillCreateOrEditEducationViewModelHandler(IEducationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreateOrEditEducationViewModel> Handle(FillCreateOrEditEducationViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditEducationViewModel() { Id = 0 };

            Education education = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (education == null)
                return new CreateOrEditEducationViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditEducationViewModel>(education);
        }
    }
}
