using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyModUseObj : MonoBehaviour
{
    string objectFind;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBoardOrPadController.Key_Space && !GameMenu_Manager.menuIsActive && ChoiceOfDifficultyMenu_Manager.easyMode && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && ReturnNameObjToUseFromCollider())
        {
            FMOD_Sound_Manager.PlayDecisionInvenctorySound();
            GameObject objToDespawn = GameObject.Find(PlayerMovement.ObjectCollideName);
            objToDespawn.SetActive(false);

            TextBoxObjectTake_Manager.ActivateTextBox(ObjectInInventory_Manager.ReturnMyNameFromNameGameObj(objectFind), false);
            ObjectInInventory_Manager.SearchObjAndDelete(objectFind);
            Timer_KeyPress_Manager.timer = 0;
            objToDespawn.SetActive(true);
        }
    }

    bool ReturnNameObjToUseFromCollider()
    {
        switch (PlayerMovement.ObjectCollideName)
        {
            case "Child_BedRoom":
                if (!Change_room_Manager.BedRoomGirlIsOpen)
                    return Change_room_Manager.BedRoomGirlIsOpen = ObjectInInventory_Manager.SearchObj(objectFind = "KeySallysRoom_Obj");
                else
                    return false;

            case "Studio_room":
                if (!Change_room_Manager.LabIsOpen)
                    return Change_room_Manager.LabIsOpen = ObjectInInventory_Manager.SearchObj(objectFind = "KeyLab_Obj");
                else
                    return false;
                               
            case "ComputerLab":
                if (!FloppyQuestTrigger_Manager.useFloppy)
                    return FloppyQuestTrigger_Manager.useFloppy = ObjectInInventory_Manager.SearchObj(objectFind = "FloppyDisk_Obj");
                else
                    return false;

            case "ComputerGirl":
                if (!FloppyQuestTrigger_Manager.useFloppy)
                    return FloppyQuestTrigger_Manager.useFloppy = ObjectInInventory_Manager.SearchObj(objectFind = "FloppyDiskWithMusicSheet_Obj");
                else
                    return false;

            case "Piano_Text":
                if (ObjectInInventory_Manager.SearchObj(objectFind = "MusicSheet_Obj")) 
                {
                    FloppyQuestTrigger_Manager.dontOpenGameMenu = true;
                    return FloppyQuestTrigger_Manager.useMusicSheet = true;
                }
                else
                    return false;

            case "Tomba_Text":
                if (ObjectInInventory_Manager.SearchObj(objectFind = "Medallion_Obj")) 
                {
                    TakePython_Quest_Manager.canTakeColt = true;
                    FMOD_Sound_Manager.PlayMagnumOpenScompSound();
                    return true;
                }
                else
                    return false;

            default:
                return false;
        }
    }
}
