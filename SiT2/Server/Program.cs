using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static int port = 3004;
        static string email = "sksf.bylyba@gmail.com";

        static string theme = "theme";
        static DateTime dateTime = DateTime.Now;

        static void Main(string[] args)
        {
            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);

            sListener.Bind(new IPEndPoint(IPAddress.Any, port));

            sListener.Listen(10);

            int length = 0;

            while (true)
            {
                Socket sender = sListener.Accept();

                byte[] byte_dateTime = new byte[Encoding.ASCII.GetBytes(DateTime.Now.Ticks.ToString()).Length];
                byte[] byte_theme_length = new byte[4];

                _ = sender.Receive(byte_dateTime);
                _ = sender.Receive(byte_theme_length);

                byte[] byte_theme = new byte[BitConverter.ToInt32(byte_theme_length)];

                length = sender.Receive(byte_theme);

                dateTime = new DateTime(Convert.ToInt64(Encoding.ASCII.GetString(byte_dateTime)));
                theme = Encoding.ASCII.GetString(byte_theme, 0, length);

                sender.Close();

                if (theme == "shutdown server") break;

                Console.WriteLine(dateTime.ToString());
                Console.WriteLine(theme);

                SendMessage();
            }

            sListener.Close(); 
        }

        static void SendMessage()
        {
            string server = "smtp.gmail.com";
            TcpClient client = new TcpClient(server, 465);

            var stream = client.GetStream();
            var sslStream = new SslStream(stream);

            sslStream.AuthenticateAsClient(server);

            var writer = new StreamWriter(sslStream);
            var reader = new StreamReader(sslStream);

            writer.WriteLine("HELO " + server);
            writer.Flush();
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("AUTH LOGIN");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            writer.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes("sksf.study@gmail.com")));
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            writer.WriteLine(Convert.ToBase64String(Encoding.UTF8.GetBytes("529440Ilya")));
            writer.Flush();
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("MAIL FROM: <sksf.study@gmail.com>");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            writer.WriteLine($"RCPT TO: <{email}>");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("DATA");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            writer.WriteLine("Subject: " + theme);
            writer.Flush();
            writer.WriteLine("Date: " + dateTime.ToString());
            writer.Flush();
            writer.WriteLine("Resent-Date: " + dateTime.ToString());
            writer.Flush();
            writer.WriteLine("SMTP is working!");
            writer.Flush();
            writer.WriteLine(".");
            writer.Flush();

            writer.WriteLine("QUIT");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            Console.WriteLine(reader.ReadLine());
            Console.WriteLine(reader.ReadLine());
        }
    }
}
