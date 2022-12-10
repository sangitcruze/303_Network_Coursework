using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
        };

        ClientSend.PlayerMovement(_inputs);
    }

    //public void ServerCorrection(Vector3 _position, int _time)
    //{
    //    int currentTime = previousPositions.FirstOrDefault(x => x.Time == _time);

    //    if (currentTime == null)
    //        return;

    //    if (Vector3.Distance(new Vector3(currentTime.Position.x, currentTime.Position.y, currentTime.Position.z), _position) > correctionThreshold)
    //    {
    //        transform.position = _position;
    //    }

    //    previousPositions.RemoveAll(x => x.Time <= _time);
    //}



            
    }
