using AutoMapper;
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
        private readonly IMapper _mapper;

        public SocialMediaService(ISocialMediaRepository socialMediaRepository,IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }
        #endregion

        public async Task<List<SocialMediaViewModel>> GetAllSocialMedias(CancellationToken cancellationToken)
        {
            var socialMedias = await _socialMediaRepository.GetAllOrderedAsync(cancellationToken);

            return _mapper.Map<List<SocialMediaViewModel>>(socialMedias);


        }



    }
}
