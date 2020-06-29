﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Wander")]
public class WanderBehaviour : FilteredFlockBehaviour
{
    Path path = null;
    int? currentWaypoint = null;


    public (Vector2, bool) StayInRadius(FlockAgent agent)
    {
        //direction towards the center
        Vector2 centerOffset = (Vector2)path.waypoints[(int)currentWaypoint].position - (Vector2)agent.transform.position;
        //distance to center
        float t = centerOffset.magnitude / path.radius;
        if (t < path.returnPercentage)
        {
            return (Vector2.zero, true);
        }

        return (centerOffset, false);
    }
    public Vector2 FollowPath(FlockAgent agent)
    {
        if (path == null)
        {
            return Vector2.zero;
        }

        if (currentWaypoint == null)
        {
            currentWaypoint = 0;
        }

        (Vector2 move, bool isAtRadius) = StayInRadius(agent);

        if (isAtRadius)
        {
            currentWaypoint++;

            if (currentWaypoint >= path.waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }
        return move;
    }

    public void FindPath(FlockAgent agent, List<Transform> areaContext)
    {
        List<Transform> filteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);

        //if no neighbours maintain current alignment
        if (filteredContext.Count == 0)
        {
            return;
        }


        int pathIndex = Random.Range(0, filteredContext.Count);
        path = filteredContext[pathIndex].GetComponentInParent<Path>();
    }

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        if (path == null)
        {
            FindPath(agent, areaContext);
        }

        return FollowPath(agent);
    }
}
