using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        // Thread signalling object.
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            //Create and start the TCP Server
            TcpListener server = new TcpListener(IPAddress.Loopback, 12345);
            server.Start();

            while (true) {
                tcpClientConnected.Reset();
                server.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), server);
                tcpClientConnected.WaitOne();
            }
        }

        static void AcceptClient(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on 
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

            tcpClientConnected.Set();
            NetworkStream stream = client.GetStream();

            using (StreamReader sr = new StreamReader(stream))
            {
                String message = sr.ReadToEnd();
                Console.Write(message);

            }

            stream.Close();
            client.Close();
            
        }
    }
}
