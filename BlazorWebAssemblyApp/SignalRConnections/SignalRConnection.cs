﻿using BlazorWebAssemblyApp.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWebAssemblyApp.SignalRConnections
{
    public class SignalRConnection(NavigationManager NavManager, AuthenticationStateProvider authStateProvider)
    {

        public HubConnection hubConnection;

        public event Action? ConnectionStateChanged;
        public string ConnectionState = string.Empty;
        public string UserName = string.Empty;

        public async Task StartConnection()
        {
            UserName = await GetUserName();
            hubConnection = new HubConnectionBuilder()
           .WithUrl(NavManager.ToAbsoluteUri("https://localhost:7015/connect") + "?username=" + UserName)
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

        private async Task<string> GetUserName()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var username = "unknown user";

            if (user.Identity?.IsAuthenticated ?? false)
            {
                username = user.Identity.Name ?? "Unknown User";
            }
            else
            {
                username = "Guest"; // Default value if not authenticated
            }
            return username;

        }

    }
}
