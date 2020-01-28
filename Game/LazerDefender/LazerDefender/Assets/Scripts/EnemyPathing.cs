using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints; //Since they are dynamic we can just create the size in the inspector.
    int wayPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints(); //The scriptable object we have this assigned to will grab a reference to all the wavepoints assigned in the scriptable object
        transform.position = waypoints[wayPointIndex].transform.position; //Starting our enemy at this position
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig) //If you have a bug of just dragging one enemy into the heiarchy you will have an error since this never ran.
    {
        this.waveConfig = waveConfig; //this waveConfig equals whatever we pass into it
    }

    private void Move()
    {
        if (wayPointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[wayPointIndex].transform.position; //Grabs the target position *We will start at the first position*
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime; //Frame independent
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame); //Static method which is basically like a lerp but its a method instead and does the same thing

            if (transform.position == targetPosition) //If we are at the position
            {
                wayPointIndex++; //Increase index
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
