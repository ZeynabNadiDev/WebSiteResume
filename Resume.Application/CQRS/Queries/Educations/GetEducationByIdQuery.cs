using AngleSharp.Dom;
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
    public record GetEducationByIdQuery(long Id) : IRequest<Education>;
    public class GetEducationByIdHandler : IRequestHandler<GetEducationByIdQuery, Education>
    {
        private readonly IEducationRepository _repo;
        private readonly ICacheService _cacheService;

        public GetEducationByIdHandler(IEducationRepository repo,ICacheService cacheService)
        {
            _repo = repo;
            _cacheService = cacheService;  
        }

        public async Task<Education> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"education:{request.Id}";
            var cachedData = await _cacheService.GetAsync<Education>(cacheKey);
            if (cachedData != null) return cachedData;

            var education = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (education != null)
               await _cacheService.SetAsync(cacheKey,education, TimeSpan.FromMinutes(10));

           return education;
        }
    }
}

