using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeZPositionObj_Manager : MonoBehaviour
{
    public float Zmin;
    public float Zmax;
    GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.y < gameObject.transform.position.y)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Zmin);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Zmax);
        }
    }
}
