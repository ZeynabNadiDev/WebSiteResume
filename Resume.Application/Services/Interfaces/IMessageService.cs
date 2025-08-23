using Resume.Domain.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Application.Services.Interfaces
{
    public interface IMessageService
    {
        Task<bool> CreateMessage(CreateMessageViewModel message,CancellationToken cancellationToken);
        Task<List<MessageViewModel>> GetAllMessages(CancellationToken cancellationToken);
        Task<bool> DeleteMessage(long id,CancellationToken cancellationToken);
    }
}
