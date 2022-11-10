using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooter_Manager : MonoBehaviour
{
    public static float damage = 1f;

    public static int gunAmmunition = 15;
    public static int maxGunAmmunition = 15;

    public static int ammunitionBox = 15;
    public static int maxAmmunitionBox = 15;

    public static int ShotGunammunition = 4;
    public static int maxShotGunAmmunition = 4;

    public static int magnumAmmunition = 6;
    public static int maxmagnumAmmunition = 6;

    public static GameObject[] bulletsAvaiable = new GameObject[16];
    GameObject player;
    GameObject ammunitionBag;
    float shooterTimer = 2f; //sec.
    float timerCharge = 1.3f; //sec. e si modifica in base all'arma (PER MODIFICARE I PARAMETRI ANDARE NEL METODO CHE GESTISCE LE ARMI)
    float shootModetimer = 0;
    float maxShootModetimer = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ammunitionBag = GameObject.FindGameObjectWithTag("AmmunitionBag");

        for (int i = 0; i < bulletsAvaiable.Length; i++)
        {
            bulletsAvaiable[i] = new GameObject($"bullet_{i}");
            bulletsAvaiable[i].AddComponent<BoxCollider2D>();
            bulletsAvaiable[i].GetComponent<BoxCollider2D>().isTrigger = true;
            bulletsAvaiable[i].GetComponent<BoxCollider2D>().size = new Vector2(0.2f, 0.2f);
            bulletsAvaiable[i].AddComponent<Rigidbody2D>().gravityScale = 0;
            bulletsAvaiable[i].AddComponent<BulletManager>();
            bulletsAvaiable[i].AddComponent<LineRenderer>();
            bulletsAvaiable[i].AddComponent<SpriteRenderer>();
            bulletsAvaiable[i].transform.parent = ammunitionBag.transform;
            bulletsAvaiable[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBoardOrPadController.Key_E && shootModetimer < maxShootModetimer)
            shootModetimer += Time.deltaTime;
        else if (!KeyBoardOrPadController.Key_E)
            shootModetimer = 0;

        if (shootModetimer > maxShootModetimer && KeyBoardOrPadController.Key_Space && shooterTimer >= timerCharge && ControlAmmunitionOfObjEquip() && !PlayerMovement.canMove && ObjectInInventory_Manager.objToEquip != null && !PlayerMovement.triggerEnter && !GameMenu_Manager.menuIsActive)
        {
            ChangeNumberAmm();
            PlayCurrentGunFire();
            
            for (int i = 0; i < bulletsAvaiable.Length; i++)
            {
                if (!bulletsAvaiable[i].activeSelf)
                {
                    bulletsAvaiable[i].SetActive(true);
                    bulletsAvaiable[i].transform.localPosition = Vector2.zero;
                    shooterTimer = 0;
                    break;
                }
            }
        }

        shooterTimer += Time.deltaTime;
    }
    
    void ChangeNumberAmm()
    {
        switch (ObjectInInventory_Manager.objToEquip.name)
        {
            case "Gun": //M11-A1
                if (gunAmmunition > 0)
                {
                    gunAmmunition -= 1;
                    damage = ReturnDamage(20,50,10,20);
                    timerCharge = 0.6f;
                    BulletManager.distanceArea = 7.5f;
                }
                break;

            case "ShotGun": //500 MILLS
                if (ShotGunammunition > 0)
                {
                    ShotGunammunition -= 1;
                    damage = Random.Range(30f, 50f);
                    timerCharge = 1.3f;
                    BulletManager.distanceArea = 4f;
                }
                break;

            case "ColtPython": //Python 357
                if (magnumAmmunition > 0)
                {
                    magnumAmmunition -= 1;
                    damage = ReturnDamage(40, 80, 30, 40);
                    timerCharge = 1.3f;
                    BulletManager.distanceArea = 6f;
                }
                break;

            case null:
                break;
        }
    }

    int ReturnDamage(int minValueEasy, int maxValueEasy, int minValueHard, int maxValueHard)
    {
        if (ChoiceOfDifficultyMenu_Manager.easyMode)
        {
            return (int)Random.Range(minValueEasy, maxValueEasy);
        }
        else
        {
            return (int)Random.Range(minValueHard, maxValueHard);
        }
    }

    bool ControlAmmunitionOfObjEquip()
    {
        if (ObjectInInventory_Manager.objToEquip != null)
        {
            switch (ObjectInInventory_Manager.objToEquip.name)
            {
                case "Gun":
                    if (gunAmmunition > 0)
                        return true;
                    else
                        return false;

                case "ColtPython":
                    if (magnumAmmunition > 0)
                        return true;
                    else
                        return false;
            }
        }       

        return false;
    }

    public static void ResetAllQuantity()
    {
        gunAmmunition = maxGunAmmunition;
        ammunitionBox = maxAmmunitionBox;
        magnumAmmunition = maxmagnumAmmunition;
    }

    static void PlayCurrentGunFire()
    {
        switch (ObjectInInventory_Manager.objToEquip.name)
        {
            case "Gun":
                FMOD_Sound_Manager.PlayGunSound();
                break;

            case "ColtPython":
                FMOD_Sound_Manager.PlayMagnumFireSound();
                break;
        }
    }
}
