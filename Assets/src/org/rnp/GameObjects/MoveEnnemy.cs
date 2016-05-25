using UnityEngine;
using System.Collections;

public class MoveEnnemy : MonoBehaviour {

    private GameObject[] waypoints;
    private GameObject road;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;

    private bool initialized = false;


    public void Init(GameObject Path)
    {
        road = Path;

        // Initialize to the current time

        lastWaypointSwitchTime = Time.time;
        Transform parent = road.transform;
        int children = parent.childCount;
        Debug.Log("ChildCount : " + children);
        waypoints = new GameObject[children];
        for (int i = 0; i < children; i++)
        {
            waypoints[i] = parent.GetChild(i).gameObject;
        }

        Debug.Log("wp size: " + waypoints.Length);

        currentWaypoint = 0;
        initialized = true;
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (initialized)
        {

            // Gets the start and end position for the path
            Vector3 startPosition = waypoints[currentWaypoint].transform.position;
            Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;


            // 
            float pathLength = Vector3.Distance(startPosition, endPosition);
            float totalTimeForPath = pathLength / speed;
            float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            // 3 
            if (gameObject.transform.position.Equals(endPosition))
            {
                if (currentWaypoint < waypoints.Length - 2)
                {
                    // 3.a 
                    currentWaypoint++;
                    lastWaypointSwitchTime = Time.time;
                    // TODO: Rotate into move direction
                }
                else
                {
                    // 3.b 
                    Destroy(gameObject);
                    // TODO: deduct health
                }
            }
        }


    }
}
