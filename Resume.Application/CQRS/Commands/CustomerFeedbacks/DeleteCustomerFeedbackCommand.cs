using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.CustomerFeedbacks
{
    public record DeleteCustomerFeedbackCommand(long Id):IRequest<bool>;
    public class DeleteCustomerFeedbackCommandHandler : IRequestHandler<DeleteCustomerFeedbackCommand, bool>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteCustomerFeedbackCommandHandler> _logger;

        public DeleteCustomerFeedbackCommandHandler(ICustomerFeedbackRepository repository, 
            IUnitOfWork uow,ICacheService cacheService,
             ILogger<DeleteCustomerFeedbackCommandHandler> logger)
        {
            _repository = repository;
            _uow = uow;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCustomerFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCustomerFeedbackCommand, Id: {Id}", request.Id);

            try
            {
                var customerFeedback = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customerFeedback == null)
                {
                    _logger.LogWarning("CustomerFeedback not found for Id: {Id}", request.Id);
                    return false;
                }

                _repository.Delete(customerFeedback);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Deleted CustomerFeedback successfully, Id: {Id}", request.Id);

                // Cache Invalidation
            await _cacheService.RemoveAsync($"customerfeedback:{request.Id}:entity");
            await _cacheService.RemoveAsync("customerfeedbacks:index:all");

                _logger.LogInformation("Cache invalidated for CustomerFeedback Id: {Id}", request.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting CustomerFeedback, Id: {Id}", request.Id);
                throw;
            }

        }
    }

}
