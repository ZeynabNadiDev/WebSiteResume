using AutoMapper;
using MediatR;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.SocialMedia;
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

        public GetAllSocialMediasQueryHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }

        public async Task<List<SocialMediaViewModel>> Handle(GetAllSocialMediasQuery request, CancellationToken cancellationToken)
        {
            var socialMedias = await _socialMediaRepository.GetAllOrderedAsync(cancellationToken);
            return _mapper.Map<List<SocialMediaViewModel>>(socialMedias);
        }
    }
}
