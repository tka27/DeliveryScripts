using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer), typeof(NavMeshAgent))]
public class AgentDebug : MonoBehaviour
{
    LineRenderer lineRenderer;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if (agent.pathPending)
        {
            Debug.Log("pending");
        }
        else
        {
            Debug.Log("no pending");
        }

        Debug.Log(agent.pathStatus);



        if (agent.hasPath)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPositions(agent.path.corners);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            Debug.LogError("hasnt path");
        }
    }
}
