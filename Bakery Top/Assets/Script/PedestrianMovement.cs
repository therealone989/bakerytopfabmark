using UnityEngine;
using UnityEngine.AI;

public class PedestrianMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    private Vector3 targetPosition;
    private bool hasSetDestination = false;

    void Start()
    {
    }

    void Update()
    {
        if (!hasSetDestination)
        {
            agent.SetDestination(targetPosition);
            hasSetDestination = true;
        }

    }

}
