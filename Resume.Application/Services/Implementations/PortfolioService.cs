using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Portfolio;
using Resume.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class PortfolioService : IPortfolioService
    {

        #region Constructor PortfolioRepository
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioCategoryRepository _portfolioCategoryRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository, IPortfolioCategoryRepository portfolioCategoryRepository)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioCategoryRepository = portfolioCategoryRepository;
        }
        #endregion


        #region Portfolio

        public async Task<Portfolio> GetPortfolioById(long id,CancellationToken cancellationToken)
        {
            return await _portfolioRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<PortfolioViewModel>> GetAllPortfolios(CancellationToken cancellationToken)
        {
           var portfolios = await _portfolioRepository.GetAllOrderedAsync(cancellationToken);
           return  portfolios.Select(p => new PortfolioViewModel()
                 {
                    Id = p.Id,
                    Image = p.Image,
                    ImageAlt = p.ImageAlt,
                    Link = p.Link,
                    Order = p.Order,
                    PortfolioCategoryName = p.PortfolioCategory.Name,
                    Title = p.Title
                })
                .ToList();

          
        }

        public async Task<CreateOrEditPortfolioViewModel> FillCreateOrEditPortfolioViewModel(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditPortfolioViewModel() {
                Id = 0,
                PortfolioCategories = await GetAllPortfolioCategories(cancellationToken)
            };

            Portfolio portfolio = await GetPortfolioById(id,cancellationToken);

            if (portfolio == null) return new CreateOrEditPortfolioViewModel() {
                Id = 0,
                PortfolioCategories = await GetAllPortfolioCategories(cancellationToken)
            };

            return new CreateOrEditPortfolioViewModel()
            {
                Id = portfolio.Id,
                Image = portfolio.Image,
                ImageAlt = portfolio.ImageAlt,
                Link = portfolio.Link,
                Order = portfolio.Order,
                Title = portfolio.Title,
                PortfolioCategoryId = portfolio.PortfolioCategoryId,
                PortfolioCategories = await GetAllPortfolioCategories(cancellationToken)
            };

        }

        public async Task<bool> CreateOrEditPortfolio(CreateOrEditPortfolioViewModel portfolio,CancellationToken cancellationToken)
        {
            if (portfolio.Id == 0)
            {
                var newPortfolio = new Portfolio()
                {
                    Image = portfolio.Image,
                    ImageAlt = portfolio.ImageAlt,
                    Link = portfolio.Link,
                    Order = portfolio.Order,
                    Title = portfolio.Title,
                    PortfolioCategoryId = portfolio.PortfolioCategoryId,
                };
                await _portfolioRepository.AddAsync(newPortfolio,cancellationToken);
                await _portfolioRepository.SaveChangeAsync(cancellationToken);
                return true;
            }

            Portfolio currnetPortfolio = await GetPortfolioById(portfolio.Id,cancellationToken);
            if (currnetPortfolio == null) return false;

            currnetPortfolio.Image = portfolio.Image;
            currnetPortfolio.ImageAlt = portfolio.ImageAlt;
            currnetPortfolio.Link = portfolio.Link;
            currnetPortfolio.Order = portfolio.Order;
            currnetPortfolio.Title = portfolio.Title;
            currnetPortfolio.PortfolioCategoryId = portfolio.PortfolioCategoryId;

            _portfolioRepository.Update(currnetPortfolio);
            await _portfolioRepository.SaveChangeAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeletePortfolio(long id, CancellationToken cancellationToken)
        {
            Portfolio portfolio = await GetPortfolioById(id, cancellationToken);

            if (portfolio == null) return false;

            _portfolioRepository.Delete(portfolio);
            await _portfolioRepository.SaveChangeAsync(cancellationToken);
            return true;
        }

        #endregion


        #region Portfolio Category
        public async Task<PortfolioCategory> GetPortfolioCategoryById(long id,CancellationToken cancellationToken)
        {
            return await _portfolioCategoryRepository.GetByIdAsync(id,cancellationToken);
        }

        public async Task<List<PortfolioCategoryViewModel>> GetAllPortfolioCategories(CancellationToken cancellationToken)
        {
            var portfolioCategories = await _portfolioCategoryRepository.GetAllOrderedAsync(cancellationToken);
              return portfolioCategories.Select(pc => new PortfolioCategoryViewModel()
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    Order = pc.Order,
                    Title = pc.Title
                })
                .ToList();

           
        }

        public async Task<CreateOrEditPortfolioCategoryViewModel> FillCreateOrEditPortfolioCategoryViewModel(long id,CancellationToken cancellationToken)
        {
            if (id == 0) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };

            PortfolioCategory portfolioCategory = await GetPortfolioCategoryById(id,cancellationToken);

            if (portfolioCategory == null) return new CreateOrEditPortfolioCategoryViewModel() { Id = 0 };

            return new CreateOrEditPortfolioCategoryViewModel()
            {
                Id = portfolioCategory.Id,
                Name = portfolioCategory.Name,
                Order = portfolioCategory.Order,
                Title = portfolioCategory.Title
            };
        }

        public async Task<bool> CreateOrEditPortfolioCategory(CreateOrEditPortfolioCategoryViewModel portfolioCategory,CancellationToken cancellationToken)
        {

            if (portfolioCategory.Id == 0)
            {
                var newPortfolioCategory = new PortfolioCategory()
                {
                    Name = portfolioCategory.Name,
                    Order = portfolioCategory.Order,
                    Title = portfolioCategory.Title
                };

                await _portfolioCategoryRepository.AddAsync(newPortfolioCategory, cancellationToken);
                await _portfolioCategoryRepository.SaveChangeAsync(cancellationToken);
                return true;
            }

            PortfolioCategory currentPortfolioCategory = await GetPortfolioCategoryById(portfolioCategory.Id,cancellationToken);

            if (currentPortfolioCategory == null) return false;

            currentPortfolioCategory.Name = portfolioCategory.Name;
            currentPortfolioCategory.Order = portfolioCategory.Order;
            currentPortfolioCategory.Title = portfolioCategory.Title;

            _portfolioCategoryRepository.Update(currentPortfolioCategory);
            await _portfolioCategoryRepository.SaveChangeAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeletePortfolioCategory(long id, CancellationToken cancellationToken)
        {
            PortfolioCategory portfolioCategory = await GetPortfolioCategoryById(id, cancellationToken);

            if (portfolioCategory == null) return false;

           _portfolioCategoryRepository.Delete(portfolioCategory);
            await _portfolioCategoryRepository.SaveChangeAsync(cancellationToken);

            return true;
        }




        #endregion


    }
}
