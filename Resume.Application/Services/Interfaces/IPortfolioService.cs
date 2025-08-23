using Resume.Domain.Entity;
using Resume.Domain.ViewModels.Portfolio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IPortfolioService
    {
        #region Portfolio
        Task<Portfolio> GetPortfolioById(long id,CancellationToken cancellationToken);
        Task<List<PortfolioViewModel>> GetAllPortfolios(CancellationToken cancellationToken);
        Task<CreateOrEditPortfolioViewModel> FillCreateOrEditPortfolioViewModel(long id,CancellationToken cancellationToken);
        Task<bool> CreateOrEditPortfolio(CreateOrEditPortfolioViewModel portfolio,CancellationToken cancellationToken);
        Task<bool> DeletePortfolio(long id, CancellationToken cancellationToken);
        #endregion


        #region Portfolio Category
        Task<PortfolioCategory> GetPortfolioCategoryById(long id, CancellationToken cancellationToken);
        Task<List<PortfolioCategoryViewModel>> GetAllPortfolioCategories(CancellationToken cancellationToken);
        Task<CreateOrEditPortfolioCategoryViewModel> FillCreateOrEditPortfolioCategoryViewModel(long id, CancellationToken cancellationToken);
        Task<bool> CreateOrEditPortfolioCategory(CreateOrEditPortfolioCategoryViewModel portfolioCategory, CancellationToken cancellationToken);
        Task<bool> DeletePortfolioCategory(long id, CancellationToken cancellationToken);
        #endregion


    }
}
