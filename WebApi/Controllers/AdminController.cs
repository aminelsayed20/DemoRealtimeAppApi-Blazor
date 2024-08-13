using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.RealTimeServices.Connections;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IHubContext<SignalRConnectionHub> _hubContext;
        private  UserConnectionManager _userConnectionManager;
        private readonly ILogger<WeatherForecastController> _logger;

        public AdminController(ILogger<WeatherForecastController> logger,
            IHubContext<SignalRConnectionHub> hubContext,
             UserConnectionManager userConnectionManager)
        {
            _logger = logger;
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
        }

        [HttpPost("Hi")]
        public IActionResult hi()
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server Say : Hi");
            return Ok("hi");
        }

        [HttpPost("sendToUser")]
        public async Task<IActionResult> SendMessageToUser([FromQuery] string username, string message)
        {
            var connectionIds = _userConnectionManager.GetConnectionIds(username);
            if (connectionIds.Any())
            {
                foreach (var connectionId in connectionIds)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
                return Ok();
            }
            return NotFound("User not found or not connected.");
        }

    }
}
