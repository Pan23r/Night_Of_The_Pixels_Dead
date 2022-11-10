using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh_Manager : MonoBehaviour
{
    Transform target;

    protected NavMeshAgent agent;

    public bool goToX;
    
    // Start is called before the first frame update
    public void NavMeshStart()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    public void NavMeshUpdate()
    {
        agent.SetDestination(target.position);

        Vector2 targetPos = new Vector2 (Mathf.Abs(target.position.x), Mathf.Abs(target.position.y));
        Vector2 agentPos = new Vector2(Mathf.Abs(agent.transform.position.x), Mathf.Abs(agent.transform.position.y));
        float distanceX = (targetPos.x > agentPos.x) ? targetPos.x - agentPos.x : agentPos.x - targetPos.x;
        float distanceY = (targetPos.y > agentPos.y) ? targetPos.y - agentPos.y : agentPos.y - targetPos.y;

        goToX = (distanceX > distanceY) ? true : false;

       /* Vector3 destinationX = new Vector3(target.position.x, agent.transform.position.y, agent.transform.position.z);
        Vector3 destinationY = new Vector3(target.position.x, target.position.y, target.position.z);

        if (goToX)
            agent.SetDestination(destinationX);
        else
            agent.SetDestination(destinationY);*/
    }
}
