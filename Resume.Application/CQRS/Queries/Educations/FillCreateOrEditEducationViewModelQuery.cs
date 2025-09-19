using AutoMapper;
using MediatR;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Educations
{
    public record FillCreateOrEditEducationViewModelQuery(long Id) : IRequest<CreateOrEditEducationViewModel>;
    public class FillCreateOrEditEducationViewModelHandler
       : IRequestHandler<FillCreateOrEditEducationViewModelQuery, CreateOrEditEducationViewModel>
    {
        private readonly IEducationRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FillCreateOrEditEducationViewModelHandler(IEducationRepository repo, 
            IMapper mapper,ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<CreateOrEditEducationViewModel> Handle(FillCreateOrEditEducationViewModelQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
                return new CreateOrEditEducationViewModel() { Id = 0 };

            var cacheKey = $"education:{request.Id}";

            var cachData = await _cacheService.GetAsync<CreateOrEditEducationViewModel>(cacheKey);
            if (cachData != null) return cachData;

            var education = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (education == null)
                return new CreateOrEditEducationViewModel() { Id = 0 };

            var mapped = _mapper.Map<CreateOrEditEducationViewModel>(education);

            await _cacheService.SetAsync(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }
    }
}
