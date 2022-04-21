using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class npc1Walk : MonoBehaviour
{
    public NavMeshAgent agent;

    public Vector3 Destination1;
    public Vector3 Destination2;
    public Vector3 Destination3;
    public Vector3 Destination4;
    
    void Update()
    {
        if (transform.position == Destination1)
        {
            agent.SetDestination(Destination2);
        }
        else if(transform.position == Destination2)
        {
            agent.SetDestination(Destination3);
        }
        else if (transform.position == Destination3)
        {
            agent.SetDestination(Destination4);
        }
        else if (transform.position == Destination4)
        {
            agent.SetDestination(Destination1);
        }

    }
}
