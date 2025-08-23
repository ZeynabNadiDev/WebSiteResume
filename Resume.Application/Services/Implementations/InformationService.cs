using Microsoft.EntityFrameworkCore;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Information;
using Resume.Infra.Data.Context;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class InformationService : IInformationService
    {

        #region Constructor InformationRepository
       private IInformationRepository _informationRepository;
        public InformationService(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }


        #endregion

        public async Task<InformationViewModel> GetInformation(CancellationToken cancellationToken)
        {
            InformationViewModel information = await _informationRepository.GetEntities()
                .Select(i => new InformationViewModel()
                {
                    Address = i.Address,
                    Avatar = i.Avatar,
                    DateOfBirth = i.DateOfBirth,
                    Email = i.Email,
                    Id = i.Id,
                    Job = i.Job,
                    Name = i.Name,
                    Phone = i.Phone,
                    ResumeFile = i.ResumeFile,
                    MapSrc = i.MapSrc
                })
                .FirstOrDefaultAsync();


            if (information == null)
            {
                return new InformationViewModel();
            }

            return information;
        }

        public async Task<Information?> GetInformationModel(CancellationToken cancellationToken)
        {
            return await _informationRepository.GetSingleAsync(cancellationToken);
        }

        public async Task<CreateOrEditInformationViewModel> FillCreateOrEditInformationViewModel(CancellationToken cancellationToken)
        {
            Information information = await GetInformationModel(cancellationToken);

            if (information == null) return new CreateOrEditInformationViewModel() { Id = 0 };

            return new CreateOrEditInformationViewModel() {
            Id = information.Id,
            Address = information.Address,
            Avatar = information.Avatar,
            DateOfBirth = information.DateOfBirth,
            Email = information.Email,
            Job = information.Job,
            MapSrc = information.MapSrc,
            Name = information.Name,
            Phone = information.Phone,
            ResumeFile = information.ResumeFile
            };

        }

        public async Task<bool> CreateOrEditInformation(CreateOrEditInformationViewModel information,CancellationToken cancellationToken)
        {
            if (information.Id == 0)
            {
                var newInformation = new Information() {
                    Address = information.Address,
                    Avatar = information.Avatar,
                    DateOfBirth = information.DateOfBirth,
                    Email = information.Email,
                    Job = information.Job,
                    MapSrc = information.MapSrc,
                    Name = information.Name,
                    Phone = information.Phone,
                    ResumeFile = information.ResumeFile
                };

                await _informationRepository.AddAsync(newInformation,cancellationToken);
                await _informationRepository.SaveChangeAsync(cancellationToken);
                return true;
            }

            Information currentInformation = await GetInformationModel(cancellationToken);

            currentInformation.Address = information.Address;
            currentInformation.Avatar = information.Avatar;
            currentInformation.DateOfBirth = information.DateOfBirth;
            currentInformation.Email = information.Email;
            currentInformation.Job = information.Job;
            currentInformation.MapSrc = information.MapSrc;
            currentInformation.Name = information.Name;
            currentInformation.Phone = information.Phone;
            currentInformation.ResumeFile = information.ResumeFile;

           _informationRepository.Update(currentInformation);
            await _informationRepository.SaveChangeAsync(cancellationToken);
            return true;

        }


    }
}
