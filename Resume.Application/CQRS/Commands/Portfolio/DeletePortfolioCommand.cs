using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Portfolios
{
    public record DeletePortfolioCommand(long Id) : IRequest<bool>;

    public class DeletePortfolioCommandHandler
        : IRequestHandler<DeletePortfolioCommand, bool>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IUnitOfWork _uow;

        public DeletePortfolioCommandHandler(IPortfolioRepository portfolioRepository, IUnitOfWork uow)
        {
            _portfolioRepository = portfolioRepository;
            _uow = uow;
        }

        public async Task<bool> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);
            if (portfolio == null) return false;

            _portfolioRepository.Delete(portfolio);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
