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

namespace Resume.Application.CQRS.Commands.PortfolioCategories
{
    public record CreateOrEditPortfolioCategoryCommand(CreateOrEditPortfolioCategoryViewModel PortfolioCategoryVm) : IRequest<bool>;

    public class CreateOrEditPortfolioCategoryCommandHandler
        : IRequestHandler<CreateOrEditPortfolioCategoryCommand, bool>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditPortfolioCategoryCommandHandler> _logger;

        public CreateOrEditPortfolioCategoryCommandHandler(
            IPortfolioCategoryRepository portfolioCategoryRepository,
            IMapper mapper,
            IUnitOfWork uow,
            ICacheService cacheService,
            ILogger<CreateOrEditPortfolioCategoryCommandHandler> logger)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditPortfolioCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateOrEditPortfolioCategoryCommand, Id: {Id}", request.PortfolioCategoryVm.Id);

            try
            {
                if (request.PortfolioCategoryVm.Id == 0)
                {
                    _logger.LogInformation("Creating new PortfolioCategory");
                    var newCategory = _mapper.Map<PortfolioCategory>(request.PortfolioCategoryVm);
                    await _portfolioCategoryRepository.AddAsync(newCategory, cancellationToken);
                    await _uow.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("PortfolioCategory created successfully, Id: {Id}", newCategory.Id);

                    await _cacheService.RemoveAsync("portfoliocategories:index:all");
                    _logger.LogInformation("Cache invalidated for portfoliocategories:index:all");

                    return true;
                }

                _logger.LogInformation("Editing PortfolioCategory, Id: {Id}", request.PortfolioCategoryVm.Id);
                var currentCategory = await _portfolioCategoryRepository.GetByIdAsync(request.PortfolioCategoryVm.Id, cancellationToken);
                if (currentCategory == null)
                {
                    _logger.LogWarning("PortfolioCategory not found for Id: {Id}", request.PortfolioCategoryVm.Id);
                    return false;
                }

                _mapper.Map(request.PortfolioCategoryVm, currentCategory);
                _portfolioCategoryRepository.Update(currentCategory);
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("PortfolioCategory updated successfully, Id: {Id}", request.PortfolioCategoryVm.Id);

                await _cacheService.RemoveAsync($"portfoliocategory:{request.PortfolioCategoryVm.Id}:entity");
                await _cacheService.RemoveAsync("portfoliocategories:index:all");
                _logger.LogInformation("Cache invalidated for portfoliocategory:{Id}:entity and portfoliocategories:index:all", request.PortfolioCategoryVm.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating or editing PortfolioCategory, Id: {Id}", request.PortfolioCategoryVm.Id);
                throw;
            }
        }
    }
}
