using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.SocialMedia;
using Resume.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class SocialMediaService : ISocialMediaService
    {

        #region Constructor SocialMediaRepository
       private readonly ISocialMediaRepository _socialMediaRepository;

        public SocialMediaService(ISocialMediaRepository socialMediaRepository)
        {
            _socialMediaRepository = socialMediaRepository;
        }
        #endregion

        public async Task<List<SocialMediaViewModel>> GetAllSocialMedias(CancellationToken cancellationToken)
        {
            var socialMedias = await _socialMediaRepository.GetAllOrderedAsync(cancellationToken);

            return socialMedias.Select(s => new SocialMediaViewModel()
                {
                    Id = s.Id,
                    Icon = s.Icon,
                    Order = s.Order,
                    Link = s.Link
                })
                .ToList();

            
        }



    }
}
