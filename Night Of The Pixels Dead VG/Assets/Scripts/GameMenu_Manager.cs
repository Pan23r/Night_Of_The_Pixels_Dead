using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

enum GameMenu { invenctory, options, endGame }

public class GameMenu_Manager : MonoBehaviour
{
    public static GameObject menuInGame;

    Image backGround;
    TextMeshProUGUI invenctory;
    TextMeshProUGUI option;
    TextMeshProUGUI endGame;
    static GameMenu gameMenu;

    public static bool menuIsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        menuInGame = GameObject.Find("GameMenu");
        backGround = GameObject.Find("BackGroundMenu").GetComponent<Image>();
        invenctory = GameObject.Find("Inventario").GetComponent<TextMeshProUGUI>();
        option = GameObject.Find("Opzioni").GetComponent<TextMeshProUGUI>();
        endGame = GameObject.Find("EsciDalGioco").GetComponent<TextMeshProUGUI>();

        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0.5f);

        gameMenu = GameMenu.invenctory;

        if (menuInGame != null)
        {
            menuInGame.SetActive(false);
            menuIsActive = false;
        }
    }

    #region Update
    // Update is called once per frame
    void Update()
    {
        if(KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && menuIsActive && menuInGame != null && !Text_Creator.isActive && !OptionMenu_Manager.isOpen && !ReturnToMainMenu.isOpen && !Inventory_Manager.isOpen && !SetFirstResolution.WarningIsOpen||
            KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && menuIsActive && menuInGame != null && !Text_Creator.isActive && !OptionMenu_Manager.isOpen && !ReturnToMainMenu.isOpen && !Inventory_Manager.isOpen && !SetFirstResolution.WarningIsOpen)
        {
            ExitFromGameMenu(true);
        }

        if (menuIsActive && !SetFirstResolution.WarningIsOpen)
        {
            ControllerMenu();
            ChangeMenuSelect();
        }
    }
    #endregion

    #region Change Menu Select
    private void ChangeMenuSelect()
    {
        switch (gameMenu)
        {
            case GameMenu.invenctory:
                InvenctorySelect();
                break;

            case GameMenu.options:
                OptionSelect();
                break;

            case GameMenu.endGame:
                EndGameSelect();
                break;
        }
    }
    #endregion

    #region Controller Menu
    private void ControllerMenu()
    {
        if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (gameMenu)
            {
                case GameMenu.invenctory:
                    gameMenu = GameMenu.options;
                    break;

                case GameMenu.options:
                    gameMenu = GameMenu.endGame;
                    break;

                case GameMenu.endGame:
                    gameMenu = GameMenu.invenctory;
                    break;
            }

            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (gameMenu)
            {
                case GameMenu.invenctory:
                    gameMenu = GameMenu.endGame;
                    break;

                case GameMenu.options:
                    gameMenu = GameMenu.invenctory;
                    break;

                case GameMenu.endGame:
                    gameMenu = GameMenu.options;
                    break;
            }

            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    #region Invenctory Select
    private void InvenctorySelect()
    {
        invenctory.color = Color.yellow;
        option.color = Color.white;
        endGame.color = Color.white;

        if ((KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge))
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            Timer_KeyPress_Manager.timer = 0;
            Inventory_Manager.InventoryObj.SetActive(true);
            Inventory_Manager.isOpen = true;
            Timer_KeyPress_Manager.timer = 0;
            Inventory_Manager.objSelectMenu.SetActive(false);
            menuInGame.SetActive(false);
        }
    }
    #endregion

    #region Option Select
    private void OptionSelect()
    {
        invenctory.color = Color.white;
        option.color = Color.yellow;
        endGame.color = Color.white;

        if ((KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge))
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            if (OptionMenu_Manager.option_Menu != null)
            {
                Timer_KeyPress_Manager.timer = 0;
                OptionMenu_Manager.option_Menu.SetActive(true);
                OptionMenu_Manager.isOpen = true;
                menuInGame.SetActive(false);
                OptionMenu_Manager.RecreateOption();
            }
        }
    }
    #endregion

    #region EndGame Select
    private void EndGameSelect()
    {
        invenctory.color = Color.white;
        option.color = Color.white;
        endGame.color = Color.yellow;

        if ((KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge))
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            ReturnToMainMenu.Obj_returnToMenu.SetActive(true);
            ReturnToMainMenu.isOpen = true;
            menuInGame.SetActive(false);
            ObjectInInventory_Manager.UnequipsObject();
            Shooter_Manager.ResetAllQuantity();
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    public static void ExitFromGameMenu(bool playSound)
    {
        if(playSound)
            FMOD_Sound_Manager.PlayBackSound();

        menuInGame.SetActive(false);
        menuIsActive = false;
        Timer_KeyPress_Manager.timer = 0;
        PlayerMovement.canMove = true;
        Enemy_Manager.canMove = true;
        Enemy_Manager_NavMesh.canMove = true;
        gameMenu = GameMenu.invenctory;
        Text_Creator.canActive = true;
        Change_room_Manager.canChange = true;
        Timer_KeyPress_Manager.timer = 0;
        UI_Game_Manager.uiCanOpen = true;
    }
}
