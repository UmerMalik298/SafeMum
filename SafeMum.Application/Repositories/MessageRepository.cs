using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
using Supabase.Interfaces;

namespace SafeMum.Application.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Supabase.Client _client;
        public MessageRepository(ISupabaseClientFactory supabaseClient)
        {
           _client = supabaseClient.GetClient();
            
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            var response = await _client
        .From<Message>()
        .Order("sendat", Supabase.Postgrest.Constants.Ordering.Ascending)
        .Get();

            return response.Models;
        }

        public async Task<List<Message>> GetMessagesAsync(Guid senderId, Guid receiverId)
        {
            var response = await _client
                .From<Message>()
                .Where(x =>
                    (x.SenderId == senderId && x.ReceiverId == receiverId) ||
                    (x.ReceiverId == senderId && x.SenderId == receiverId))
                .Order("sendat", Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            return response.Models;
        }


        public async Task SaveMessageAsync(Message message)
        {
          await  _client.From<Message>().Insert(message);

        }
    }
}
