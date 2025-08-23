using Resume.Domain.ViewModels.SocialMedia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface ISocialMediaService
    {
        Task<List<SocialMediaViewModel>> GetAllSocialMedias(CancellationToken cancellationToken);
    }
}
