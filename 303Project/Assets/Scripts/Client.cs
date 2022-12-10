using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
using _303_Coursework;


public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    //public string ip = "127.0.0.1";
    public string ip = "109.153.147.157";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;
     
    private bool isConnected = false;


    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> _packetHandlers;
    //internal byte myId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance alredy exists, destroying object!");
            Destroy(this);
        }


    }



   


    private void OnApplicationQuit()
    {

        Disconnect();


    }




    public void ConnectToServer()
    {
        //lets us choose custom ip
        ip = UIManager.instance.IPInputField.text.Trim();
        tcp = new TCP();
        udp = new UDP();
        IntializeClientData();
        isConnected= true;


        tcp.Connect();

    }

    public class TCP
    {

        public TcpClient sockets;
        private NetworkStream stream;
        private Packet receiveData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            sockets = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            receiveBuffer = new byte[dataBufferSize];
            sockets.BeginConnect(instance.ip, instance.port, ConnectCallback, sockets);
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            sockets.EndConnect(_result);

            if (!sockets.Connected)
            {
                return;
            }


            stream = sockets.GetStream();

            receiveData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {

                if (sockets != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);


                }

            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");




            }



        }


        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receiveData.Reset(HandelData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            catch (Exception _ex)
            {
                Disconnect();

            }

        }

        private bool HandelData(byte[] _data)
        {

            int _packetLength = 0;
            receiveData.SetBytes(_data);

            if (receiveData.UnreadLength() >= 4)
            {
                _packetLength = receiveData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }


            }

            while (_packetLength > 0 && _packetLength <= receiveData.UnreadLength())

            {
                byte[] _packetBytes = receiveData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))

                    {
                        int _packetID = _packet.ReadInt();
                        _packetHandlers[_packetID](_packet);

                    }

                });

                _packetLength = 0;
                if (receiveData.UnreadLength() >= 4)
                {
                    _packetLength = receiveData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }


                }


            }
            if (_packetLength <= 1)
            {
                return true;


            }

            return false;

        }

        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;

            receiveData = null;
            receiveBuffer= null;
            sockets= null;

        }

    }



    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);

        }

        public void Connect(int _localPort)
        {

            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);
            using (Packet _packet = new Packet())
            {

                SendData(_packet);

            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)

            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");

            }


        }







        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                if (_data.Length < 4)
                {
                    instance.Disconnect();
                    return;

                }
                HandleData(_data);

            }
            catch
            {

                Disconnect();

            }


        }



        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    _packetHandlers[_packetId](_packet);
                }
            });


        }


        private void Disconnect()
        {
            instance.Disconnect();

             endPoint= null;
             socket= null;

        }


    }



    private void IntializeClientData()
    {
        _packetHandlers = new Dictionary<int, PacketHandler>()
            {

                { (int)ServerPackets.welcome, ClientHandle.Welcome},
                { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer},
                { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition},
                { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation},
                { (int)ServerPackets.VictoryMessage, ClientHandle.CoinCounter}

               

            };
        Debug.Log("Initialized packets.");


    }

    private void Disconnect()
    {
        
        if (isConnected) 
        {
          
            isConnected= false;
            tcp.sockets.Close();
            udp.socket.Close();

            Debug.Log("Disconncted from server.");
        
        
        
        
        }




    }

}



























 





