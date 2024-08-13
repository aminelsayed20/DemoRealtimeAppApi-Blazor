using Microsoft.AspNetCore.SignalR;

namespace WebApi.RealTimeServices.Connections
{
    public class SignalRConnectionHub : Hub
    {
        private readonly UserConnectionManager _userConnectionManager;

        public SignalRConnectionHub(UserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            _userConnectionManager.AddConnection(username, Context.ConnectionId);

            if (username == "Admin") return;

            var connectionIds = _userConnectionManager.GetConnectionIds("Admin");
            if (connectionIds != null)
            {
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("UserConnected", username, DateTime.Now);
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = _userConnectionManager.RemoveConnection(Context.ConnectionId);

            if (username == "Admin") return;

            var connectionIds = _userConnectionManager.GetConnectionIds("Admin");
            if (connectionIds != null )
            {
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("UserDisConnected", username, DateTime.Now);
                }
            }

        }
        public async Task UpdateDivPosition(int x, int y)
        {
            await Clients.Others.SendAsync("ReceiveDivPosition", x, y);
        }

        public async Task SendMessageToUser(string username, string message)
        {
            var connectionIds = _userConnectionManager.GetConnectionIds(username);
            if (connectionIds != null)
            {
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
            }
        }




    }
}
