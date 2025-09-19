using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerFeedbacks
{
    public record GetCustomerFeedbackByIdQuery(long Id) : IRequest<CustomerFeedback>;
    public class GetCustomerFeedbackByIdQueryHandler : IRequestHandler<GetCustomerFeedbackByIdQuery, CustomerFeedback>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly ICacheService _cacheService;
        public GetCustomerFeedbackByIdQueryHandler(ICustomerFeedbackRepository repository,ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }
        public async Task<CustomerFeedback> Handle(GetCustomerFeedbackByIdQuery request, CancellationToken cancellationToken)
        {    
            // get from redis (cach)
            var cacheKey = $"customerfeedback:{request.Id}";
            var cachedData = await _cacheService.GetAsync<CustomerFeedback>(cacheKey);
            if (cachedData != null)  return cachedData;

            // get from database
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            //save in redis(cach) with 10 minutes 
            if (entity != null)
            {
                await _cacheService.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(10));
            }

            return entity;
           

        }
    }

}
