using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding_Manager : MonoBehaviour
{
    Rigidbody2D agent;
    Rigidbody2D target;

    public bool goToX = false;

    float fX, fY, ePosX, ePosY, tPosX, tPosY;

    public float velocity = 2.5f;
    public const float distance = 0.6f;

    public void PathStart(Rigidbody2D enemyRigidBody, Rigidbody2D targetRigidBody)
    {
        agent = enemyRigidBody;
        target = targetRigidBody;
        goToX = false;

        ePosX = agent.transform.position.x;
        ePosY = agent.transform.position.y;
        tPosX = target.transform.position.x;
        tPosY = target.transform.position.y;

        fX = (ePosX < tPosX) ? tPosX - ePosX : ePosX - tPosX;
        fX = (fX < 0) ? fX * (-1) : fX;
        fY = (ePosY < tPosY) ? tPosY - ePosY : ePosY - tPosY;
        fY = (fY < 0) ? fY * (-1) : fY;
    }

    public void PathUpdate()
    {
        int whenEgual = (fX == fY)? Random.Range(0,1) : (fX > fY)? 0 : 1;
                
        ePosX = agent.transform.position.x;
        ePosY = agent.transform.position.y;
        tPosX = target.transform.position.x;
        tPosY = target.transform.position.y;
        whenEgual = (whenEgual == 0)? ((ePosX <= tPosX - distance || ePosX >= tPosX + distance) ? 0 : 1) : ((ePosY <= tPosY - distance || ePosY >= tPosY + distance) ? 1 : 0);
        
        goToX = (whenEgual == 0) ? true : false;

        if (goToX)
            fX = (agent.transform.position.x < target.transform.position.x) ? RightMethod() : LeftMethod();
        else
            fY = (agent.transform.position.y < target.transform.position.y) ? UpMethod() : DownMethod();
    }

    #region Methods
    float RightMethod()
    {
        agent.velocity = new Vector2(velocity, 0);
        return (target.transform.position.x - agent.transform.position.x) - 1;
    }

    float LeftMethod()
    {
        agent.velocity = new Vector2(-velocity, 0);
        return (agent.transform.position.x - target.transform.position.x) - 1;
    }

    float UpMethod()
    {
        agent.velocity = new Vector2(0, velocity);
        return (target.transform.position.y - agent.transform.position.y) - 1;
    }

    float DownMethod()
    {
        agent.velocity = new Vector2(0, -velocity);
        return (agent.transform.position.y - target.transform.position.y) - 1;
    }
    #endregion
}
