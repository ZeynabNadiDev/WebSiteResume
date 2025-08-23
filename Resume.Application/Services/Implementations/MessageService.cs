using Microsoft.EntityFrameworkCore;
using Resume.Application.Security;
using Resume.Application.Services.Interfaces;
using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Domain.ViewModels.Message;
using Resume.Infra.Data.Context;
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
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        #endregion

        public async Task<bool> CreateMessage(CreateMessageViewModel message,CancellationToken cancellationToken)
        {
            Message newMessage = new Message()
            {
                Email = message.Email.SanitizeText(),
                Name = message.Name.SanitizeText(),
                Text = message.Text.SanitizeText()
            };

            await _messageRepository.AddAsync(newMessage,cancellationToken);
            await _messageRepository.SaveChangeAsync(cancellationToken);

            return true;
        }


        public async Task<List<MessageViewModel>> GetAllMessages(CancellationToken cancellationToken)
        {
            List<MessageViewModel> messages = await _messageRepository.GetEntities()
                .Select(m => new MessageViewModel()
                {
                    Id = m.Id,
                    Email = m.Email,
                    Name = m.Name,
                    Text = m.Text
                })
                .ToListAsync(cancellationToken);

            return messages;
        }


        public async Task<bool> DeleteMessage(long id,CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.GetByIdAsync(id, cancellationToken);

            if (message == null) return false;

            _messageRepository.Delete(message);
            await _messageRepository.SaveChangeAsync(cancellationToken) ;

            return true;
        }



    }
}
