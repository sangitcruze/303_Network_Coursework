using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int Id { get; set; }
    public string Username { get; set; }

    public List<OldPositions> Positions { get; } = new();
    [SerializeField][Range(0.1f, 5f)] private float NetworkThreshold = 1;

    [SerializeField] bool isPredictionEnabled;


    private void Start()
    {
        fromPosition = transform.position;
        toPosition= transform.position;

    }
    private Vector3 fromPosition = Vector3.zero;
    private Vector3 toPosition = Vector3.zero;
    private float lastTime;



    public void SetPosition(Vector3 position)
    {
        fromPosition = toPosition;
        toPosition = position;
        lastTime = Time.time;



    }
    //sends the player back to the correct position if the client prediction is too behind 
    public bool ServerCorrection(Vector3 _position, int _tick)
    {
        
        var currentTime = Positions.Where(x => x.tick == _tick).FirstOrDefault();

        if (currentTime == null)
        { 
            Debug.Log("current time is null");
            return false;
        }
       
        Debug.Log(message: $"Correctiong position at server time: {_tick} Client time: {currentTime.tick}");

        var distance =
        Vector3.Distance(new Vector3(currentTime.Position.x, currentTime.Position.y, currentTime.Position.z), _position);
        if (distance > NetworkThreshold)
        {   //if the prediction is too far behind 
            Debug.Log($"prediction was behind by {distance}");
            transform.position = _position;
        }
        // accepts the client prediction and removes it afterwards 
        Positions.RemoveAll(x => x.tick <= _tick);
       
        return true; 
        
    }

}
