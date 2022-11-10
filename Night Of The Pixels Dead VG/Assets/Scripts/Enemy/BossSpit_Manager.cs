using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit_Manager : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Rigidbody2D boss;
    Rigidbody2D player;
    float directionX;
    float directionY;
    bool destroy = false; 

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        boss = GameObject.Find("Boss").GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        myRigidBody.transform.position = boss.transform.position;

        if (!EnemyBoss.directionOnX)
        {
            directionX = (boss.transform.position.x < player.transform.position.x) ? 4 : -4;
            directionY = Random.Range(-0.5f, 0.6f);
        }
        else
        {
            directionY = (boss.transform.position.y < player.transform.position.y) ? 4 : -4;
            directionX = Random.Range(-0.5f, 0.6f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
            Destroy(gameObject);

        if (!GameMenu_Manager.menuIsActive)
            myRigidBody.velocity = new Vector2(directionX, directionY);
        else
            myRigidBody.velocity = Vector2.zero;
    }

    float Damage()
    {
        return (ChoiceOfDifficultyMenu_Manager.easyMode) ? Random.Range(0.25f, 1f) : Random.Range(1, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            PlayerLife_Manager.life -= Damage();
            destroy = true;
        }
    }
}
