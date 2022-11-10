using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public static bool isOpen = false;
    public static GameObject Obj_returnToMenu;

    TextMeshProUGUI yes_Text;
    TextMeshProUGUI no_Text;
    bool question = true;
    
    // Start is called before the first frame update
    void Start()
    {
        yes_Text = GameObject.Find("Yes").GetComponent<TextMeshProUGUI>();
        no_Text = GameObject.Find("No").GetComponent<TextMeshProUGUI>();
        Obj_returnToMenu = GameObject.Find("ReturnToMainMenu");
        yes_Text.color = Color.yellow;

        if (Obj_returnToMenu != null)
        Obj_returnToMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            question = false;
            no_Text.color = Color.yellow;
            yes_Text.color = Color.white;
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            question = true;
            no_Text.color = Color.white;
            yes_Text.color = Color.yellow;
            Timer_KeyPress_Manager.timer = 0;
        }

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && question)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            //RestoreAllVariables();
            question = true;
            isOpen = false;
            FMOD_Sound_Manager.Stop();
            SceneManager.LoadScene("Menu");
            Timer_KeyPress_Manager.timer = 0;
        }
        else if(KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !question)
        {
            FMOD_Sound_Manager.PlayBackSound();
            GameMenu_Manager.menuInGame.SetActive(true);
            Timer_KeyPress_Manager.timer = 0;
            isOpen = false;
            question = true;
            no_Text.color = Color.white;
            yes_Text.color = Color.yellow;
            Obj_returnToMenu.SetActive(false);
        }
    }

    /*public static void RestoreAllVariables()
    {
        PlayerMovement.canMove = true;
        ObjectIsTakeDelete_manager.RestoreAll();
        Change_room_Manager.RestoreAll();
        StrongBox_manager.RestoreAll();
        TriggerSoundOpenCellar.RestoreAll();
        VideoManager.RestoreAll();
        FloppyQuestTrigger_Manager.RestoreAll();
        Spawn_Enemy_Manager.RestoreAll();
        UI_Game_Manager.uiCanOpen = true;
        Text_Creator.canActive = true;

        for (int i = 0; i < 7; i++)
        {
            ObjectInInventory_Manager.DeleteObject(i);
        }

        PlayerLife_Manager.life = PlayerLife_Manager.maxLife;
    }*/
}
