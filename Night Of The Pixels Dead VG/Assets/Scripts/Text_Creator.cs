using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text_Creator : Text_DirectionPlayer_Manager
{

    //DA FARE: AGGIUNGERE LA DIREZIONE COME NEL CHANGE_ROOM_MANAGER

    public static bool canActive = true;
    public static bool isActive = false; //Verifica se il texbox è attivo

    GameObject textBox;
    GameObject getText;
    public static string putText;

    // Start is called before the first frame update
    void Start()
    {
        textBox = GameObject.FindGameObjectWithTag("TextBox_Player");
        getText = GameObject.FindGameObjectWithTag("Text_inBox_Player");
        if (textBox != null)
            textBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.dialogTriggerEnter && KeyBoardOrPadController.Key_Space && !isActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && canActive && PlayerDirectionText())
        {
            if (textBox != null)
                textBox.SetActive(true);

            UI_Game_Manager.uiCanOpen = false;

            ReturnText();

            if (getText != null)
                getText.GetComponent<TextMeshProUGUI>().text = putText;

            isActive = true;
            Timer_KeyPress_Manager.timer = 0;
            PlayerMovement.canMove = false;
            Enemy_Manager.canMove = false;
            Enemy_Manager_NavMesh.canMove = false;
        }
        else if (PlayerMovement.dialogTriggerEnter && KeyBoardOrPadController.Key_Space && isActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && canActive)
        {
            if (textBox != null)
                textBox.SetActive(false);
            
            UI_Game_Manager.uiCanOpen = true;
            isActive = false;
            Timer_KeyPress_Manager.timer = 0;
            PlayerMovement.canMove = true;
            Enemy_Manager.canMove = true;
            Enemy_Manager_NavMesh.canMove = true;
        }
    }

    //AGGIUNGERE IL NOME DEL GAMEOBJECT CON IL TESTO DA DOVER INSERIRE
    #region Return text
    private void ReturnText()
    {
        switch (PlayerMovement.ObjectCollideName)
        {
            case "Telefono_Text":
                putText = Translation_Manager.phone;
                break;

            case "Tavolini_Text":
                putText = Translation_Manager.lobbyNightTable;
                break;

            case "Piano_Text":
                if(!ObjectInInventory_Manager.SearchObj("MusicSheet_Obj") && !FloppyQuestTrigger_Manager.dontOpenGameMenu)
                    putText = Translation_Manager.piano1;
                else
                    putText = Translation_Manager.piano2;
                break;

            case "Camino_Text":
                putText = Translation_Manager.fireplace;
                break;

            case "Scudo_Text":
                putText = Translation_Manager.shield;
                break;

            case "Spade_Text":
                putText = Translation_Manager.sword;
                break;

            case "Uscita_retro":
                putText = Translation_Manager.rearExit;
                break;

            case "Cantina_Door":
                FMOD_Sound_Manager.PlayLockedDoor();
                putText = Translation_Manager.cellarDoor;
                break;

            case "Libri_daCucina":
                putText = Translation_Manager.cookBook;
                break;

            case "Child_BedRoom":
                FMOD_Sound_Manager.PlayLockedDoor();
                putText = Translation_Manager.sallyDoor;
                break;

            case "Child_Note":
                putText = "-Sally, dato che continui a rincasare tardi, finche' non imparerai che questo non e' un albergo, la tua stanza rimarra' chiusa. Puoi sempre dormire sul divano, tanto sei giovane no?-";
                break;

            case "Studio_room":
                FMOD_Sound_Manager.PlayLockedDoor();
                putText = Translation_Manager.labDoor;
                break;

            case "Libri_Orwell":
                putText = Translation_Manager.orwellBook;
                break;

            case "Cassaforte":
                putText = Translation_Manager.safe;
                break;

            case "Fogli":
                putText = Translation_Manager.sheets;
                break;

            case "ComputerGirl":
                if (!FloppyQuestTrigger_Manager.fileIsTaken)
                    putText = Translation_Manager.computerSally1;
                else
                    putText = Translation_Manager.computerSally2;
                break;

            case "Muro_CameraGirl":
                putText = Translation_Manager.wallSally;
                break;

            case "ComputerLab":
                putText = Translation_Manager.computerLab;
                TriggerSoundOpenCellar.triggerTextOpenCellar = true;
                break;

            case "Karen":
                putText = Translation_Manager.karenBody;
                break;

            case "Tomba_Text":
                putText = Translation_Manager.tomb;
                break;

            case "Dead_Body":
                putText = Translation_Manager.sallyDeadBody;
                break;

            case "DeadBodyMan":
                putText = Translation_Manager.deadBodyMan;
                break;

            case "Door_Bunker":
                FMOD_Sound_Manager.PlayLockedDoor();
                putText = Translation_Manager.doorBunker;
                break;
                
            case "BearSave_Collider":
                putText = Translation_Manager.noPOCtoUse;
                break;
        }
    }
    #endregion

    //AGGIUNGERE IL NOME DELL'OGGETTO CHE FA PARTIRE IL TEXT BOX (PER PLAYERMOVEMENT)
    #region Return Object Text
    public static bool ReturnObjText(Collider2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Telefono_Text":
                return true;

            case "Tavolini_Text":
                return true;

            case "Piano_Text":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.pianoPlayed && !FloppyQuestTrigger_Manager.dontOpenGameMenu || ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.pianoPlayed && !FloppyQuestTrigger_Manager.dontOpenGameMenu && !ObjectInInventory_Manager.SearchObj("MusicSheet_Obj"))
                    return true;
                else
                    return false;

            case "Camino_Text":
                if (!FloppyQuestTrigger_Manager.firePlaceIsOpen)
                    return true;
                else
                    return false;

            case "Scudo_Text":
                return true;

            case "Spade_Text":
                return true;

            case "Uscita_retro":
                return true;

            case "Cantina_Door":
                if (!Change_room_Manager.CellarIsOpen)
                    return true;
                else
                    return false;

            case "Libri_daCucina":
                return true;

            case "Child_BedRoom":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !Change_room_Manager.BedRoomGirlIsOpen || ChoiceOfDifficultyMenu_Manager.easyMode && !ObjectInInventory_Manager.SearchObj("KeySallysRoom_Obj") && !Change_room_Manager.BedRoomGirlIsOpen)
                    return true;
                else
                    return false;

            case "Child_Note":
                return false;

            case "Studio_room":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !Change_room_Manager.LabIsOpen || ChoiceOfDifficultyMenu_Manager.easyMode && !ObjectInInventory_Manager.SearchObj("KeyLab_Obj") && !Change_room_Manager.LabIsOpen)
                    return true;
                else
                    return false;

            case "Libri_Orwell":
                return true;

            /*case "Cassaforte":
                return true;*/

            case "Fogli":
                return true;

            case "ComputerGirl":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.useFloppy && !ObjectInInventory_Manager.SearchObj("MusicSheet_Obj") && !FloppyQuestTrigger_Manager.firePlaceIsOpen || ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.useFloppy && !ObjectInInventory_Manager.SearchObj("FloppyDiskWithMusicSheet_Obj") && !ObjectInInventory_Manager.SearchObj("MusicSheet_Obj") && !FloppyQuestTrigger_Manager.firePlaceIsOpen)
                    return true;
                else
                    return false;

            case "Muro_CameraGirl":
                return true;

            case "ComputerLab":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.fileIsTaken && !FloppyQuestTrigger_Manager.useFloppy || ChoiceOfDifficultyMenu_Manager.easyMode && !FloppyQuestTrigger_Manager.fileIsTaken && !FloppyQuestTrigger_Manager.useFloppy && !ObjectInInventory_Manager.SearchObj("FloppyDisk_Obj"))
                    return true;
                else 
                    return false;

            case "Karen":
                return true;

            case "Tomba_Text":
                if (!ChoiceOfDifficultyMenu_Manager.easyMode && !TakePython_Quest_Manager.canTakeColt || ChoiceOfDifficultyMenu_Manager.easyMode && !TakePython_Quest_Manager.canTakeColt && !ObjectInInventory_Manager.SearchObj("Medallion_Obj"))
                    return true;
                else
                    return false;
                
            case "Dead_Body":
                return true;
                
            case "DeadBodyMan":
                return true;
                
            case "Door_Bunker":
                return true;

            case "BearSave_Collider":
                return OpenSaveMenu_Manager.CreateTextNoPOC();

            default:
                return false;
        }
    }
    #endregion
}
