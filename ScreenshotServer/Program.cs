using System.Diagnostics.Metrics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var ipAddress = IPAddress.Parse("192.168.1.82");

        var port = 54751;

        var endPoint = new IPEndPoint(ipAddress, port);

        int counter = 0;

        var server = new TcpListener(endPoint);

        server.Start();

        while (true)
        {

            var client = server.AcceptTcpClient();

            var dInfo = Directory.CreateDirectory($"C:\\Users\\DELL\\Desktop\\ServerSS\\{client.Client.ToString()}");

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    Console.Clear();

                    var networkStream = client.GetStream();

                    byte[] buffer = new byte[150000];

                    int bytesRead = 0;

                    Console.WriteLine("Creating a new file for new screenshot...");

                    await Task.Delay(2000); 

                    using (var fs = new FileStream($"{dInfo.FullName}\\screens{counter++}.png", FileMode.Create, FileAccess.Write))
                    {

                            bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                        
                             fs.Write(buffer, 0, bytesRead);
                        
                    }
                    Console.WriteLine($"{counter}.File received");
                    await Task.Delay(1000);
                }
            });

        }
    }
}