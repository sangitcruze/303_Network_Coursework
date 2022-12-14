using _303_Coursework;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }


    public static void CoinCollector(PlayerManager player)
        //Client sends player id and score inrement to the server 
    {
        using (Packet _packet = new Packet((int)ClientPackets.CoinCounter))
        {

            _packet.Write(player.Id);
            _packet.Write(1);

            SendTCPData(_packet);
            Debug.Log("client sending player info to server");


        }
    }
    #endregion

}
