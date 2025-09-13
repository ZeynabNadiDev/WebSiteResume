using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record GetPortfolioCategoryByIdQuery(long Id) : IRequest<PortfolioCategory?>;

    public class GetPortfolioCategoryByIdQueryHandler
        : IRequestHandler<GetPortfolioCategoryByIdQuery, PortfolioCategory?>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;

        public GetPortfolioCategoryByIdQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
        }

        public Task<PortfolioCategory?> Handle(GetPortfolioCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
