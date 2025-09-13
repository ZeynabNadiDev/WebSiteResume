using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Information;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record GetInformationQuery() : IRequest<InformationViewModel>;
    public class GetInformationQueryHandler : IRequestHandler<GetInformationQuery, InformationViewModel>
    {
        private readonly IInformationRepository _repository;
        private readonly IMapper _mapper;

        public GetInformationQueryHandler(IInformationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<InformationViewModel> Handle(GetInformationQuery request, CancellationToken cancellationToken)
        {
            var info = await _repository.GetEntities()
                .ProjectTo<InformationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return info ?? new InformationViewModel();
        }
    }
}
