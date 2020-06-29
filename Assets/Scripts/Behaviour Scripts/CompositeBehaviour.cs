using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [System.Serializable]
    public class FlockClass
    {
        public FlockBehaviour behaviours;
        public float weights;
    }
    public FlockClass[] Flocks;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, List<Transform> areaContext, Flock flock)
    {
        //set up move
        Vector2 move = Vector2.zero;

        //iterate through behaviours
        for (int i = 0; i < Flocks.Length; i++)
        {
            Vector2 partialMove = Flocks[i].behaviours.CalculateMove(agent, context, areaContext, flock) * Flocks[i].weights;

            if (partialMove != Vector2.zero)
            {
                if (partialMove.SqrMagnitude() > Flocks[i].weights * Flocks[i].weights)
                {
                    partialMove.Normalize();
                    partialMove *= Flocks[i].weights;
                }
                move += partialMove;
            }
        }

        return move;
    }
   
}


