using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

enum OptionMenu{ FullScreen, ScreenSize, Volume, Language}
public class OptionMenu_Manager : MonoBehaviour
{
    public static GameObject option_Menu;
    public static bool isOpen = false;

    OptionMenu optionSelect;
    static TextMeshProUGUI fullScreen;
    static TextMeshProUGUI screenSize;
    static TextMeshProUGUI volume;
    static TextMeshProUGUI language;
    Image backGround;

    static Vector2[] windowSize = new Vector2[3];
    public static int currentWindowSize = 0;
    float alphaBackGround = 0.5f;

    static string saveFullScreen;
    public static string[] allTranslations = new string[1];
    public static int currentLanguageSel = 0;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        if (!Screen.fullScreen)
            saveFullScreen = Translation_Manager.off;
        else
            saveFullScreen = Translation_Manager.on;

        AllSize();
        optionSelect = OptionMenu.FullScreen;
        option_Menu = GameObject.Find("Option_Menu");
        backGround = GameObject.Find("BackGroundOptionMenu").GetComponent<Image>();
        fullScreen = GameObject.Find("Full_Screen").GetComponent<TextMeshProUGUI>();
        screenSize = GameObject.Find("Screen_Size").GetComponent<TextMeshProUGUI>();
        language = GameObject.Find("Language").GetComponent<TextMeshProUGUI>();
        volume = GameObject.Find("Volume").GetComponent<TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().name == "Menu")
            alphaBackGround = 1f;
        else
            alphaBackGround = 0.5f;

        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, alphaBackGround);
        
        RecreateOption();

        if (option_Menu != null)
            option_Menu.SetActive(false);
    }
    #endregion

    #region Return Current Language
    public static int ReturnCurrentLanguage()
    {
        for (int i = 0; i < allTranslations.Length; i++)
        {
            if (allTranslations[i].Substring(Translation_Manager.pathSaveSlot.Length) == Translation_Manager.translationName)
            {
                return i;
            }
        }
        return 0;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        ControllerMenuOption();
        ChangeOptionSelect();
    }

    #region Change Option Select
    private void ChangeOptionSelect()
    {
        switch (optionSelect)
        {
            case OptionMenu.FullScreen:
                FullScreenSelect();
                break;

            case OptionMenu.ScreenSize:
                ScreenSizeSelect();
                break;

            case OptionMenu.Volume:
                VolumeSelect();
                break;
                
            case OptionMenu.Language:
                LanguageSelect();
                break;
        }
    }
    #endregion

    #region Controller Menu Option
    private void ControllerMenuOption()
    {
        if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (optionSelect)
            {
                case OptionMenu.FullScreen:
                    optionSelect = OptionMenu.ScreenSize;
                    break;

                case OptionMenu.ScreenSize:
                    optionSelect = OptionMenu.Volume;
                    break;

                case OptionMenu.Volume:
                    optionSelect = OptionMenu.Language;
                    break;

                case OptionMenu.Language:
                    optionSelect = OptionMenu.FullScreen;
                    break;
            }

            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (optionSelect)
            {
                case OptionMenu.FullScreen:
                    optionSelect = OptionMenu.Language;
                    break;

                case OptionMenu.ScreenSize:
                    optionSelect = OptionMenu.FullScreen;
                    break;

                case OptionMenu.Volume:
                    optionSelect = OptionMenu.ScreenSize;
                    break;

                case OptionMenu.Language:
                    optionSelect = OptionMenu.Volume;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge || KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayBackSound();
            option_Menu.SetActive(false);
            isOpen = false;

            if(GameMenu_Manager.menuInGame != null)
                GameMenu_Manager.menuInGame.SetActive(true);

            SaveOptions_Manager.OverrideSaveOptions();

            currentLanguageSel = ReturnCurrentLanguage();

            Timer_KeyPress_Manager.timer = 0;
            optionSelect = OptionMenu.FullScreen;
        }
    }
    #endregion

    #region FullScreen Select 
    private void FullScreenSelect()
    {
        fullScreen.color = Color.yellow;
        screenSize.color = Color.white;
        language.color = Color.white;
        volume.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && SystemInfo.deviceType != DeviceType.Handheld)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            if (!Screen.fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                saveFullScreen = Translation_Manager.on;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                saveFullScreen = Translation_Manager.off;
            }
            fullScreen.text = $"{Translation_Manager.fullScreen}: {saveFullScreen}";
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    #region Screen Size Select
    private void ScreenSizeSelect()
    {
        fullScreen.color = Color.white;
        screenSize.color = Color.yellow;
        volume.color = Color.white;
        language.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            switch (currentWindowSize)
            {
                case 0:
                    currentWindowSize = 1;
                    break;

                case 1:
                    currentWindowSize = 2;
                    break;

                case 2:
                    currentWindowSize = 0;
                    break;
            }

            screenSize.text = $"{Translation_Manager.resolution} {windowSize[currentWindowSize].x}X{windowSize[currentWindowSize].y}";

            Screen.SetResolution((int)windowSize[currentWindowSize].x, (int)windowSize[currentWindowSize].y, Screen.fullScreenMode);
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    private void AllSize()
    {
        windowSize[0] = new Vector2(1920, 1080);
        windowSize[1] = new Vector2(1280, 720);
        windowSize[2] = new Vector2(640, 360);
    }

    #region Volume Select
    private void VolumeSelect()
    {
        fullScreen.color = Color.white;
        screenSize.color = Color.white;
        language.color = Color.white;
        volume.color = Color.yellow;
        float ValVolumeDown;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
            ValVolumeDown = 0.1f;
        else
            ValVolumeDown = 0.01f;
        
        float myJustVolume;
        FMOD_Sound_Manager.vca.getVolume(out myJustVolume);

        if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && myJustVolume > 0)
        {
            myJustVolume -= ValVolumeDown;
            FMOD_Sound_Manager.vca.setVolume(myJustVolume);

            /*AudioListener.volume -= ValVolumeDown;

            if (AudioListener.volume < 0)
                AudioListener.volume = 0;*/
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && myJustVolume < 1)
        {
            myJustVolume += ValVolumeDown;
            FMOD_Sound_Manager.vca.setVolume(myJustVolume);

            //AudioListener.volume += ValVolumeDown;

            /*if (AudioListener.volume > 1)
                AudioListener.volume = 1;*/
            Timer_KeyPress_Manager.timer = 0;
        }
               
        if (myJustVolume < 0)
            FMOD_Sound_Manager.vca.setVolume(0);

        if (myJustVolume > 1)
            FMOD_Sound_Manager.vca.setVolume(1);

        volume.text = $"{Translation_Manager.volume}: {(int)(myJustVolume * 100)}";
    }
    #endregion

    #region Select Language
    private void LanguageSelect()
    {
        fullScreen.color = Color.white;
        screenSize.color = Color.white;
        volume.color = Color.white;
        language.color = Color.yellow;

        if (KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            currentLanguageSel++;
            currentLanguageSel = (currentLanguageSel >= allTranslations.Length) ? 0 : currentLanguageSel;
            
            if (allTranslations.Length == 1)
                FMOD_Sound_Manager.PlayNullSound();
            else
                FMOD_Sound_Manager.PlayCursorSound();

            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            currentLanguageSel--;
            currentLanguageSel = (currentLanguageSel < 0) ? allTranslations.Length - 1 : currentLanguageSel;

            if (allTranslations.Length == 1)
                FMOD_Sound_Manager.PlayNullSound();
            else
                FMOD_Sound_Manager.PlayCursorSound();

            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            Translation_Manager.translationName = allTranslations[currentLanguageSel].Substring(Translation_Manager.pathSaveSlot.Length);
            Translation_Manager.LoadText();
            TranslateTextNotInScript.SetTranslation();
            ObjectIstantiator_Manager.ReturnNameOfObj();
            Load_Manager.DrawAllSavesSlot();
            Save_Manager.DrawAllSavesSlot();

            if(PlayerMovement.objectCollide != null)
            {
                PlayerMovement.objectCollide.SetActive(false);
                PlayerMovement.objectCollide.SetActive(true);
            }

            FMOD_Sound_Manager.PlayDecisionSound();
            Timer_KeyPress_Manager.timer = 0;
        }
        RecreateOption();
    }
    #endregion

    #region Recreate Option
    public static void RecreateOption()
    {
        screenSize.text = $"{Translation_Manager.resolution} {Screen.width}X{Screen.height}";

        if (Screen.width == windowSize[0].x)
            currentWindowSize = 0;
        else if (Screen.width == windowSize[1].x)
            currentWindowSize = 1;
        else if (Screen.width == windowSize[2].x)
            currentWindowSize = 2;

        if (!Screen.fullScreen)
            saveFullScreen = Translation_Manager.off;
        else
            saveFullScreen = Translation_Manager.on;

        fullScreen.text = $"{Translation_Manager.fullScreen}: {saveFullScreen}";

        float myJustAudio;
        FMOD_Sound_Manager.vca.getVolume(out myJustAudio);
        volume.text = $"{Translation_Manager.volume}: {(int)(myJustAudio * 100)}";

        if(allTranslations[currentLanguageSel] != null)
            language.text = $"{Translation_Manager.language}: {allTranslations[currentLanguageSel].Substring(Translation_Manager.pathSaveSlot.Length, allTranslations[currentLanguageSel].Length - Translation_Manager.pathSaveSlot.Length  - 4)}";
    }
    #endregion
}
