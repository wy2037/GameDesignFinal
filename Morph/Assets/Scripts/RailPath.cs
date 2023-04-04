using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPath : MonoBehaviour
{
    [SerializeField] private Transform[] pathWayPoints;
    [SerializeField] private GameObject obj;
    float moveSpeed;
    int nextWaypointIdx;
    Vector2 currentPosition;
    int wayPointCounts;

    void Start() {
        moveSpeed = 2f;
        nextWaypointIdx = 0;
        wayPointCounts = pathWayPoints.Length;
    }

    void Update() {
        currentPosition = obj.transform.position;
        obj.transform.position = Vector2.MoveTowards(currentPosition, pathWayPoints[nextWaypointIdx].position, moveSpeed * Time.deltaTime);
        if (currentPosition == (Vector2)pathWayPoints[nextWaypointIdx].position) {
            nextWaypointIdx++;
            if (nextWaypointIdx >= wayPointCounts) {
                nextWaypointIdx = 0;
            }
        }
    }
}
