

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;

namespace _303_Coursework
{
    class Server
    {
        private void test() {

        }
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;
        private static TcpClient tcpClient;

        //Does all the nesserry setup for the server
        public static void Start(int _maxPlayer, int _port)
        {
            //Assigning the parameter values to their respective properties 
            MaxPlayers = _maxPlayer;
            Port = _port;
            Console.WriteLine("Starting Server........");
            InitializeServerData();
            //inisiling the tcplisitner 
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpClient = new TcpClient();
            tcpListener.Start();
            //passing it an async callback 
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Server started on {Port}. ");

        }
        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            //once a client connects, we need to make sure to continue listeing for connections 
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");
            for (int i = 0; i <= MaxPlayers; i++)
            {
                clients[i].tcp.Connect(_client);
                return;

            }
            Console.WriteLine($"{_client.Client.RemoteEndPoint} falled to connect: server full");


        }
        private static void InitializeServerData()
        {
            for(int i = 0; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));


            }


        }
        
    }
}
