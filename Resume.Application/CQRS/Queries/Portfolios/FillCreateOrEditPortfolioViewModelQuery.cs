using AutoMapper;
using MediatR;
using Resume.Application.CQRS.Queries.PortfolioCategories;
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
    public record FillCreateOrEditPortfolioViewModelQuery(long Id):IRequest<CreateOrEditPortfolioViewModel>;
    public class FillCreateOrEditPortfolioViewModelQueryHandler
       : IRequestHandler<FillCreateOrEditPortfolioViewModelQuery, CreateOrEditPortfolioViewModel>
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FillCreateOrEditPortfolioViewModelQueryHandler(IPortfolioRepository portfolioRepository, IMediator mediator, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<CreateOrEditPortfolioViewModel> Handle(FillCreateOrEditPortfolioViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                return new CreateOrEditPortfolioViewModel
                {
                    Id = 0,
                    PortfolioCategories = await _mediator.Send(new GetAllPortfolioCategoriesQuery(), cancellationToken)
                };
            }

            var portfolio = await _portfolioRepository.GetByIdAsync(request.Id, cancellationToken);

            if (portfolio == null)
            {
                return new CreateOrEditPortfolioViewModel
                {
                    Id = 0,
                    PortfolioCategories = await _mediator.Send(new GetAllPortfolioCategoriesQuery(), cancellationToken)
                };
            }

            var vm = _mapper.Map<CreateOrEditPortfolioViewModel>(portfolio);
            vm.PortfolioCategories = await _mediator.Send(new GetAllPortfolioCategoriesQuery(), cancellationToken);
            return vm;
        }
    }

}
