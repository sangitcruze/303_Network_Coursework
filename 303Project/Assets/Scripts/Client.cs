using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Net.Sockets;
public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myID = 0;
    public TCP tcp;


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



    private void Start()
    {
        tcp = new TCP();

    }


    public void ConnectToServer()
    {

        tcp.Connect();

    }

    public class TCP
    {

        public TcpClient sockets;
        private NetworkStream stream;
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
        stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
    }
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Error receving TCP data");


            }
         
        }


    }


 }





