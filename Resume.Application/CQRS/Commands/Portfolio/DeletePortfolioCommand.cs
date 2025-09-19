using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
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
        private readonly ICacheService _cacheService;

        public DeletePortfolioCommandHandler(IPortfolioRepository portfolioRepository, IUnitOfWork uow,ICacheService cacheService)
        {
            _portfolioRepository = portfolioRepository;
            _uow = uow;
            _cacheService = cacheService;   
        }

        public async Task<bool> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);
            if (portfolio == null) return false;

            _portfolioRepository.Delete(portfolio);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"portfolio:{request.Id}:entity");
            await _cacheService.RemoveAsync("portfolios:index:all");

            return true;
        }
    }
}
