using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.PortfolioCategories
{
    public record FillCreateOrEditPortfolioCategoryViewModelQuery(long Id) : IRequest<CreateOrEditPortfolioCategoryViewModel>;

    public class FillCreateOrEditPortfolioCategoryViewModelQueryHandler
        : IRequestHandler<FillCreateOrEditPortfolioCategoryViewModelQuery, CreateOrEditPortfolioCategoryViewModel>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;

        public FillCreateOrEditPortfolioCategoryViewModelQueryHandler(IPortfolioCategoryRepository portfolioCategoryRepository, IMapper mapper)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
        }

        public async Task<CreateOrEditPortfolioCategoryViewModel> Handle(FillCreateOrEditPortfolioCategoryViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };

            var category = await _portfolioCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (category == null) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };

            return _mapper.Map<CreateOrEditPortfolioCategoryViewModel>(category);
        }
    }
}
