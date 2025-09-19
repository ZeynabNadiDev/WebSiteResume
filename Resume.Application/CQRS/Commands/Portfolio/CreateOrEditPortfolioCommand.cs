using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Portfolio;
using System;
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
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditPortfolioCommandHandler> _logger;

        public CreateOrEditPortfolioCommandHandler(
            IPortfolioRepository portfolioRepository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditPortfolioCommandHandler> logger)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditPortfolioCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrEditPortfolioCommand, Id: {Id}", request.PortfolioVm.Id);

            try
            {
                if (request.PortfolioVm.Id == 0)
                {
                    _logger.LogInformation("Creating new Portfolio");
                    var newPortfolio = _mapper.Map<Portfolio>(request.PortfolioVm);
                    await _portfolioRepository.AddAsync(newPortfolio, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Portfolio created successfully, Id: {Id}", newPortfolio.Id);

                    await _cacheService.RemoveAsync("portfolios:index:all");
                    _logger.LogInformation("Cache invalidated for portfolios:index:all");

                    return true;
                }

                _logger.LogInformation("Editing Portfolio, Id: {Id}", request.PortfolioVm.Id);

                var currentPortfolio = await _portfolioRepository.GetByIdAsync(request.PortfolioVm.Id, cancellationToken);
                if (currentPortfolio == null)
                {
                    _logger.LogWarning("Portfolio not found for Id: {Id}", request.PortfolioVm.Id);
                    return false;
                }

                _mapper.Map(request.PortfolioVm, currentPortfolio);
                _portfolioRepository.Update(currentPortfolio);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Portfolio updated successfully, Id: {Id}", request.PortfolioVm.Id);

                await _cacheService.RemoveAsync($"portfolio:{request.PortfolioVm.Id}:entity");
                await _cacheService.RemoveAsync("portfolios:index:all");
                _logger.LogInformation("Cache invalidated for portfolio:{Id}:entity and portfolios:index:all", request.PortfolioVm.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating or editing Portfolio, Id: {Id}", request.PortfolioVm.Id);
                throw;
            }
        }
    }
}
