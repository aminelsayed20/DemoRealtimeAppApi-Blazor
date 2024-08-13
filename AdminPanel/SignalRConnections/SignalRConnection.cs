using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
namespace AdminPanel.SignalRConnections
{
    public class SignalRConnection(NavigationManager NavManager)
    {

        public HubConnection hubConnection;

        public event Action? ConnectionStateChanged;
        public string ConnectionState = string.Empty;

        public async Task StartConnection()
        {
            hubConnection = new HubConnectionBuilder()
           .WithUrl(NavManager.ToAbsoluteUri("https://localhost:7015/connect") + "?username=" + "Admin")
           .Build();
            if (hubConnection.State != HubConnectionState.Connected)
            {
                try
                {
                    await hubConnection.StartAsync();
                    Console.WriteLine("Connection started");
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting connection: {ex.Message}");
                }
                hubConnection.ToString();
            }
            GetConnectionState();
        }
        public async Task CloseConnection()
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.StopAsync();
            }
            GetConnectionState();
        }
        public void GetConnectionState()
        {
            switch (hubConnection.State)
            {
                case HubConnectionState.Connected:
                    Invoke("Connected");
                    break;
                case HubConnectionState.Connecting:
                    Invoke("Connecting...");
                    break;
                case HubConnectionState.Reconnecting:
                    Invoke("Reconnecting...");
                    break;
                case HubConnectionState.Disconnected:
                    Invoke("Disconnected");
                    break;
                default:
                    ConnectionState = "Unknow error occured";
                    break;
            };

            void Invoke(string message)
            {
                ConnectionState = message;
                ConnectionStateChanged?.Invoke();
            }
        }

    }
}
