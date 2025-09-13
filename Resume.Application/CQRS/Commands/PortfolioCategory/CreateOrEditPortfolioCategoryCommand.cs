using AutoMapper;
using MediatR;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Portfolio;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Commands.PortfolioCategories
{
    public record CreateOrEditPortfolioCategoryCommand(CreateOrEditPortfolioCategoryViewModel PortfolioCategoryVm) : IRequest<bool>;

    public class CreateOrEditPortfolioCategoryCommandHandler
        : IRequestHandler<CreateOrEditPortfolioCategoryCommand, bool>
    {
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CreateOrEditPortfolioCategoryCommandHandler(IPortfolioCategoryRepository portfolioCategoryRepository, IMapper mapper, IUnitOfWork uow)
        {
            _portfolioCategoryRepository = portfolioCategoryRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> Handle(CreateOrEditPortfolioCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.PortfolioCategoryVm.Id == 0)
            {
                var newCategory = _mapper.Map<PortfolioCategory>(request.PortfolioCategoryVm);
                await _portfolioCategoryRepository.AddAsync(newCategory, cancellationToken);
                await _uow.SaveChangesAsync(cancellationToken);
                return true;
            }

            var currentCategory = await _portfolioCategoryRepository.GetByIdAsync(request.PortfolioCategoryVm.Id, cancellationToken);
            if (currentCategory == null) return false;

            _mapper.Map(request.PortfolioCategoryVm, currentCategory);
            _portfolioCategoryRepository.Update(currentCategory);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
