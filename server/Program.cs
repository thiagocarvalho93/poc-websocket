using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseWebSockets();


app.Map("/", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        while (webSocket.State == WebSocketState.Open)
        {
            // checar novas autorizacoes pendentes

            // processar

            // mandar mensagens para os destinatários correspondentes

            // aguardar um tempo determinado

            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes($"Alterdata -> {DateTime.Now}"),
                WebSocketMessageType.Text,
                true, CancellationToken.None);
            await Task.Delay(5000);
        }
    }
});

await app.RunAsync();
