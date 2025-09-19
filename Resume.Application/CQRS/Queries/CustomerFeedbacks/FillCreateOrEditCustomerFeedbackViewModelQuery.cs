using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerFeedback;
using Resume.Application.Redis.Caching.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Resume.Application.CQRS.Queries.CustomerFeedbacks
{
    public record FillCreateOrEditCustomerFeedbackViewModelQuery(long Id):IRequest<CreateOrEditCustomerFeedbackViewModel>;
    public class FillCreateOrEditCustomerFeedbackViewModelQueryHandler : IRequestHandler<FillCreateOrEditCustomerFeedbackViewModelQuery, CreateOrEditCustomerFeedbackViewModel>
    {
        private readonly ICustomerFeedbackRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrEditCustomerFeedbackViewModelQueryHandler(ICustomerFeedbackRepository repository,
            IMapper mapper,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<CreateOrEditCustomerFeedbackViewModel> Handle
            (FillCreateOrEditCustomerFeedbackViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditCustomerFeedbackViewModel { Id = 0 };

            // get from redis (cach)
            var cacheKey = $"customerfeedback:{request.Id}";
            var cachData=await _cacheService.GetAsync<CreateOrEditCustomerFeedbackViewModel>(cacheKey);
            if (cachData != null) return cachData;

            // get from database
            var customerFeedback = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (customerFeedback == null)
                return new CreateOrEditCustomerFeedbackViewModel { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditCustomerFeedbackViewModel>(customerFeedback);

            //save in redis(cach) with 10 minutes 
            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }

}
