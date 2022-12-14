
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] GameObject[] wayoints;
    int currentWaypointIndex = 0;

    [SerializeField] float speed = 1f;
   

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, wayoints[currentWaypointIndex].transform.position)< .1f)

        transform.position = Vector3.MoveTowards(transform.position, wayoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);



        
    }
}
