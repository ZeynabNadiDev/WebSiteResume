using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.CustomerLogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.CustomerLogos
{
     public record GetCustomerLogosForIndexPageQuery():IRequest<List<CustomerLogoListViewModel>>;
    public class GetCustomerLogosForIndexPageQueryHandler
        : IRequestHandler<GetCustomerLogosForIndexPageQuery, List<CustomerLogoListViewModel>>
    {
        private readonly ICustomerLogoRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomerLogosForIndexPageQueryHandler(
            ICustomerLogoRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CustomerLogoListViewModel>>
            Handle(GetCustomerLogosForIndexPageQuery request, CancellationToken cancellationToken)
        {
            var logos = await _repository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<CustomerLogoListViewModel>>(logos);
        }
    }

}
