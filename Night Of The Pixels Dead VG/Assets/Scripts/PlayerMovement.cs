using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

enum PlayerSteps { idle, move, dead}
public class PlayerMovement : MonoBehaviour
{
    PlayerSteps currentStep;
    Rigidbody2D MyRigidBody;
    float originalSpeed = 4f; //Velocità in camminata
    float speedRun = 8f; //Velocità in corsa
    float speed;
    static Animator anim;
    bool aimMode = false;
    
    public static string currentIdle = "Front";
    public static bool triggerEnter = false;
    public static bool dialogTriggerEnter = false;
    public static string ObjectCollideName;
    public static GameObject objectCollide;
    public static GameObject questionMarkCollider;
    public static GameObject bloodDamage;
    public static bool DoorTriggerEnter = false;
    public static bool canMove = true;
    public static bool takebleObjTrigger = false;
    public static bool underAttack = false;

    public static float roomMaxPosX, roomMinPosX, roomMaxPosY, roomMinPosY;

    //Guns for Aim
    GameObject GunRight;
    GameObject GunLeft;
    GameObject GunFront;

    //VAR TIMER RUN
    float timer;
    bool timerCountDown = false;
    float maxTimer = 0.8f; //tempo che si può correre
    float originalchargeTime = 5f; //tempo di recupero della corsa
    float chargeTime; //Variabile che viene diminuita con il passare del tempo
    bool shiftPress = false;

    public float minPositionX; //punto minimo raggiungibile nella mappa in X
    public float maxPositionX; //punto massimo raggiungibile nella mappa in X
    public float minPositionY; //punto minimo raggiungibile nella mappa in Y
    public float maxPositionY; //punto massimo raggiungibile nella mappa in Y

    bool activateQuestionMarkWhenCollide = true;
    
    // Start is called before the first frame update
    void Start()
    {
        activateQuestionMarkWhenCollide = ChoiceOfDifficultyMenu_Manager.easyMode;
        currentStep = PlayerSteps.move;

        currentIdle = "Front";
        roomMaxPosX = maxPositionX;
        roomMinPosX = minPositionX;
        roomMaxPosY = maxPositionY;
        roomMinPosY = minPositionY;

        MyRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MyRigidBody.freezeRotation = true; //impedisce al player di ruotare su se stesso quando collide con un altro oggetto
        questionMarkCollider = GameObject.Find("QuestionMark");
        questionMarkCollider.SetActive(false);


        GunRight = GameObject.Find("Player_Aim_Gun_Right");
        GunLeft = GameObject.Find("Player_Aim_Gun_Left");
        GunFront = GameObject.Find("Player_Aim_Gun_Front");

        GunRight.SetActive(false);
        GunLeft.SetActive(false);
        GunFront.SetActive(false);

        bloodDamage = GameObject.Find("Blood_Player");
        bloodDamage.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Bagno" || SceneManager.GetActiveScene().name == "CameraDaLetto")
        {
            ChangeAnimAndIdle("PlayerWalkBack", "Back");
        }

        if (Change_room_Manager.toKitchen && SceneManager.GetActiveScene().name == "Cucina")
        {
            MyRigidBody.transform.position = new Vector3(5.07f, -6.53f, 0);
            ChangeAnimAndIdle("PlayerWalkBack", "Back");
            Change_room_Manager.toKitchen = false;
        }

        if (SceneManager.GetActiveScene().name == "Girl_BedRoom" || SceneManager.GetActiveScene().name == "Laboratory")
        {
            ChangeAnimAndIdle("PlayerWalkRight", "Right");
        }

        if (Change_room_Manager.fromLab)
        {
            ChangeAnimAndIdle("PlayerWalkLeft", "Left");
        }

        //IN CASO DI CARICAMENTO
        if (Load_Manager.loadGame)
        {
            XmlTextReader XmlTR = new XmlTextReader($"{Load_Manager.pathSaveSlot}{Load_Manager.fileToLoad}");

            while (XmlTR.Read())
            {
                //CAMBIARE POSIZIONE 
                if (XmlTR.MoveToAttribute("positionX"))
                {
                    MyRigidBody.transform.position = new Vector2(XmlTR.ReadContentAsFloat(), MyRigidBody.transform.position.y);
                }
                else if (XmlTR.MoveToAttribute("positionY"))
                {
                    MyRigidBody.transform.position = new Vector2(MyRigidBody.transform.position.x, XmlTR.ReadContentAsFloat());
                }
            }
            
            ChangeAnimAndIdle("PlayerWalkBack", "Back");

            Load_Manager.loadGame = false;
        }

        SetIdleAnimation(-1);
    }

    public static void ChangeAnimAndIdle(string animName, string idleName)
    {
        anim.Play(animName);
        currentIdle = idleName;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStep)
        {
            case PlayerSteps.idle:
                PlayerIdle();
                break;

            case PlayerSteps.move:
                PlayerMove();
                break;

            case PlayerSteps.dead:
                PlayerDead();
                break;
        }
    }
    
    void PlayerIdle()
    {
        MyRigidBody.velocity = Vector2.zero;

        if (KeyBoardOrPadController.Key_E_Released && aimMode)
        {
            if (!GameMenu_Manager.menuIsActive)
                canMove = true;

            aimMode = false;
            GunFront.SetActive(false);
            GunRight.SetActive(false);
            GunLeft.SetActive(false);
        }

        if (!KeyBoardOrPadController.Key_E || KeyBoardOrPadController.Key_E && ObjectInInventory_Manager.objToEquip == null || GameMenu_Manager.menuIsActive || underAttack)
        {
            GunFront.SetActive(false);
            GunRight.SetActive(false);
            GunLeft.SetActive(false);
            SetIdleAnimation(0.75f);
        }

        if (canMove)
            currentStep = PlayerSteps.move;
    }

    void PlayerMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        anim.speed = 1;
        speed = originalSpeed;

        //RUN
        if (KeyBoardOrPadController.Key_Shift && !timerCountDown)
        {
            speed = speedRun;
            shiftPress = true;
        }
        else
        {
            shiftPress = false;
        }

        //TIMER COUNT DOWN
        if (timer >= maxTimer && !timerCountDown)
            timerCountDown = true;

        if (timerCountDown || !KeyBoardOrPadController.Key_Shift)
            chargeTime -= Time.deltaTime;

        if (chargeTime <= 0)
        {
            chargeTime = originalchargeTime; //la ricarica torna al suo stato originale
            timer = 0; //il timer torna a 0 per poi essere aumentato quando si preme shift
            timerCountDown = false; //diventa false per far funzionare nuovamente lo shift
        }

        #region Aim
        if (!KeyBoardOrPadController.Key_W && !KeyBoardOrPadController.Key_A && !KeyBoardOrPadController.Key_S && !KeyBoardOrPadController.Key_D && KeyBoardOrPadController.Key_E && ObjectInInventory_Manager.objToEquip != null && !GameMenu_Manager.menuIsActive)
        {
            switch (currentIdle)
            {
                case "Left":
                    anim.Play("PlayerAimLeft", -1);
                    GunLeft.SetActive(true);
                    break;

                case "Right":
                    anim.Play("PlayerAimRight", -1);
                    GunRight.SetActive(true);
                    break;

                case "Front":
                    anim.Play("PlayerAimFront", -1);
                    GunFront.SetActive(true);
                    break;

                case "Back":
                    anim.Play("PlayerAimBack", -1);
                    break;
            }

            aimMode = true;
            canMove = false;
        }
        #endregion

        #region Player Movement
        //PLAYER MOVEMENT
        if (KeyBoardOrPadController.Key_A)
        {
            anim.Play("PlayerWalkLeft", -1);
            MyRigidBody.velocity = new Vector2(-speed, 0);
            currentIdle = "Left";

            if (shiftPress)
                timer += Time.deltaTime;
        }
        else if (KeyBoardOrPadController.Key_D)
        {
            anim.Play("PlayerWalkRight", -1);
            MyRigidBody.velocity = new Vector2(speed, 0);
            currentIdle = "Right";

            if (shiftPress)
                timer += Time.deltaTime;
        }
        else if (KeyBoardOrPadController.Key_S)
        {
            anim.Play("PlayerWalkFront", -1);
            MyRigidBody.velocity = new Vector2(0, -speed);
            currentIdle = "Front";

            if (shiftPress)
                timer += Time.deltaTime;
        }
        else if (KeyBoardOrPadController.Key_W)
        {
            anim.Play("PlayerWalkBack", -1);
            MyRigidBody.velocity = new Vector2(0, speed);
            currentIdle = "Back";

            if (shiftPress)
                timer += Time.deltaTime;
        }
        else
        {
            MyRigidBody.velocity = Vector2.zero;

            if (!KeyBoardOrPadController.Key_E || KeyBoardOrPadController.Key_E && ObjectInInventory_Manager.objToEquip == null || GameMenu_Manager.menuIsActive)
                SetIdleAnimation(0.75f);

            //anim.speed = 0;
        }
        #endregion

        if (PlayerLife_Manager.life <= 0)
            currentStep = PlayerSteps.dead;

        if(!canMove)
            currentStep = PlayerSteps.idle;

        anim.SetFloat("Horizontal", MyRigidBody.velocity.x);
        anim.SetFloat("Vertical", MyRigidBody.velocity.y);

    }

    void PlayerDead()
    {
        canMove = false;
        UI_Game_Manager.uiCanOpen = false;
        MyRigidBody.velocity = Vector2.zero; 
        anim.Play("PlayerDead", -1);
        bloodDamage.SetActive(false);
    }

    #region Set Idle Aniamtion
    void SetIdleAnimation(float changeTime)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > changeTime && !anim.IsInTransition(0)) //Fa in modo che l'animazione arrivi fino al primo stop prima di essere cambiata (questo quando changeTime è 0.75f)
        {
            switch (currentIdle)
            {
                case "Front":
                    anim.Play("PlayerIdleFront", -1);
                    break;

                case "Back":
                    anim.Play("PlayerIdleBack", -1);
                    break;

                case "Right":
                    anim.Play("PlayerIdleRight", -1);
                    break;

                case "Left":
                    anim.Play("PlayerIdleLeft", -1);
                    break;
            }
        }       
    }
    #endregion

    private void LateUpdate()
    {
        if(transform.position.x <= minPositionX && MyRigidBody.velocity.x < 0 || transform.position.x >= maxPositionX && MyRigidBody.velocity.x > 0 ||
            transform.position.y <= minPositionY && MyRigidBody.velocity.y < 0 || transform.position.y >= maxPositionY && MyRigidBody.velocity.y > 0)
        {
            MyRigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerEnter = true;

        if (Text_Creator.ReturnObjText(collision))
        {
            dialogTriggerEnter = true;
            ActivateQuestionMark();
        }

        if (Change_room_Manager.ReturnObjDoor(collision))
        {
            DoorTriggerEnter = true;
            ActivateQuestionMark();
        }

        if (ObjectIstantiator_Manager.ObjToTake(collision))
        {
            takebleObjTrigger = true;
            ActivateQuestionMark();
        }

        if (FloppyQuestTrigger_Manager.CollideWithObj(collision))
        {
            FloppyQuestTrigger_Manager.enterTrigger = true;
            ActivateQuestionMark();
        }

        if (TextNote_Manager.ReturneNote(collision))
        {
            TextNote_Manager.triggerIsActive = true;
            ActivateQuestionMark();
        }


        if (collision.gameObject.name == "BearSave_Collider") //VERIFICA SE SI COLLIDE CON L'ORSO DI SALVATAGGIO
        {
            Save_Manager.canOpenSave = true;
            ActivateQuestionMark();
        }

        if (collision.gameObject.name == "Cassaforte")
        {
            StrongBox_manager.canOpen = true;
            ActivateQuestionMark();
        }

        if (collision.gameObject.name == "Tomba_Text" && !TakePython_Quest_Manager.coltIsTaken)
        {
            ActivateQuestionMark();
        }

        if (ChoiceOfDifficultyMenu_Manager.easyMode && collision.gameObject.name == "Child_BedRoom" || ChoiceOfDifficultyMenu_Manager.easyMode && collision.gameObject.name == "Studio_room")
        {
            ActivateQuestionMark();
        }

        ObjectCollideName = collision.gameObject.name;
        objectCollide = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerEnter = false;
        dialogTriggerEnter = false;
        DoorTriggerEnter = false;
        takebleObjTrigger = false;
        StrongBox_manager.canOpen = false;
        Save_Manager.canOpenSave = false;
        FloppyQuestTrigger_Manager.enterTrigger = false;
        TextNote_Manager.triggerIsActive = false;
        questionMarkCollider.SetActive(false);
    }

    void ActivateQuestionMark()
    {
        if(activateQuestionMarkWhenCollide)
            questionMarkCollider.SetActive(true);
    }
}
