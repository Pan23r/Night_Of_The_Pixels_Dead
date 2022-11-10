using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager_NavMesh : NavMesh_Manager
{
    float lifeZombie = 100;

    EnemyStep currentStep;
    Rigidbody2D myRigidbody;
    Transform playerTransform;
    Animator anim;
    GameObject blood;
    GameObject deadColliderLeft;
    GameObject deadColliderRight;
    GameObject deadColliderFront;
    GameObject deadColliderBack;
    BoxCollider2D myCollider2D;
    Animator bloodAnim;
    float newdistance = 0.5f;
    //float velocity = 2.5f;
    float decreseVelocity = 0.5f;
    bool checkY = false;
    float distanceArea = 10f;
    bool collisionBulletForce = false;
    float timerStopMove = 0;
    float returnToMove = 0.3f;
    float bloodTimeOfDestroy = 0.5f;
    public static bool canMove = true;
    float timer = 0;
    bool damageToPlayer = false;
    bool setAnimationDead = false;

    //CLIP ANIMATOR NAME
    const string frontClip = "ZombieFront";
    const string backClip = "ZombieBack";
    const string rightClip = "ZombieRight";
    const string leftClip = "ZombieLeft";

    const string staticFrontClip = "ZombieStaticFront";
    const string staticBackClip = "ZombieStaticBack";
    const string staticRightClip = "ZombieStaticRight";
    const string staticLeftClip = "ZombieStaticLeft";

    const string AttackAnimatorBack = "ZombieAttack";
    const string AttackAnimatorFront = "ZombieAttackFront";

    const string DeadAnimationLeft = "ZombieDeadLeft";
    const string DeadAnimationRight = "ZombieDeadRight";
    const string DeadAnimationFront = "ZombieDeadFront";
    const string DeadAnimationBack = "ZombieDeadBack";

    float soundTimer;
    bool setTimer = true;
    bool AttackSoundisPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        currentStep = EnemyStep.follow;
        canMove = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myRigidbody.freezeRotation = true;
        blood = GameObject.Find($"{gameObject.name}_Blood");
        deadColliderLeft = GameObject.Find($"{gameObject.name}_DeadCollider_Left");
        deadColliderRight = GameObject.Find($"{gameObject.name}_DeadCollider_Right");
        deadColliderBack = GameObject.Find($"{gameObject.name}_DeadCollider_Back");
        deadColliderFront = GameObject.Find($"{gameObject.name}_DeadCollider_Front");
        myCollider2D = GameObject.Find("Player_Collider_Enemy").GetComponent<BoxCollider2D>();
        bloodAnim = blood.GetComponent<Animator>();

        NavMeshStart();

        blood.SetActive(false);
        deadColliderLeft.SetActive(false);
        deadColliderRight.SetActive(false);
        deadColliderFront.SetActive(false);
        deadColliderBack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        switch (currentStep)
        {
            case EnemyStep.follow:
                EnemyFollow();
                PlaySoundClip();
                break;

            case EnemyStep.attack:
                EnemyAttack();
                break;

            case EnemyStep.notAttack:
                EnemyNotAttackCharge();
                break;

            case EnemyStep.death:
                EnemyDead();
                break;
        }
    }

    void EnemyAttack()
    {
        myRigidbody.velocity = Vector2.zero;
        anim.Play((myRigidbody.transform.position.y < playerTransform.position.y) ? AttackAnimatorBack : AttackAnimatorFront);
        PlayerMovement.canMove = false;
        UI_Game_Manager.uiCanOpen = false;
        PlayerMovement.bloodDamage.SetActive(true);

        if (!AttackSoundisPlaying)
        {
            AttackSoundisPlaying = true;
            FMOD_Sound_Manager.PlayZombieAttackSound();
        }

        myRigidbody.transform.position = (myRigidbody.transform.position.y < playerTransform.position.y) ? new Vector3(playerTransform.position.x, playerTransform.position.y - 0.5f, myRigidbody.transform.position.z) : new Vector3(playerTransform.position.x, playerTransform.position.y + 0.5f, myRigidbody.transform.position.z);
        
        if (timer >= 3f)
        {
            AttackSoundisPlaying = false;
            PlayerMovement.canMove = true;
            UI_Game_Manager.uiCanOpen = true;
            PlayerMovement.bloodDamage.SetActive(false);
            PlayerMovement.underAttack = false;
            myCollider2D.enabled = false;
            timer = 0;
            currentStep = EnemyStep.notAttack;
        }
        timer += Time.deltaTime;
    }

    void EnemyNotAttackCharge()
    {
        anim.Play(StaticAnimator());

        if (!damageToPlayer)
        {
            PlayerLife_Manager.life -= (ChoiceOfDifficultyMenu_Manager.easyMode) ? Random.Range(5, 10) : Random.Range(10, 20);
            damageToPlayer = true;
        }

        if (timer >= 3f)
        {
            damageToPlayer = false;
            myCollider2D.enabled = true;
            timer = 0;
            currentStep = EnemyStep.follow;
        }        

        timer += Time.deltaTime;
    }

    void EnemyFollow()
    {
        if (!collisionBulletForce && canMove && PlayerInAreaView())
        {
            NavMeshUpdate();
            anim.Play(myAnimator());
        }
        else if (collisionBulletForce)
        {
            if (myRigidbody.velocity.x != 0)
                myRigidbody.velocity = (myRigidbody.velocity.x > 0) ? new Vector2(decreseVelocity, 0) : (myRigidbody.velocity.x < 0) ? new Vector2(-decreseVelocity, 0) : Vector2.zero;

            if (myRigidbody.velocity.y != 0)
                myRigidbody.velocity = (myRigidbody.velocity.y > 0) ? new Vector2(0, decreseVelocity) : (myRigidbody.velocity.y < 0) ? new Vector2(0, -decreseVelocity) : Vector2.zero;

            timerStopMove += Time.deltaTime;
            collisionBulletForce = (timerStopMove > returnToMove) ? ReturnToMove() : true;
        }
        else
        {
            agent.velocity = Vector3.zero;
            myRigidbody.velocity = Vector2.zero;
            anim.Play(StaticAnimator());
        }

        blood.SetActive((blood.activeSelf && bloodAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > bloodTimeOfDestroy) ? false : (!blood.activeSelf) ? false : true);
    }

    void EnemyDead()
    {
        myRigidbody.velocity = Vector2.zero;

        if (!setAnimationDead)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            string animation = CurrentDeadAnimation();
            anim.Play(animation);
            ActiveDeadCollider(animation);
            setAnimationDead = true;
        }

        blood.SetActive(false);
    }

    void ActiveDeadCollider(string animation)
    {
        deadColliderLeft.SetActive(animation == DeadAnimationLeft ? true : false);
        deadColliderRight.SetActive(animation == DeadAnimationRight ? true : false);
        deadColliderBack.SetActive(animation == DeadAnimationBack ? true : false);
        deadColliderFront.SetActive(animation == DeadAnimationFront ? true : false);
    }

    bool ReturnToMove()
    {
        timerStopMove = 0;
        return false;
    }

    private void LateUpdate()
    {
        if (lifeZombie <= 0)
        {
            //this.gameObject.SetActive(false);
            DesactivateSpawnOfThisEnemy();
            currentStep = EnemyStep.death;
        }
    }

    string CurrentDeadAnimation()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(frontClip) ? DeadAnimationFront : anim.GetCurrentAnimatorStateInfo(0).IsName(backClip) ? DeadAnimationBack : anim.GetCurrentAnimatorStateInfo(0).IsName(rightClip) ? DeadAnimationRight : anim.GetCurrentAnimatorStateInfo(0).IsName(leftClip) ? DeadAnimationLeft : DeadAnimationFront;
    }

    string StaticAnimator()
    {
        return (canMove ? staticFrontClip : CurrentAnimation());
    }

    string CurrentAnimation()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(frontClip) ? staticFrontClip : anim.GetCurrentAnimatorStateInfo(0).IsName(backClip) ? staticBackClip : anim.GetCurrentAnimatorStateInfo(0).IsName(rightClip) ? staticRightClip : anim.GetCurrentAnimatorStateInfo(0).IsName(leftClip) ? staticLeftClip : null;
    }

    bool PlayerInAreaView()
    {
        return (Vector2.Distance(transform.position, playerTransform.position) < distanceArea) ? true : false;
    }

    bool CheckDistance(float transformEnemy, float transformPlayer, bool checkY, bool returnTrue, bool returnFalse)
    {
        return (transformEnemy > transformPlayer - newdistance && transformEnemy < transformPlayer + newdistance && checkY) ? returnTrue : returnFalse;
    }

    string myAnimator()
    {
        if (goToX) //!checkY
            return (agent.velocity.x > 0) ? rightClip : (agent.velocity.x < 0) ? leftClip : null;
        else
            return (agent.velocity.y > 0) ? backClip : (agent.velocity.y < 0) ? frontClip : null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < Shooter_Manager.bulletsAvaiable.Length; i++)
        {
            lifeZombie -= (collision.name == $"bullet_{i}") ? CollideWithBullet(i) : 0f;
        }

        if (collision.name == "Player_Collider_Enemy" && currentStep != EnemyStep.death && !collisionBulletForce && !PlayerMovement.underAttack)
        {
            currentStep = EnemyStep.attack;
            PlayerMovement.underAttack = true;
        }
    }

    public float CollideWithBullet(int i)
    {
        if (GameObject.Find($"bullet_{i}") != null)
        {
            GameObject.Find($"bullet_{i}").GetComponent<BulletManager>().takePlayerDirection = true;
            GameObject.Find($"bullet_{i}").SetActive(false);
            //DISEGNA SANGUE
            blood.SetActive(true);
            //USARE IMPULSO PER SPINGERE LO ZOMBIE
            collisionBulletForce = true;
            return Shooter_Manager.damage;
        }
        return 0;
    }

    void DesactivateSpawnOfThisEnemy()
    {
        switch (gameObject.name)
        {
            case "Zombie_0":
                Spawn_Enemy_Manager.SpawnZombie[0] = false;
                break;

            case "Zombie_1":
                Spawn_Enemy_Manager.SpawnZombie[1] = false;
                break;

            case "Zombie_2":
                Spawn_Enemy_Manager.SpawnZombie[2] = false;
                break;

            case "Zombie_3":
                Spawn_Enemy_Manager.SpawnZombie[3] = false;
                break;

            case "Zombie_4":
                Spawn_Enemy_Manager.SpawnZombie[4] = false;
                break;

            case "Zombie_5":
                Spawn_Enemy_Manager.SpawnZombie[5] = false;
                break;
        }
    }

    void PlaySoundClip()
    {
        if (setTimer)
        {
            soundTimer = Random.Range(15, 25);
            setTimer = false;
        }
        else if (soundTimer <= 0)
        {
            setTimer = true;
            FMOD_Sound_Manager.PlayZombieSound();
        }

        soundTimer -= Time.deltaTime;
    }
}

