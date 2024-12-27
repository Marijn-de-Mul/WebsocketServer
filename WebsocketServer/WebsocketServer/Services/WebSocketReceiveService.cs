using System.Net.WebSockets;
public class WebSocketReceiveService
{
    public async Task<WebSocketReceiveResult> ReceiveAsync(WebSocket webSocket, byte[] buffer, CancellationToken cancellationToken)
    {
        return await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
    }
}