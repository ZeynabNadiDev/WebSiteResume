using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Security;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.UnitOfWorks.Interface;
using Resume.Domain.ViewModels.Message;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Implementations
{
    public class MessageService : IMessageService
    {

        #region Constructor MessageRepository
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public MessageService(IMessageRepository messageRepository,IMapper mapper,IUnitOfWork unit)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _uow = unit;
        }
        #endregion

        public async Task<bool> CreateMessage(CreateMessageViewModel message,CancellationToken cancellationToken)
        {
            Message newMessage =_mapper.Map<Message>(message);

            await _messageRepository.AddAsync(newMessage,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return true;
        }


        public async Task<List<MessageViewModel>> GetAllMessages(CancellationToken cancellationToken)
        {
            List<MessageViewModel> messages = await _messageRepository.GetEntities()
                 .ProjectTo<MessageViewModel>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            return messages;
        }


        public async Task<bool> DeleteMessage(long id,CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.GetByIdAsync(id, cancellationToken);

            if (message == null) return false;

            _messageRepository.Delete(message);
            await _uow.SaveChangesAsync(cancellationToken) ;

            return true;
        }



    }
}
