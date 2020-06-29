using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Area Avoidance")]
public class AreaAvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);

        if (filteredContext.Count == 0)
        {
            return agent.transform.up;
        }

        //
        Vector2 avoidanceMove = Vector2.zero;
        int AvoidCount = 0;
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                AvoidCount++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }

        }
        if (AvoidCount > 0)
        {
            avoidanceMove /= AvoidCount;
        }


        return avoidanceMove;
    }
}
