using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Informations
{
    public record GetInformationModelQuery() : IRequest<Information?>;
    public class GetInformationModelQueryHandler : IRequestHandler<GetInformationModelQuery, Information?>
    {
        private readonly IInformationRepository _repository;

        public GetInformationModelQueryHandler(IInformationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Information?> Handle(GetInformationModelQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetSingleAsync(cancellationToken);
        }
    }
}
