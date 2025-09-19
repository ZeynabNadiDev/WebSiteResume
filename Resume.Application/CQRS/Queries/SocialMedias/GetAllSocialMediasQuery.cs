using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.SocialMedia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.SocialMedias
{
    public record GetAllSocialMediasQuery() : IRequest<List<SocialMediaViewModel>>;

    public class GetAllSocialMediasQueryHandler
        : IRequestHandler<GetAllSocialMediasQuery, List<SocialMediaViewModel>>
    {
        private readonly ISocialMediaRepository _socialMediaRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllSocialMediasQueryHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper,ICacheService cacheService)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<SocialMediaViewModel>> Handle(GetAllSocialMediasQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "socialmedias:index:all";
            var cachedData = await _cacheService.GetAsync<List<SocialMediaViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var socialMedias = await _socialMediaRepository.GetAllOrderedAsync(cancellationToken);
            var mapped = _mapper.Map<List<SocialMediaViewModel>>(socialMedias);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
