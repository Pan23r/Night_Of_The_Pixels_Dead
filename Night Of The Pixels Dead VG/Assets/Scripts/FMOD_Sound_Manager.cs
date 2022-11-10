using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class FMOD_Sound_Manager : MonoBehaviour
{
    public GameObject ObjEmitter;
    public static VCA vca;
    static StudioEventEmitter musicEmitter;
    static StudioEventEmitter cursor_Snd;
    static StudioEventEmitter decision_Snd;
    static StudioEventEmitter cellarIsOp_Snd;

    static StudioEventEmitter gunFire_Snd;
    static StudioEventEmitter magnumFire_Snd;
    static StudioEventEmitter takeObj_Snd;

    static StudioEventEmitter openDoor_snd;
    static StudioEventEmitter closeDoor_snd;
    static StudioEventEmitter lockedDoor_snd;

    static StudioEventEmitter deadBarbara_snd;

    //Invenctory Sound
    static StudioEventEmitter decisionInv_Snd;
    static StudioEventEmitter cursorInv_Snd;
    static StudioEventEmitter equip_Snd;
    static StudioEventEmitter unequip_Snd;
    static StudioEventEmitter null_Snd;
    static StudioEventEmitter back_Snd;
    static StudioEventEmitter fileReader_Snd;
    static StudioEventEmitter fileOpenAndClose_Snd;

    //StrongBox Sound
    static StudioEventEmitter verifytrueSB_Snd;
    static StudioEventEmitter verifyfalseSB_Snd;
    static StudioEventEmitter cursorSB_Snd;
    static StudioEventEmitter SelectSB_Snd;
    
    static StudioEventEmitter openMagnumScomp_Snd;

    //Enemy Sound
    static StudioEventEmitter zombie_Snd;

    //Enemy Attack
    static StudioEventEmitter zombieAttack_snd;

    // Start is called before the first frame update
    void Start()
    {
        vca = RuntimeManager.GetVCA("vca:/MasterVolume");

        if(SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "MenuDifficoltà" || SceneManager.GetActiveScene().name == "Girl_BedRoom" && Load_Manager.playMusicOnLoad)
            musicEmitter = ObjEmitter.GetComponent<StudioEventEmitter>();

        cursor_Snd = GameObject.Find("Cursor_snd").GetComponent<StudioEventEmitter>();
        decision_Snd = GameObject.Find("Decision_snd").GetComponent<StudioEventEmitter>();
        null_Snd = GameObject.Find("Null_snd").GetComponent<StudioEventEmitter>();

        if (GameObject.Find("ZombieSnd") != null)
            zombie_Snd = GameObject.Find("ZombieSnd").GetComponent<StudioEventEmitter>();

        if (SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "MenuDifficoltà")
        {
            decisionInv_Snd = GameObject.Find("DecisionInvenctory_snd").GetComponent<StudioEventEmitter>();
            cursorInv_Snd = GameObject.Find("CursorInvenctory_snd").GetComponent<StudioEventEmitter>();
            equip_Snd = GameObject.Find("Equip_snd").GetComponent<StudioEventEmitter>();
            unequip_Snd = GameObject.Find("Unequip_snd").GetComponent<StudioEventEmitter>();
            gunFire_Snd = GameObject.Find("GunFIreSound").GetComponent<StudioEventEmitter>();
            fileReader_Snd = GameObject.Find("NoteBox_snd").GetComponent<StudioEventEmitter>();
            fileOpenAndClose_Snd = GameObject.Find("FileOpenAndClose_Sound").GetComponent<StudioEventEmitter>();
            takeObj_Snd = GameObject.Find("TakeObj_snd").GetComponent<StudioEventEmitter>();
            openDoor_snd = GameObject.Find("OpenDoor_snd").GetComponent<StudioEventEmitter>();
            closeDoor_snd = GameObject.Find("CloseDoor_snd").GetComponent<StudioEventEmitter>();
            lockedDoor_snd = GameObject.Find("LockDoor_snd").GetComponent<StudioEventEmitter>();
            deadBarbara_snd = GameObject.Find("Dead_Barbara_snd").GetComponent<StudioEventEmitter>();
            magnumFire_Snd = GameObject.Find("MagnumFire_snd").GetComponent<StudioEventEmitter>();
            openMagnumScomp_Snd = GameObject.Find("MagnumOpenScomp_snd").GetComponent<StudioEventEmitter>();
        }   
        
        if(SceneManager.GetActiveScene().name == "CameraDaLetto")
        {
            verifytrueSB_Snd = GameObject.Find("VerifyTrue_snd").GetComponent<StudioEventEmitter>();
            verifyfalseSB_Snd = GameObject.Find("VerifyFalse_snd").GetComponent<StudioEventEmitter>();
            cursorSB_Snd = GameObject.Find("Button_snd").GetComponent<StudioEventEmitter>();
            SelectSB_Snd = GameObject.Find("SelectButton_snd").GetComponent<StudioEventEmitter>();
        }

        if(SceneManager.GetActiveScene().name != "MenuDifficoltà")
            back_Snd = GameObject.Find("Back_snd").GetComponent<StudioEventEmitter>();

        if (SceneManager.GetActiveScene().name == "Laboratory")
            cellarIsOp_Snd = GameObject.Find("TriggerCellar").GetComponent<StudioEventEmitter>();

        if(GameObject.Find("Blood_Player") != null)
            zombieAttack_snd = GameObject.Find("Blood_Player").GetComponent<StudioEventEmitter>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Girl_BedRoom" && Load_Manager.playMusicOnLoad)
        {
            Play();
            InGame(7);
            Load_Manager.playMusicOnLoad = false;
        }
    }

    static void CreatePlay(StudioEventEmitter emitter)
    {
        if (emitter != null)
            emitter.Play();
        else
            Debug.LogError("EMITTER: NULL");
    }

    public static void PlayZombieAttackSound()
    {
        CreatePlay(zombieAttack_snd);
    }

    public static void PlayZombieSound()
    {
        CreatePlay(zombie_Snd);
    }


    public static void PlayMagnumFireSound()
    {
        CreatePlay(magnumFire_Snd);
    }

    public static void PlayMagnumOpenScompSound()
    {
        CreatePlay(openMagnumScomp_Snd);
    }

    public static void PlayDeadBarbaraSound()
    {
        CreatePlay(deadBarbara_snd);
    }

    public static void PlayVerifyTrueSB()
    {
        CreatePlay(verifytrueSB_Snd);
    }

    public static void PlayVerifyFalseSB()
    {
        CreatePlay(verifyfalseSB_Snd);
    }

    public static void PlaySlectButtonSB()
    {
        CreatePlay(SelectSB_Snd);
    }

    public static void PlayCursorButtonSB()
    {
        CreatePlay(cursorSB_Snd);
    }

    public static void PlayLockedDoor()
    {
        CreatePlay(lockedDoor_snd);
    }

    public static void PlayOpenDoor()
    {
        CreatePlay(openDoor_snd);
    }

    public static void PlayCloseDoor()
    {
        CreatePlay(closeDoor_snd);
    }

    public static void PlayTakeObjSound()
    {
        CreatePlay(takeObj_Snd);
    }

    public static void PlayBackSound()
    {
        CreatePlay(back_Snd);
    }

    public static void PlayNoteOpenAndCloseSound()
    {
        CreatePlay(fileOpenAndClose_Snd);
    }

    public static void PlayNoteSound()
    {
        CreatePlay(fileReader_Snd);
    }

    public static void PlayDecisionInvenctorySound()
    {
        CreatePlay(decisionInv_Snd);
    }

    public static void PlayGunSound()
    {
        CreatePlay(gunFire_Snd);
    }

    public static void PlayCursorInvenctorySound()
    {
        CreatePlay(cursorInv_Snd);
    }

    public static void PlayEquipSound()
    {
        CreatePlay(equip_Snd);
    }

    public static void PlayUnequipSound()
    {
        CreatePlay(unequip_Snd);
    }

    public static void PlayCursorSound()
    {
        CreatePlay(cursor_Snd);
    }

    public static void PlayNullSound()
    {
        CreatePlay(null_Snd);
    }

    public static void PlayDecisionSound()
    {
        CreatePlay(decision_Snd);
    }

    public static void PlayCellarOpenSound()
    {
        CreatePlay(cellarIsOp_Snd);
    }

    public static void GameMenu(int parameter)
    {
        musicEmitter.SetParameter("CurrentMenu", parameter);
    }

    public static void InGame(int parameter)
    {
        musicEmitter.SetParameter("NewGameSoundManager", parameter);
    }

    public static void Stop()
    {
        musicEmitter.Stop();
    }

    public static void Play()
    {
        CreatePlay(musicEmitter);
    }
}
