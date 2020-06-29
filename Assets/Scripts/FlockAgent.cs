﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock
    {
        get
        {
            return agentFlock;
        }
    }

    private Collider2D agentCollider;
    public Collider2D AgentCollider
    {
        get
        {
            return agentCollider;
        }
    }
    private void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        if (agentCollider == null)
        {
            Debug.LogError("FlockAgent can't find Collider2D");
        }
    }

    public void inisialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
