using Resume.Domain.Entity;
using Resume.Domain.Repository;
using Resume.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Repository
{
    public class MessageRepository:GenericRepository<Message>,IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context) { }
    }
}
