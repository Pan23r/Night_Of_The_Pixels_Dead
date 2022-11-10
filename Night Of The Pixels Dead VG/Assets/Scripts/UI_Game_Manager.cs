using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Game_Manager : MonoBehaviour
{
    public static bool uiCanOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && uiCanOpen &&!Save_Manager.canOpenSave && !StrongBox_manager.canOpen && !FloppyQuestTrigger_Manager.dontOpenGameMenu && !VideoManager.videoIsPlaying && !TextNote_Manager.noteBox.activeSelf)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            GameMenu_Manager.menuInGame.SetActive(true);
            GameMenu_Manager.menuIsActive = true;
            PlayerMovement.canMove = false;
            Enemy_Manager.canMove = false;
            Enemy_Manager_NavMesh.canMove = false;
            Text_Creator.canActive = false;
            Change_room_Manager.canChange = false;
            Timer_KeyPress_Manager.timer = 0;
            uiCanOpen = false;
        }

        //CASSAFORTE
        if (StrongBox_manager.strongBox != null && !StrongBox_manager.strongBox.activeSelf && StrongBox_manager.canOpen && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && PlayerMovement.currentIdle == "Back")
        {
            if (!StrongBox_manager.StrongBoxIsOpen)
            {
                StrongBox_manager.strongBox.SetActive(true);
                Timer_KeyPress_Manager.timer = 0;
            }
            else
            {
                if (!ObjectIsTakeDelete_manager.keyLabIsTaken)
                {
                    ObjectIsTakeDelete_manager.keyLabIsTaken = ObjectInInventory_Manager.PutObject(Translation_Manager.nameLabKey, "KeyLab_Obj","Key_Lab", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);
                    //Debug.Log("PRESA CHIAVE LABORATORIO" + ObjectIsTakeDelete_manager.keyLabIsTaken);
                    Destroy(GameObject.Find("Cassaforte"));
                    FMOD_Sound_Manager.PlayTakeObjSound();
                }
                Timer_KeyPress_Manager.timer = 0;
            }
        }
    }
}
