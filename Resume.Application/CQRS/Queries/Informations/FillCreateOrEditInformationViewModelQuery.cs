using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record FillCreateOrEditInformationViewModelQuery():IRequest<CreateOrEditInformationViewModel>;
    public class FillCreateOrEditInformationViewModelQueryHandler : IRequestHandler<FillCreateOrEditInformationViewModelQuery, CreateOrEditInformationViewModel>
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;

        public FillCreateOrEditInformationViewModelQueryHandler(IInformationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateOrEditInformationViewModel> Handle(FillCreateOrEditInformationViewModelQuery request, CancellationToken cancellationToken)
        {
            var info = await _repository.GetSingleAsync(cancellationToken);
            if (info == null)
                return new CreateOrEditInformationViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditInformationViewModel>(info);
        }
    }

}
