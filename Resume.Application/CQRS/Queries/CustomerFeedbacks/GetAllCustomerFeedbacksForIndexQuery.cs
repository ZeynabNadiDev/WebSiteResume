using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerFeedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerFeedbacks
{
    public record GetAllCustomerFeedbacksForIndexQuery():IRequest<List<CustomerFeedbackViewModel>>;
    public class GetAllCustomerFeedbacksForIndexQueryHandler : IRequestHandler<GetAllCustomerFeedbacksForIndexQuery, List<CustomerFeedbackViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerFeedbackRepository _repository;
        private readonly ICacheService _cacheService;

        public GetAllCustomerFeedbacksForIndexQueryHandler(IMapper mapper,
            ICustomerFeedbackRepository repository,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;   
        }
        public async Task<List<CustomerFeedbackViewModel>> Handle
            (GetAllCustomerFeedbacksForIndexQuery request, CancellationToken cancellationToken)
        {
            // get from redis (cach)
            var cacheKey = "customerfeedbacks:index:all";
            var cachedData = await _cacheService.GetAsync<List<CustomerFeedbackViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            // get from database
            var customerFeedbacks = await _repository.GetAllOrderedAsync(cancellationToken);

            var mapped= _mapper.Map<List<CustomerFeedbackViewModel>>(customerFeedbacks);

            //save in redis(cach) with 10 minutes 
            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;

        }

    }
}
