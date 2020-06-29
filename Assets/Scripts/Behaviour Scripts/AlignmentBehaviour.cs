﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> filteredContext;
        if (filter == null)
        {
            filteredContext = context;
        }
        else
        {
            filteredContext = filter.Filter(agent, context);
        }
        //if no neighbours maintain current alignment
        if (filteredContext.Count == 0)
        {
            return agent.transform.up;
        }

        //add all directions together and average
        Vector2 alignmentMove = Vector2.zero;
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= filteredContext.Count;


        return alignmentMove;

    }
}
