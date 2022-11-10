using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb_Manager : MonoBehaviour
{
    float timer = 0;
    bool attackPlayer = false;
    Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = gameObject.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 90)
        {
            Destroy(gameObject);
        }
        else if (timer >= 88)
        {
            myAnim.Play("BombExplosion");
        }

        if (attackPlayer && timer >= 0.2f && !GameMenu_Manager.menuIsActive)
        {
            PlayerLife_Manager.life -= Damage();
            PlayerMovement.canMove = true;
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }

    int Damage()
    {
        return (ChoiceOfDifficultyMenu_Manager.easyMode) ? Random.Range(5, 15) : Random.Range(15, 25);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player_Collider_Enemy")
        {
            attackPlayer = true;
            PlayerMovement.canMove = false;
            timer = 0; 
            myAnim.Play("BombExplosion");
        }
    }
}
