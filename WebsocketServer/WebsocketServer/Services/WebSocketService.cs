using System.Net;
using System.Net.WebSockets;

public class WebSocketService
{
    private readonly WebSocketReceiveService _receiveService;
    private readonly WebSocketProcessService _processService;
    private readonly WebSocketSendService _sendService;

    public WebSocketService(WebSocketReceiveService receiveService, WebSocketProcessService processService, WebSocketSendService sendService)
    {
        _receiveService = receiveService;
        _processService = processService;
        _sendService = sendService;
    }

    public async Task HandleWebSocketAsync(WebSocket webSocket, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            var result = await _receiveService.ReceiveAsync(webSocket, buffer, cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
            }
            else
            {
                var message = _processService.ProcessMessage(buffer, result.Count);
                await _sendService.SendAsync(webSocket, message, cancellationToken);
            }
        }
    }

    public async Task StartAsync(string uri, CancellationToken cancellationToken)
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add(uri);
        httpListener.Start();
        Console.WriteLine("Server started at " + uri);

        while (!cancellationToken.IsCancellationRequested)
        {
            var context = await httpListener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                _ = HandleWebSocketAsync(webSocketContext.WebSocket, cancellationToken);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }

        httpListener.Stop();
    }
}