using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SafeMum.Application.Interfaces;
using SafeMum.Application.Repositories;
using SafeMum.Domain.Entities.Communication;

namespace SafeMum.Application.Hubs
{
    public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly Supabase.Client _client;

    public ChatHub(IMessageRepository messageRepository, ISupabaseClientFactory supabaseClient)
    {
        _messageRepository = messageRepository;
        _client = supabaseClient.GetClient();
    }

    public async Task SendMessage(Guid senderId, Guid receiverId, string message)
    {
        var msg = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message,
            SendAt = DateTime.UtcNow
        };

        await _messageRepository.SaveMessageAsync(msg);

        await Clients.User(receiverId.ToString())
            .SendAsync("ReceiveMessage", senderId.ToString(), message);
    }

    public async Task JoinGroup(Guid userId, Guid groupId)
    {
        var result = await _client
            .From<GroupMember>()
            .Where(m => m.UserId == userId && m.ChatGroupId == groupId)
            .Single();

        if (result == null)
        {
            throw new HubException("You are not a member of this group.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
    }

    public async Task SendGroupMessage(Guid senderId, Guid groupId, string message)
    {
        var result = await _client
            .From<GroupMember>()
            .Where(m => m.UserId == senderId && m.ChatGroupId == groupId)
            .Single();

        if (result == null)
        {
            throw new HubException("You are not a member of this group.");
        }

        var msg = new Message
        {
            SenderId = senderId,
            GroupId = groupId,
            Content = message,
            SendAt = DateTime.UtcNow
        };

        await _messageRepository.SaveMessageAsync(msg);

        await Clients.Group(groupId.ToString())
            .SendAsync("ReceiveGroupMessage", senderId.ToString(), message);
    }
}

}
