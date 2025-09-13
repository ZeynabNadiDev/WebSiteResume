using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Portfolios
{
    public record GetAllPortfoliosQuery() : IRequest<List<PortfolioViewModel>>;
    public class GetAllPortfoliosQueryHandler
       : IRequestHandler<GetAllPortfoliosQuery, List<PortfolioViewModel>>
    {
        private readonly IPortfolioRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPortfoliosQueryHandler(IPortfolioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<PortfolioViewModel>> Handle(GetAllPortfoliosQuery request,CancellationToken cancellationToken)
        {
            var portfolios=await _repository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<PortfolioViewModel>>(portfolios);
        }

    }
}
