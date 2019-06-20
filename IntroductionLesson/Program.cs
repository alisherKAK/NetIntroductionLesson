using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IntroductionLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                string ipAddress = "0.0.0.0";
                //string ipAddress = "127.0.0.1";
                //string ipAddress = "10.1.4.46";
                int port = 12345;

                socketServer.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
                socketServer.Listen(100);

                while (true)
                {
                    Socket client = socketServer.Accept();

                    Console.WriteLine("Клинет подключился: ");
                    Console.WriteLine(client.RemoteEndPoint.ToString());

                    ThreadPool.QueueUserWorkItem(Temp, client);

                }
            }

            Console.ReadLine();
        }

        static void Temp(object state)
        {
            Socket client = state as Socket;
            try
            {
                byte[] buf = new byte[1024];
                while (true)
                {
                    int recSize = client.Receive(buf);
                    client.Send(buf, recSize, SocketFlags.None);
                }
            }
            catch(Exception)
            {

            }

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
