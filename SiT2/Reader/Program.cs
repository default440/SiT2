using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ReceiveMessage();
                Thread.Sleep(12000);
            }
        }

        static public void ReceiveMessage()
        {
            Console.Clear();

            string server = "pop.gmail.com";
            TcpClient client = new TcpClient(server, 995);

            var stream = client.GetStream();
            var sslStream = new SslStream(stream);

            sslStream.AuthenticateAsClient(server);

            var writer = new StreamWriter(sslStream);
            var reader = new StreamReader(sslStream);

            writer.WriteLine("USER " + "sksf.study@gmail.com");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("PASS " + "529440Ilya");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("STAT");
            writer.Flush();
            Console.WriteLine(reader.ReadLine());

            writer.WriteLine("LIST");
            writer.Flush();
            string rcv = reader.ReadLine();
            Console.WriteLine(rcv);
            
            for (int i = 0; i < Convert.ToInt32(rcv.Split(' ')[1]); i++)
            {
                Console.WriteLine(reader.ReadLine());
            }
            Console.WriteLine(reader.ReadLine());

            for (int i = 0; i < Convert.ToInt32(rcv.Split(' ')[1]); i++)
            {
                Console.WriteLine("===========================================================");

                writer.WriteLine("RETR " + (i + 1).ToString());
                writer.Flush();

                string s = reader.ReadLine();
                while (s != ".")
                {
                    Console.WriteLine(s);
                    s = reader.ReadLine();
                }
            }
        }
    }
}
