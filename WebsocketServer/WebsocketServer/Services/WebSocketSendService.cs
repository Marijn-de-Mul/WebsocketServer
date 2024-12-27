using System.Net.WebSockets;
using System.Text;

public class WebSocketSendService
{
    public async Task SendAsync(WebSocket webSocket, string message, CancellationToken cancellationToken)
    {
        var response = Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, cancellationToken);
    }
}