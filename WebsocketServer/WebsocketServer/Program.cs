using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var receiveService = new WebSocketReceiveService();
        var processService = new WebSocketProcessService();
        var sendService = new WebSocketSendService();
        var webSocketService = new WebSocketService(receiveService, processService, sendService);
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
        };

        await webSocketService.StartAsync("http://localhost:5000/", cts.Token);
    }
}