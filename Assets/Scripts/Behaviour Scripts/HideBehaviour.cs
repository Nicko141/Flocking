using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Hide")]
public class HideBehaviour : FilteredFlockBehaviour
{
    public ContextFilter obsticleFilter;

    public float hideBehindObsticleDistance = 2f;


    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        //hide from
        List<Transform> filteredContext = (filter == null) ? areaContext : filter.Filter(agent, areaContext);
        //hide behind
        List<Transform> obsticleContext = (filter == null) ? areaContext : obsticleFilter.Filter(agent, areaContext);

        if (filteredContext.Count == 0)
        {
            return Vector2.zero;
        }

        //find nearest obsticle
        float nearestDistance = float.MaxValue;
        Transform nearestObsticle = null;

        foreach (Transform item in obsticleContext)
        {
            float Distance = Vector2.Distance(item.position, agent.transform.position);
            if (Distance < nearestDistance)
            {
                nearestObsticle = item;
                nearestDistance = Distance;
            }
        }


        if (nearestObsticle == null)
        {
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;
        foreach (Transform item in filteredContext)
        {

            Vector2 obsticleDirection = nearestObsticle.position - item.position;

            obsticleDirection += obsticleDirection.normalized * hideBehindObsticleDistance;
            //get position
            Vector2 hidePosition = ((Vector2)item.position) + obsticleDirection;

            move += hidePosition;
        }
        move /= filteredContext.Count;
        //debug only
        Debug.DrawRay(move, Vector2.up * 3f);
        //find direction AI want to move
        move -= (Vector2)agent.transform.position;

        return move;
    }
}
