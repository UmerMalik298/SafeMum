using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Repositories
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(Message message);
        Task<List<Message>> GetMessagesAsync(Guid senderId, Guid receiverId);


        Task<List<Message>> GetAllMessagesAsync();

    }
}
