using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum bossSteps { idle, firstStep, secondStep, thirdStep, attack, notAttack, death }
public class EnemyBoss : PathFinding_Manager
{
    public GameObject prefab_Bomb;
    public GameObject prefab_Spit;

    GameObject blood;
    Image blackGround;
    Animator bloodAnim;
    Animator myAnim;
    float bloodTimeOfDestroy = 0.5f;

    float life;
    float currentMaxLife;
    const float maxLifeEasy = 1400;//1400
    const float maxLifeHard = 800; //800
    bossSteps currentStep;
    bossSteps saveStep;
    bossSteps attackSaveCurrentStep;
    bool damageToPlayer = false;
    Rigidbody2D boss;
    Rigidbody2D player;
    bool follow = true;
    float timer;
    float timerAttack;
    bool spitActive = false;
    const float spitDistance = 8;
    const float maxTimerSpit = 30;
    public static bool directionOnX = false;
    bool run = false;
    bool AttackSoundisPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        life = (ChoiceOfDifficultyMenu_Manager.easyMode)? maxLifeEasy : maxLifeHard;
        currentMaxLife = life;
        currentStep = bossSteps.firstStep;
        boss = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        blood = GameObject.Find("BloodBoss");
        blackGround = GameObject.Find("ENDGAME_BACKGROUND").GetComponent<Image>();
        blackGround.CrossFadeAlpha(0, 0, false);
        bloodAnim = blood.GetComponent<Animator>();
        myAnim = GetComponent<Animator>();
        PathStart(boss, player);

        blood.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y);
        switch (currentStep)
        {
            case bossSteps.idle:
                boss.velocity = Vector2.zero;
                
                if (!GameMenu_Manager.menuIsActive && !Text_Creator.isActive)
                    currentStep = saveStep;

               break;

            case bossSteps.firstStep:
                FirstStep();
                break;

            case bossSteps.secondStep:
                SecondStep();
                break;

            case bossSteps.thirdStep:
                ThirdStep();
                break;

            case bossSteps.attack:
                EnemyAttack();
                break;

            case bossSteps.notAttack:
                EnemyNotAttackCharge();
                break;

            case bossSteps.death:
                Death();
                break;
        }

        blood.SetActive((blood.activeSelf && bloodAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > bloodTimeOfDestroy) ? false : (!blood.activeSelf) ? false : true);

        if (!GameMenu_Manager.menuIsActive && !Text_Creator.isActive)
        {
            timer += Time.deltaTime;
            saveStep = currentStep;
        }
        else
            currentStep = bossSteps.idle;

        if(!spitActive && currentStep != bossSteps.attack)
            myAnim.Play((boss.velocity.x > 0)? "RightBoss" : (boss.velocity.x < 0)? "LeftBoss" : (boss.velocity.y > 0)? "BackBoss" : (boss.velocity.y < 0)? "FrontBoss" : (currentStep == bossSteps.death)? "DeadBoss" : "IdleBoss");
        else if (currentStep == bossSteps.attack)
            myAnim.Play((boss.transform.position.y > player.transform.position.y) ? "IdleBoss" : "IdleBackBoss");
        else
            myAnim.Play((!directionOnX && boss.transform.position.x < player.transform.position.x) ? "IdleRightBoss" : (!directionOnX && boss.transform.position.x > player.transform.position.x) ? "IdleLeftBoss" : (directionOnX && boss.transform.position.y < player.transform.position.y) ? "IdleBackBoss" : (directionOnX && boss.transform.position.y > player.transform.position.y) ? "IdleBoss" : "IdleBoss");
    }

    #region First Step
    void FirstStep()
    {
        if (timer >= 15f)
        {
            boss.velocity = Vector2.zero;
            follow = false;
            timer = 0;
        }
            
        if(follow)
            PathUpdate();
        else if(!follow && timer >= 4f)
        {
            prefab_Bomb.transform.position = new Vector3 (boss.transform.position.x, boss.transform.position.y -0.5f, 10);
            Instantiate(prefab_Bomb);
            follow = true;
            timer = 0;
        }

        if(life <= currentMaxLife * 0.68f && follow)
            currentStep = bossSteps.secondStep;
    }
    #endregion

    #region Second Step
    void SecondStep()
    {
        velocity = 3;

        if (follow)
            PathUpdate();
        else if (timer >= 11f)
        {
            follow = true;
            spitActive = false;
            timer = 0;
        }

        //Sputa
        if (XSpitDistance() && timer >= maxTimerSpit || YSpitDistance() && timer >= maxTimerSpit)
        {
            follow = false;
            boss.velocity = Vector2.zero;
            spitActive = true;

            StartCoroutine(IstantiateSpit());
            
            timer = 0;
        }
        
        if (life <= currentMaxLife * 0.34f && follow)
            currentStep = bossSteps.thirdStep;
    }

    IEnumerator IstantiateSpit()
    {
        //CREA SPUTO
        for (int i = 0; i < 150; i++)
        {
            while (GameMenu_Manager.menuIsActive)
            {
                yield return new WaitForSeconds(0.2f);
            }
            Instantiate(prefab_Spit);
            yield return new WaitForSeconds(0.05f);
        }
    }

    bool XSpitDistance()
    {
        float bossPos = boss.transform.position.y;
        float playerPos = player.transform.position.y;
        float disPos = (bossPos < 0 && playerPos < 0) ? ((bossPos < playerPos) ? playerPos - bossPos : bossPos - playerPos) : (bossPos > 0 && playerPos > 0) ? ((bossPos > playerPos) ? bossPos - playerPos : playerPos - bossPos) : (bossPos > playerPos) ? bossPos - playerPos : playerPos - bossPos;
        return directionOnX = (boss.transform.position.x >= player.transform.position.x - distance && boss.transform.position.x <= player.transform.position.x + distance && disPos < spitDistance) ? true : false;
    }

    bool YSpitDistance()
    {
        float bossPos = boss.transform.position.x;
        float playerPos = player.transform.position.x;
        float disPos = (bossPos < 0 && playerPos < 0) ? ((bossPos < playerPos) ? playerPos - bossPos : bossPos - playerPos) : (bossPos > 0 && playerPos > 0) ? ((bossPos > playerPos) ? bossPos - playerPos : playerPos - bossPos) : (bossPos > playerPos) ? bossPos - playerPos : playerPos - bossPos;
        return (boss.transform.position.y >= player.transform.position.y - distance && boss.transform.position.y <= player.transform.position.y + distance && disPos < spitDistance) ? true : false;
    }
    #endregion
       
    #region Third Step
    void ThirdStep()
    {
        if (follow)
            PathUpdate();
        else if(timer >= 3)
        {
            follow = true;
            timer = 0;
        }

        //Corre
        if (!run && timer >= 15)
        {
            velocity = 5f;
            timer = 0;
            run = true;
        }
        else if (run && timer >= 2)
        {
            velocity = 2.5f;
            run = false;
            timer = 0;
            follow = false;
            boss.velocity = Vector2.zero;
        }

        if (life <= 0)
        {
            FMOD_Sound_Manager.InGame(12);
            timer = 0;
            currentStep = bossSteps.death;
            UI_Game_Manager.uiCanOpen = false;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player_Collider_Enemy" && follow && currentStep != bossSteps.death)
        {
            attackSaveCurrentStep = currentStep;
            PlayerMovement.underAttack = true;
            currentStep = bossSteps.attack;
        }

        for (int i = 0; i < Shooter_Manager.bulletsAvaiable.Length; i++)
        {
            life -= (collision.name == $"bullet_{i}") ? CollideWithBullet(i) : 0f;
            //Debug.Log(life);
        }
    }

    #region Collide With Bullet
    public float CollideWithBullet(int i)
    {
        if (GameObject.Find($"bullet_{i}") != null)
        {
            GameObject.Find($"bullet_{i}").GetComponent<BulletManager>().takePlayerDirection = true;
            GameObject.Find($"bullet_{i}").SetActive(false);
            //DISEGNA SANGUE
            blood.SetActive(true);
            return Shooter_Manager.damage;
        }
        return 0;
    }
    #endregion

    #region Enemy Attack
    void EnemyAttack()
    {
        boss.velocity = Vector2.zero;
        //anim.Play((myRigidbody.transform.position.y < playerTransform.position.y) ? AttackAnimatorBack : AttackAnimatorFront);
        PlayerMovement.canMove = false;
        UI_Game_Manager.uiCanOpen = false;
        PlayerMovement.bloodDamage.SetActive(true);

        if (!AttackSoundisPlaying)
        {
            AttackSoundisPlaying = true;
            FMOD_Sound_Manager.PlayZombieAttackSound();
        }

        boss.transform.position = (boss.transform.position.y < player.transform.position.y) ? new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, boss.transform.position.z) : new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, boss.transform.position.z);
        if (timerAttack >= 3f)
        {
            AttackSoundisPlaying = false;
            PlayerMovement.canMove = true;
            UI_Game_Manager.uiCanOpen = true;
            PlayerMovement.bloodDamage.SetActive(false);
            PlayerMovement.underAttack = false;
            timerAttack = 0;
            currentStep = bossSteps.notAttack;
        }
        timerAttack += Time.deltaTime;
    }
    #endregion

    #region Not Attack Charge
    void EnemyNotAttackCharge()
    {
        if (!damageToPlayer)
        {
            PlayerLife_Manager.life -= (ChoiceOfDifficultyMenu_Manager.easyMode) ? Random.Range(15, 20) : Random.Range(20, 30);
            damageToPlayer = true;
        }

        timerAttack += Time.deltaTime;
        //anim.Play(StaticAnimator());
        if (timerAttack >= 3f)
        {
            currentStep = attackSaveCurrentStep;
            damageToPlayer = false;
            timerAttack = 0;
        }
    }
    #endregion

    void Death()
    {
        boss.velocity = Vector2.zero;
        blackGround.CrossFadeAlpha(1, 2, true);
        PlayerMovement.canMove = false;

        if (timer >= 10)
        {
            SceneManager.LoadScene("EndGame");
        }
    }
}
