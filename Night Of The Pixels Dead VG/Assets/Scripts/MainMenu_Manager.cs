using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum MainMenu { play, load, options, exit}

public class MainMenu_Manager : MonoBehaviour
{
    MainMenu menuSelect;
    GameObject whiteScreen;
    TextMeshProUGUI playText;
    TextMeshProUGUI loadText;
    TextMeshProUGUI optionText;
    TextMeshProUGUI exitText;
    bool playPress = false;
    bool whiteFade = false;

    // Start is called before the first frame update
    void Start()
    {
        menuSelect = MainMenu.play;
        whiteScreen = GameObject.Find("White_Screen");
        whiteScreen.SetActive(false);
        playText = GameObject.Find("Play_Text").GetComponent<TextMeshProUGUI>();
        loadText = GameObject.Find("Load_Text_Main").GetComponent<TextMeshProUGUI>();
        optionText = GameObject.Find("Option_Text").GetComponent<TextMeshProUGUI>();
        exitText = GameObject.Find("Exit_Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!OptionMenu_Manager.isOpen && !Load_Manager.isOpen && !SetFirstResolution.WarningIsOpen)
        {
            if (!playPress)
                MainMenuController();

            ChangeMainMenuSelect();
            FMOD_Sound_Manager.GameMenu(0);
        }

        if (playPress)
            FMOD_Sound_Manager.GameMenu(2);
    }

    #region Change Menu Select
    void ChangeMainMenuSelect()
    {
        switch (menuSelect)
        {
            case MainMenu.play:
                PlaySelect();
                break;

            case MainMenu.load:
                LoadSelect();
                break;

            case MainMenu.options:
                OptionSelect();
                break;

            case MainMenu.exit:
                ExitSelect();
                break;
        }
    }
    #endregion

    #region Main Menu Controller
    void MainMenuController()
    {
        if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (menuSelect) 
            {
                case MainMenu.play:
                    menuSelect = MainMenu.load;
                    break;

                case MainMenu.load:
                    menuSelect = MainMenu.options;
                    break;

                case MainMenu.options:
                    menuSelect = MainMenu.exit;
                    break;

                case MainMenu.exit:
                    menuSelect = MainMenu.play;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (menuSelect)
            {
                case MainMenu.play:
                    menuSelect = MainMenu.exit;
                    break;

                case MainMenu.load:
                    menuSelect = MainMenu.play;
                    break;

                case MainMenu.options:
                    menuSelect = MainMenu.load;
                    break;

                case MainMenu.exit:
                    menuSelect = MainMenu.options;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    #region Play Select
    void PlaySelect()
    {
        playText.color = new Color32(255, 255, 150, 255);
        loadText.color = Color.white;
        optionText.color = Color.white;
        exitText.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.switchTimer >= Timer_KeyPress_Manager.timeChargeForMusic && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !playPress)
        {
            RestoreAllVariables();
            playPress = true;
            Destroy(playText);
            Destroy(loadText);
            Destroy(optionText);
            Destroy(exitText);

            whiteScreen.SetActive(true);
            Timer_KeyPress_Manager.timer = 0;
            Timer_KeyPress_Manager.switchTimer = 0;
        }

        if (playPress && !whiteFade && Timer_KeyPress_Manager.timer >= 0.05f)
        {
            whiteFade = true;
            whiteScreen.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        }

        if(playPress && whiteFade && Timer_KeyPress_Manager.timer >= 3f)
        {
            whiteScreen.GetComponent<Image>().color = Color.black;
            whiteScreen.GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        }

        if (playPress && Timer_KeyPress_Manager.timer >= 8f)
            SceneManager.LoadScene("MenuDifficoltà");
    }
    #endregion

    #region Load Select
    void LoadSelect()
    {
        playText.color = Color.white;
        loadText.color = new Color32(255, 255, 150, 255);
        optionText.color = Color.white;
        exitText.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.switchTimer >= Timer_KeyPress_Manager.timeChargeForMusic)
        {
            RestoreAllVariables();
            Load_Manager.loadMenu.SetActive(true);
            Load_Manager.isOpen = true;
            FMOD_Sound_Manager.PlayDecisionSound();
            Timer_KeyPress_Manager.timer = 0;
            Timer_KeyPress_Manager.switchTimer = 0;
        }
    }
    #endregion

    #region Option Select
    void OptionSelect()
    {
        playText.color = Color.white;
        loadText.color = Color.white;
        optionText.color = new Color32(255, 255, 150, 255);
        exitText.color = Color.white;

        if ((KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge))
        {
            if (OptionMenu_Manager.option_Menu != null)
            {
                Timer_KeyPress_Manager.timer = 0;
                OptionMenu_Manager.option_Menu.SetActive(true);
                OptionMenu_Manager.isOpen = true;
                FMOD_Sound_Manager.PlayDecisionSound();
                OptionMenu_Manager.RecreateOption();
            }
        }
    }
    #endregion

    #region Exit Select
    void ExitSelect()
    {
        playText.color = Color.white;
        loadText.color = Color.white;
        optionText.color = Color.white;
        exitText.color = new Color32(255, 255, 150, 255);

        if ((KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge))
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            Application.Quit();
        }
    }
    #endregion

    #region Restore all Variables
    void RestoreAllVariables()
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
        Shooter_Manager.ResetAllQuantity();
        ObjectInInventory_Manager.UnequipsObject();
        TakePython_Quest_Manager.RestoreAll();

        for (int i = 0; i < 7; i++)
        {
            ObjectInInventory_Manager.DeleteObject(i);
        }

        PlayerLife_Manager.life = PlayerLife_Manager.maxLife;
    }
    #endregion
}
