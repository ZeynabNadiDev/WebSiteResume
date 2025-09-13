using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record GetAllPortfolioCategoriesQuery() : IRequest<List<PortfolioCategoryViewModel>>;

    public class GetAllPortfolioCategoriesQueryHandler
        : IRequestHandler<GetAllPortfolioCategoriesQuery, List<PortfolioCategoryViewModel>>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;

        public GetAllPortfolioCategoriesQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository, IMapper mapper)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
        }

        public async Task<List<PortfolioCategoryViewModel>> Handle(GetAllPortfolioCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _portfolioCategoryRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<PortfolioCategoryViewModel>>(categories);
        }
    }
}
