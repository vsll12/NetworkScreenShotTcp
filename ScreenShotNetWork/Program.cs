using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

//client ekrani 10 saniyeden bir screenshot edib servere gonderir.
//server client uchun folder yaradib, onun ichirisine download edir.


internal class Program
{
    private static async Task Main(string[] args)
    {
        var ipAddress = IPAddress.Parse("192.168.1.82");

        var port = 54751;

        var endPoint = new IPEndPoint(ipAddress, port);

        var client = new TcpClient();

        int counter = 0;

        client.Connect(endPoint);

        if (client.Connected)
        {

            while (true)
            {
                var networkStream = client.GetStream();

                var path = GetScreen(counter);

                var bytes = new byte[150000];

                var memoryStream = new MemoryStream();

                int len = 0;


                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {

                    len = fs.Read(bytes, 0, bytes.Length);
                    
                    networkStream.Write(bytes, 0, len);
                    
                }

                Console.WriteLine($"Process{counter + 1} finished successfully");

                counter++;

                Thread.Sleep(10000);

            }
        }

        Console.ReadKey();

    }
    public static string GetScreen(int counter)
    {

        int screenWidth = 1920;
        int screenHeight = 1080;

        using (Bitmap screenshot = new Bitmap(screenWidth, screenHeight))
        {

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
            }

            var path = $"C:\\Users\\DELL\\Desktop\\Screenshots\\screenshot{counter++}.png";

            screenshot.Save(path, ImageFormat.Png);

            return path;
        }
    }
}