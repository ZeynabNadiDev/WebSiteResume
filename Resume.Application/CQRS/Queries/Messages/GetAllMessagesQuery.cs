using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Redis.Caching.Interfaces;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.CQRS.Queries.Messages
{
    public record GetAllMessagesQuery():IRequest<List<MessageViewModel>>;
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, List<MessageViewModel>>
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetAllMessagesQueryHandler(IMessageRepository repository, 
            IMapper mapper,ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<List<MessageViewModel>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "messages:index:all";
            var cachedData = await _cacheService.GetAsync<List<MessageViewModel>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            var messages = await _repository.GetEntities().
                ProjectTo<MessageViewModel>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(cacheKey, messages, TimeSpan.FromMinutes(10));
           
            return messages;
        }
    }

}
