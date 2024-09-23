using Chating.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string recipientUserId, string message)
        {
            try
            {
                var chatMessage = new ChatMessage
                {
                    Sender = Context.UserIdentifier,
                    Recipient = recipientUserId,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    recever = recipientUserId

                };

                _dbContext.ChatMessages.Add(chatMessage);
                await _dbContext.SaveChangesAsync();

                var connection = await _dbContext.Connections.FirstOrDefaultAsync(c => c.UserId == recipientUserId);

                if (connection != null)
                {
                    await Clients.Client(connection.ConnectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
                }
                else
                {
                    var offlineMessage = new OfflineMessage
                    {
                        UserId = recipientUserId,
                        Sender = Context.UserIdentifier,
                        Message = message,
                        Timestamp = DateTime.UtcNow

                    };

                    _dbContext.OfflineMessages.Add(offlineMessage);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SendMessage: " + ex.Message);
                throw; // Re-throw the exception to propagate it upwards
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                string connectionId = Context.ConnectionId;

                var connection = new Connection
                {
                    UserId = Context.UserIdentifier,
                    ConnectionId = connectionId
                };

                _dbContext.Connections.Add(connection);
                await _dbContext.SaveChangesAsync();

                var offlineMessages = await _dbContext.OfflineMessages
                    .Where(m => m.UserId == Context.UserIdentifier)
                    .ToListAsync();

                foreach (var offlineMessage in offlineMessages)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", offlineMessage.Sender, offlineMessage.Message);
                }

                _dbContext.OfflineMessages.RemoveRange(offlineMessages);
                await _dbContext.SaveChangesAsync();

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OnConnectedAsync: " + ex.Message);
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                string connectionId = Context.ConnectionId;

                var connection = await _dbContext.Connections.FirstOrDefaultAsync(c => c.ConnectionId == connectionId);

                if (connection != null)
                {
                    _dbContext.Connections.Remove(connection);
                    await _dbContext.SaveChangesAsync();
                }

                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OnDisconnectedAsync: " + ex.Message);
                throw;
            }
        }
    }
}