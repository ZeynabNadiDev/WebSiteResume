using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public GetAllMessagesQueryHandler(IMessageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<MessageViewModel>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _repository.GetEntities().
                ProjectTo<MessageViewModel>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return messages;
        }
    }

}
