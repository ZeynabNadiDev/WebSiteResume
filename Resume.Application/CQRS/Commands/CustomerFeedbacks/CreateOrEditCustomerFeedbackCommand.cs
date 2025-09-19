using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.CustomerFeedback;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.CustomerFeedbacks
{
    public record CreateOrEditCustomerFeedbackCommand(CreateOrEditCustomerFeedbackViewModel Model)
        :IRequest<bool>;
    public class CreateOrEditCustomerFeedbackCommandHandler : IRequestHandler<CreateOrEditCustomerFeedbackCommand, bool>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateOrEditCustomerFeedbackCommandHandler> _logger;

        public CreateOrEditCustomerFeedbackCommandHandler(
            ICustomerFeedbackRepository repository,
            IUnitOfWork uow,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<CreateOrEditCustomerFeedbackCommandHandler> logger)
        {
            _repository = repository;
            _uow = uow;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateOrEditCustomerFeedbackCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;

            _logger.LogInformation("Handling CreateOrEditCustomerFeedbackCommand, Model Id: {Id}", model.Id);

            try
            {
                if (model.Id == 0)
            {
                _logger.LogInformation("Creating new CustomerFeedback");
                var newCustomerFeedback = _mapper.Map<CustomerFeedback>(model);
                await _repository.AddAsync(newCustomerFeedback, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                await _cacheService.RemoveAsync("customerfeedbacks:index:all");
                _logger.LogInformation("Created new CustomerFeedback successfully, Id: {NewId}", newCustomerFeedback.Id);
                return true;
            }

            _logger.LogInformation("Editing CustomerFeedback, Id: {Id}", model.Id);
            var currentCustomerFeedback = await _repository.GetByIdAsync(model.Id, cancellationToken);

            if (currentCustomerFeedback == null)
                {
                    _logger.LogWarning("CustomerFeedback not found for Id: {Id}", model.Id);
                    return false;
                }
              

            _mapper.Map(model, currentCustomerFeedback);
            _repository.Update(currentCustomerFeedback);
            await _uow.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"customerfeedback:{model.Id}:entity");
            await _cacheService.RemoveAsync("customerfeedbacks:index:all");

           _logger.LogInformation("Edited CustomerFeedback successfully, Id: {Id}", model.Id);
            return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling CreateOrEditCustomerFeedbackCommand with Id: {Id}", model.Id);
                throw;
            }
        }
    }

}
