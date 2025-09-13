using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Portfolio;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.Portfolios
{
    public record CreateOrEditPortfolioCommand(CreateOrEditPortfolioViewModel PortfolioVm) : IRequest<bool>;

    public class CreateOrEditPortfolioCommandHandler
        : IRequestHandler<CreateOrEditPortfolioCommand, bool>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateOrEditPortfolioCommandHandler(IPortfolioRepository portfolioRepository, IMapper mapper, IUnitOfWork uow)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateOrEditPortfolioCommand request, CancellationToken cancellationToken)
        {
            if (request.PortfolioVm.Id == 0)
            {
                var newPortfolio = _mapper.Map<Portfolio>(request.PortfolioVm);
                await _portfolioRepository.AddAsync(newPortfolio, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            var currentPortfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioVm.Id, cancellationToken);
            if (currentPortfolio == null) return false;

            _mapper.Map(request.PortfolioVm, currentPortfolio);
            _portfolioRepository.Update(currentPortfolio);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
