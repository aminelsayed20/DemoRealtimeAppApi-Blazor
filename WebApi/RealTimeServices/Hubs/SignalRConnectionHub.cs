using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.RealTimeServices.Connections
{
    [Authorize]
    public class SignalRConnectionHub : Hub
    {
        private readonly UserConnectionManager _userConnectionManager;

        public SignalRConnectionHub(UserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
           // string username = await GetUsernameFromTokenAcync(token);
            string username = Context.GetHttpContext().Request.Query["username"];
           // string token = Context.GetHttpContext().Request.Query["access_token"];
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


        //private async Task<string> GetUsernameFromTokenAcync(string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //    {
        //        return "Invalid Token";
        //    }

        //    try
        //    {
        //        var handler = new JwtSecurityTokenHandler();
        //        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        //        if (jsonToken == null)
        //        {
        //            return "Invalid Token";
        //        }

        //        // Validate the token (you should use a validation method as needed)
        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = "http://localhost:8310", // Replace with your issuer
        //            ValidAudience = "https://localhost:7015", // Replace with your audience
        //            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("gfggggggggggggggggggggggggggggggggggg"))
        //        };

        //        SecurityToken validatedToken;
        //        ClaimsPrincipal principal = handler.ValidateToken(token, validationParameters, out validatedToken);

        //        // Extract the username or other claims
        //        var usernameClaim = principal.FindFirst(ClaimTypes.Name); // Or ClaimTypes.Email, etc.
        //        return usernameClaim?.Value ?? "Unknown User";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception as needed
        //        return "Error processing token";
        //    }
        //}


    }
}
