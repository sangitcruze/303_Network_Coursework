using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
   
    //gets the currect tick we are on
    public int tick;
    //server tick rate/fps
    private const float SERVER_TICK_RATE = 30f;
    public bool isTimerRunning = false;

  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        //DontDestroyOnLoad(this);
    }
    public void StartTimer(int _tick)
    {
        tick = _tick;
        isTimerRunning= true;
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }



        _player.GetComponent<PlayerManager>().Id = _id;
        _player.GetComponent<PlayerManager>().Username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());

    }

    public void FixedUpdate()
    {
          if (isTimerRunning)
        {
            // intrement the current tick
            tick++;
        }
            
          
        
    }

    public void stopGame()
    {
        StartCoroutine(StopGame());
        Debug.Log("stopping the game");
    }


   public IEnumerator StopGame()
    {
        yield return new WaitForSeconds(3);
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        Debug.Log("application quit timer working ");
    }



}