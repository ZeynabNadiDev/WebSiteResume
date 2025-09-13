using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Portfolios
{
    public record GetPortfolioByIdQuery(long Id) : IRequest<Portfolio?>;

    public class GetPortfolioByIdQueryHandler
        : IRequestHandler<GetPortfolioByIdQuery, Portfolio?>
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public GetPortfolioByIdQueryHandler(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public Task<Portfolio?> Handle(GetPortfolioByIdQuery request, CancellationToken cancellationToken)
        {
            return _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }

}
