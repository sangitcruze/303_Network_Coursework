using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _303_Coursework
{
    internal class Client
    {
        public static int dataBufferSize = 4096;
       



        //stores client id
        public int id;
        public TCP tcp;

        //construtor 
        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);

        }
      
       


        public class TCP
        {

            public TcpClient socket;
            
            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;

            }
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receiveBuffer = new byte[dataBufferSize];
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "welcome to the server");


            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);


                    }
                }

                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP:{_ex}");


                }


                }


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
                    Console.WriteLine(ex.Message,"Error receving TCP data");


                }






            }

        }

    }
}
