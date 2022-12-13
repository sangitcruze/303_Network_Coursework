using _303_Coursework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UIElements;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.sockets.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void CoinCounter(Packet _packet)
    {
        //reads the message from the server that a player has won the game 
        int id = _packet.ReadInt();
        string username = _packet.ReadString();
        UIManager.instance.Victory.text = $"Player {username} has won by collectiing 5 golden balls first :) " ;
        GameManager.instance.stopGame();
       

        Debug.Log("reading message from the server that a player has won");
    }

    //calculates the players pos by the input and the tick
    public static void PlayerPosition(Packet _packet)
    {
        var _id = _packet.ReadInt();
        var tick = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Debug.Log($"current tick: {tick},{ GameManager.instance.tick}");
        //GameManager.players[_id].transform.position = _position;
        if (GameManager.players.TryGetValue(_id, out var _player))
        {

            //local player
            if(_id.Equals(Client.instance.myId))

            if (!_player.ServerCorrection(_position, tick))
            {
                    Debug.Log("local player connection failed");

            }

        }
        //Exteranal player
        else
        {
            _player.transform.position = _position;
            _player.Positions.Add(new OldPositions( _position, tick));
            if (_player.Positions.Count > 10) _player.Positions.RemoveAt(0);



        }
    }
        
        
       
    //    if (GameManager.instance.tick == _tick)
    //    {
    //        Debug.Log("Tick is lining up correctly")

    //    }

    //    Debug.Log("player pos moveemnt handle RTX4090TI sent from client");
    //    Debug.Log($"{_id} has position:{_position}");
    //}

    public static void StartTimer(Packet _packet)
    {
        //reads the current tick from the server
        int _tick = _packet.ReadInt();
        GameManager.instance.StartTimer(_tick);
    }

    


public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
     
    }



}