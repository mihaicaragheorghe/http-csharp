using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpServer;

public class Server(int port)
{
    public async Task Start(CancellationToken cancellationToken)
    {
        var server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine($"Server started on port {port}...");

        while (!cancellationToken.IsCancellationRequested)
        {
            Socket socket = await server.AcceptSocketAsync(cancellationToken);
            _ = Task.Run(async () =>
            {
                try
                {
                    await HandleSocketAsync(socket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured while processing the socket, {ex.Message}");
                }
                finally
                {
                    socket.Disconnect(true);
                }
            }, cancellationToken);
        }
    }

    private async Task HandleSocketAsync(Socket socket)
    {
        Console.WriteLine("New socket connected");
        string response = "HTTP/1.1 200 OK\r\n\r\n";
        await socket.SendAsync(Encoding.UTF8.GetBytes(response));
    }
}