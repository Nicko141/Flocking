using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public int agentsCount { get { return agents.Count; } }
    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 100f)]
    public float areaRadius = 20f;
    [Range(0f,1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeughbourRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius
    {
        get
        {
            return squareAvoidanceRadius;
        }
    }
    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeughbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeughbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(agentPrefab, Random.insideUnitCircle * startingCount * AgentDensity,Quaternion.Euler(Vector3.forward * Random.Range(0f,360f)),transform);
            newAgent.inisialize(this);
            newAgent.name = "Agent " + i;

            agents.Add(newAgent);
        }
    }
    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent, neighbourRadius);
            List<Transform> areaContext = GetNearbyObjects(agent, areaRadius);
            //TEST ONLY!!
            //agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behaviour.CalculateMove(agent, context, areaContext, this);
            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent, float radius)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, radius);

        foreach (Collider2D contextCollider in contextColliders)
        {
            if (contextCollider != agent.AgentCollider)
            {
                context.Add(contextCollider.transform);
            }
        }

        return context;
    }
}
