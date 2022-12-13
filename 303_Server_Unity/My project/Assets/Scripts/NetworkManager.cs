using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    public int tick;
    public bool isTimerRunning = false;

    public void Fixedupdate()
    {
        if (isTimerRunning)
        {
            // intrement the current tick
            tick++;

        }
    }
    public void StartTimer()
    {
        //starts the timer and resets the tick 
        isTimerRunning = true;
        tick= 0;
    }


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
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;


        Server.Start(50, 26950);

    }



    private void OnApplicationQuit()
    {
        Server.Stop();
    }





    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(219.60f,400.51f,638.92f) , Quaternion.identity).GetComponent<Player>();
    }
}