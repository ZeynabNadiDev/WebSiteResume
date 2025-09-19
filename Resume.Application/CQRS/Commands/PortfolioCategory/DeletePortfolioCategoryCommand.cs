using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.PortfolioCategories
{
    public record DeletePortfolioCategoryCommand(long Id) : IRequest<bool>;

    public class DeletePortfolioCategoryCommandHandler
        : IRequestHandler<DeletePortfolioCategoryCommand, bool>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public DeletePortfolioCategoryCommandHandler(IPortfolioCategoryRepository portfolioCategoryRepository, 
            IUnitOfWork uow, ICacheService cacheService )
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _uow = uow;
            _cacheService = cacheService;
        }

        public async Task<bool> Handle
            (DeletePortfolioCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null) return false;

            _portfolioCategoryRepository.Delete(category);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"portfoliocategory:{request.Id}:entity");
            await _cacheService.RemoveAsync("portfoliocategories:index:all");

            return true;
        }
    }
}
