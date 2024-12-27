using System.Text;

public class WebSocketProcessService
{
    public string ProcessMessage(byte[] buffer, int count)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, count);
        Console.WriteLine("Received: " + message);
        return "Echo: " + message;
    }
}