using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_DirectionPlayer_Manager : MonoBehaviour
{
    public string[] playerDirection = new string[4];

    protected bool PlayerDirectionText()
    {
        for (int i = 0; i < playerDirection.Length; i++)
        {
            if (PlayerMovement.currentIdle == GameObject.Find(PlayerMovement.ObjectCollideName).GetComponent<Text_DirectionPlayer_Manager>().playerDirection[i])
            {
                return true;
            }
        }

        return false;
    }
}
