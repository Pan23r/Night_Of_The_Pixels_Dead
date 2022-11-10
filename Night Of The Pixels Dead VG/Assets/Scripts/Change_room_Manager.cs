using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change_room_Manager : MonoBehaviour
{
    public string playerDirection;
    public static bool canChange = true;
    public static bool fromKitchen = false;
    public static bool fromBathRoom = false;
    public static bool fromCellar = false;
    public static bool fromLab = false;
    public static bool CellarIsOpen = false; //APRE LA CANTINA
    public static bool fromStairs = false;
    public static bool fromBedRoom = false;
    public static bool fromBedRoomGirl = false;
    public static bool BedRoomGirlIsOpen = false; //APRE LA CAMERA DI SALLY
    public static bool LabIsOpen = false; //APRE IL LABORATORIO
    public static bool toKitchen = false; //Qunado si va dall'atrio alla cucina
    public static bool fromAisleBunker = false;

    static bool closeDoor = false;

    public static void RestoreAll()
    {
        canChange = true;
        fromKitchen = false;
        fromBathRoom = false;
        fromCellar = false;
        fromLab = false;
        CellarIsOpen = false;
        fromStairs = false;
        fromBedRoom = false;
        fromBedRoomGirl = false;
        BedRoomGirlIsOpen = false;
        LabIsOpen = false;
        toKitchen = false;
        fromAisleBunker = false;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.DoorTriggerEnter && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && canChange && PlayerDirectionChangeRoom())
        {
            ReturnRoom();
            Timer_KeyPress_Manager.timer = 0;
        }

        if (closeDoor)
        {
            FMOD_Sound_Manager.PlayCloseDoor();
            closeDoor = false;
        }
    }

    bool PlayerDirectionChangeRoom()
    {
        if (PlayerMovement.currentIdle == GameObject.Find(PlayerMovement.ObjectCollideName).GetComponent<Change_room_Manager>().playerDirection)
            return true;
        else return false;
    }

    private void PlaySoundDoor()
    {
        FMOD_Sound_Manager.PlayOpenDoor();
        closeDoor = true;
    }

    //AGGIUNGERE IL NOME DELLA PORTA CON LA SCENA A CUI E' INDIRIZZATA
    #region Return Room
    private void ReturnRoom()
    {
        switch (PlayerMovement.ObjectCollideName)
        {
            case "Cucina_Door":
                FMOD_Sound_Manager.InGame(3);
                SceneManager.LoadScene("Cucina");
                toKitchen = true;
                PlaySoundDoor();
                break;

            case "Bagno_Door":
                SceneManager.LoadScene("Bagno");
                PlaySoundDoor();
                break;

            case "Cantina_Door":
                FMOD_Sound_Manager.InGame(4);
                SceneManager.LoadScene("Cantina");
                PlaySoundDoor();
                break;

            case "Scale_Door":
                SceneManager.LoadScene("Piano2");
                break;

            case "Atrio_Door_FKitchen":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("AtrioVers2");
                fromKitchen = true;
                PlaySoundDoor();
                break;

            case "ToAtrio_FBath":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("AtrioVers2");
                fromBathRoom = true;
                PlaySoundDoor();
                break;

            case "Atrio_Door_F2Floor":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("AtrioVers2");
                fromStairs = true;
                break;

            case "To2Floor_FBedRoom":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("Piano2");
                fromBedRoom = true;
                PlaySoundDoor();
                break;

            case "BedRoom_Door":
                FMOD_Sound_Manager.InGame(5);
                SceneManager.LoadScene("CameraDaLetto");
                PlaySoundDoor();
                break;

            case "To2Floor_FBedRoomGirl":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("Piano2");
                fromBedRoomGirl = true;
                PlaySoundDoor();
                break;

            case "Child_BedRoom":
                FMOD_Sound_Manager.InGame(7);
                SceneManager.LoadScene("Girl_BedRoom");
                PlaySoundDoor();
                break;
                
            case "Studio_room":
                FMOD_Sound_Manager.InGame(6);
                ObjectInInventory_Manager.SearchObjAndDelete("Chiave Laboratorio");
                SceneManager.LoadScene("Laboratory");
                PlaySoundDoor();
                break;

            case "LabDoorToSecondFloor":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("Piano2");
                fromLab = true;
                PlaySoundDoor();
                break;

            case "FromCellar":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("AtrioVers2");
                fromCellar = true;
                PlaySoundDoor();
                break;

            case "Camino_Text":
                FMOD_Sound_Manager.InGame(0);
                SceneManager.LoadScene("Corridoio_Bunker");
                break;

            case "FromAisleBunkerToAtrio":
                FMOD_Sound_Manager.InGame(2);
                SceneManager.LoadScene("AtrioVers2");
                fromAisleBunker = true;
                break;
                
            case "ToBunkerRoom":
                FMOD_Sound_Manager.InGame(11);
                SceneManager.LoadScene("Bunker_Room");
                break;
        }
    }
    #endregion

    //AGGIUNGERE IL NOME DELLA PORTA OGNI VOLTA CHE NE VIENE CREATA UNA (Ritorna true quando il player entra nel collider della porta inserita)
    #region Return Object Door
    public static bool ReturnObjDoor(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Cucina_Door":
                return true;

            case "Bagno_Door":
                return true;

            case "Cantina_Door":
                if (CellarIsOpen)
                    return true;
                else
                    return false;

            case "Scale_Door":
                return true;

            case "Atrio_Door_FKitchen":
                return true;

            case "ToAtrio_FBath":
                return true;

            case "Atrio_Door_F2Floor":
                return true;

            case "To2Floor_FBedRoom":
                return true;

            case "BedRoom_Door":
                return true;

            case "To2Floor_FBedRoomGirl":
                return true;

            case "Child_BedRoom":
                if (BedRoomGirlIsOpen)
                    return true;
                else
                    return false;

            case "Studio_room":
                if (LabIsOpen)
                    return true;
                else 
                    return false;

            case "LabDoorToSecondFloor":
                return true;

            case "FromCellar":
                return true;

            case "Camino_Text":
                if (FloppyQuestTrigger_Manager.firePlaceIsOpen)
                    return true;
                else
                    return false;

            case "FromAisleBunkerToAtrio":
                return true;

            case "ToBunkerRoom":
                return true;

            default:
                return false;
        }
    }
    #endregion
}
