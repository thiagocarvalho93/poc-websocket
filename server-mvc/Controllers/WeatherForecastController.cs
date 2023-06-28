using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace server_mvc.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [Route("/ws")]
    public async Task GetSocket([FromQuery] string user)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket,user);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task Echo(WebSocket webSocket, string user)
    {
        while (webSocket.State == WebSocketState.Open)
        {
            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes($"{DateTime.Now} user: {user}"),
                WebSocketMessageType.Text,
                true, CancellationToken.None);
            await Task.Delay(5000);
        }
    }
}
