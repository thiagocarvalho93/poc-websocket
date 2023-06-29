using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WebsocketController : ControllerBase
{
    public WebsocketController()
    {
    }

    [Route("/ws")]
    public async Task GetSocket([FromQuery] string user)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket, user);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private static async Task Echo(WebSocket webSocket, string user)
    {
        // Lista de autorizações pendentes
        // Aqui entrará a rotina de consulta no banco de dados
        List<AutorizacaoPendente> lista = new()
        {
            new AutorizacaoPendente()
            {
                Autorizadores = new List<string>{"thiagopaes.dsn.erp","marina.dsn.erp"},
                Dados = "Autorizacao 1",
            },
            new AutorizacaoPendente()
            {
                Autorizadores = new List<string>{"thiagopaes.dsn.erp"},
                Dados = "Autorizacao 2"
            },
        };
        // Filtrando as autorizações relativas ao usuário passado
        var dadosFiltrados = lista.Where(x => x.Autorizadores.Contains(user)).ToList();

        // Tratando o objeto para mandar para o front
        var objetoResponse = new Response
        {
            Autorizacoes = dadosFiltrados.Select(x => x.Dados).ToList(),
            User = user
        };

        while (webSocket.State == WebSocketState.Open)
        {

            // Serializando
            var json = JsonSerializer.Serialize(objetoResponse);

            // Mandando mensagem
            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes(json),
                WebSocketMessageType.Text,
                true, CancellationToken.None);

            // Aguarda 5 segundos
            await Task.Delay(5000);
        }
    }
}
