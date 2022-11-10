using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class TranslateTextNotInScript : MonoBehaviour
{
    //MAIN MENU
    static TextMeshProUGUI play;
    static TextMeshProUGUI load;
    static TextMeshProUGUI option;
    static TextMeshProUGUI exit;

    //OPTION
    static TextMeshProUGUI optionTitle;
    static TextMeshProUGUI fullScreen;
    static TextMeshProUGUI resolution;
    static TextMeshProUGUI volume;
    static TextMeshProUGUI language;

    //LOAD & SAVE GAME
    static TextMeshProUGUI saveTitle;
    static TextMeshProUGUI loadTitle;
    static TextMeshProUGUI loadEmptySlot1;
    static TextMeshProUGUI loadEmptySlot2;
    static TextMeshProUGUI loadEmptySlot3;
    static TextMeshProUGUI saveEmptySlot1;
    static TextMeshProUGUI saveEmptySlot2;
    static TextMeshProUGUI saveEmptySlot3;

    //OVERLOAD SAVE REQUEST
    static TextMeshProUGUI requestSaveHard;
    static TextMeshProUGUI yes_SaveHard;
    static TextMeshProUGUI no_SaveHard;
    static TextMeshProUGUI requestOverloadSave;
    static TextMeshProUGUI yes_OverLoadSave;
    static TextMeshProUGUI no_OverLoadSave;

    //SELECT DIFFICULTY 
    static TextMeshProUGUI difficultyTitle;
    static TextMeshProUGUI choiceHardMod;
    static TextMeshProUGUI choiceEasyMod;

    //GAME MENU
    static TextMeshProUGUI invenctory;
    static TextMeshProUGUI backToMainMenu;
    static TextMeshProUGUI gameMenuOptions;

    //INVENCTORY
    static TextMeshProUGUI requestMainMenu;
    static TextMeshProUGUI yes;
    static TextMeshProUGUI no;

    //INVENCTORY
    static TextMeshProUGUI objEquipped;
    static TextMeshProUGUI checkSelect;
    static TextMeshProUGUI combineSelect;
    static TextMeshProUGUI life;

    //DEAD SCREEN
    static TextMeshProUGUI dead;

    //SET DEFAULT RESOLUTION 
    static TextMeshProUGUI setResolution;

    //SET END GAME TEXT
    static TextMeshProUGUI endGameStory;
    static TextMeshProUGUI credits;

    // Start is called before the first frame update
    void Start()
    {
        //MAIN MENU
        play = (gameObject.name == "Play_Text") ? gameObject.GetComponent<TextMeshProUGUI>() : play;
        load = (gameObject.name == "Load_Text_Main") ? gameObject.GetComponent<TextMeshProUGUI>() : load;
        option = (gameObject.name == "Option_Text") ? gameObject.GetComponent<TextMeshProUGUI>(): option;
        exit = (gameObject.name == "Exit_Text") ? gameObject.GetComponent<TextMeshProUGUI>() : exit;

        //OPTION
        optionTitle = (gameObject.name == "Options_Tiltle") ? gameObject.GetComponent<TextMeshProUGUI>() : optionTitle;
        //fullScreen = GameObject.Find("Full_Screen") != null ? GameObject.Find("Full_Screen").GetComponent<TextMeshProUGUI>() : null;
        //resolution = GameObject.Find("Screen_Size") != null ? GameObject.Find("Screen_Size").GetComponent<TextMeshProUGUI>() : null;
        //volume = GameObject.Find("Volume") != null ? GameObject.Find("Volume").GetComponent<TextMeshProUGUI>() : null;
        //language = GameObject.Find("Language") != null ? GameObject.Find("Language").GetComponent<TextMeshProUGUI>() : null;

        //LOAD & SAVE GAME
        loadTitle = (gameObject.name == "Load_Text") ? gameObject.GetComponent<TextMeshProUGUI>() : loadTitle;
        saveTitle = (gameObject.name == "Save_text") ? gameObject.GetComponent<TextMeshProUGUI>() : saveTitle;
        loadEmptySlot1 = (gameObject.name == "LoadState_1") ? gameObject.GetComponent<TextMeshProUGUI>() : loadEmptySlot1;
        loadEmptySlot2 = (gameObject.name == "LoadState_2") ? gameObject.GetComponent<TextMeshProUGUI>() : loadEmptySlot2;
        loadEmptySlot3 = (gameObject.name == "LoadState_3") ? gameObject.GetComponent<TextMeshProUGUI>() : loadEmptySlot3;

        saveEmptySlot1 = (gameObject.name == "SaveState_1") ? gameObject.GetComponent<TextMeshProUGUI>() : saveEmptySlot1;
        saveEmptySlot2 = (gameObject.name == "SaveState_2")? gameObject.GetComponent<TextMeshProUGUI>() : saveEmptySlot2;
        saveEmptySlot3 = (gameObject.name == "SaveState_3") ? gameObject.GetComponent<TextMeshProUGUI>() : saveEmptySlot3;

        requestSaveHard = (gameObject.name == "questionUsePoc") ? gameObject.GetComponent<TextMeshProUGUI>() : requestSaveHard;
        yes_SaveHard = (gameObject.name == "Yes_POC") ? gameObject.GetComponent<TextMeshProUGUI>() : yes_SaveHard;
        no_SaveHard = (gameObject.name == "No_POC") ? gameObject.GetComponent<TextMeshProUGUI>() : no_SaveHard;

        requestOverloadSave = (gameObject.name == "OverloadSaveQuestion") ? gameObject.GetComponent<TextMeshProUGUI>() : requestOverloadSave;
        yes_OverLoadSave = (gameObject.name == "Yes_OverSave") ? gameObject.GetComponent<TextMeshProUGUI>() : yes_OverLoadSave;
        no_OverLoadSave = (gameObject.name == "No_OverSave") ? gameObject.GetComponent<TextMeshProUGUI>() : no_OverLoadSave;

        //SELECT DIFFICULTY 
        difficultyTitle = (gameObject.name == "Difficulty_NameMenu") ? gameObject.GetComponent<TextMeshProUGUI>() : difficultyTitle;
        choiceHardMod = (gameObject.name == "ChoiceHardMod") ? gameObject.GetComponent<TextMeshProUGUI>() : choiceHardMod;
        choiceEasyMod = (gameObject.name == "ChoiceEasyMod") ? gameObject.GetComponent<TextMeshProUGUI>() : choiceEasyMod;

        //GAME MENU
        invenctory = (gameObject.name == "Inventario") ? gameObject.GetComponent<TextMeshProUGUI>() : invenctory;
        backToMainMenu = (gameObject.name == "EsciDalGioco") ? gameObject.GetComponent<TextMeshProUGUI>() : backToMainMenu;
        gameMenuOptions = (gameObject.name == "Opzioni") ? gameObject.GetComponent<TextMeshProUGUI>() : gameMenuOptions;

        //BACK TO MAIN MENU REQUEST
        requestMainMenu = (gameObject.name == "Question") ? gameObject.GetComponent<TextMeshProUGUI>() : requestMainMenu;
        yes = (gameObject.name == "Yes") ? gameObject.GetComponent<TextMeshProUGUI>() : yes;
        no = (gameObject.name == "No") ? gameObject.GetComponent<TextMeshProUGUI>() : no;

        //INVENCTORY
        objEquipped = (gameObject.name == "Equipaggiato") ? gameObject.GetComponent<TextMeshProUGUI>() : objEquipped;
        checkSelect = (gameObject.name == "Check") ? gameObject.GetComponent<TextMeshProUGUI>() : checkSelect;
        combineSelect = (gameObject.name == "Combines") ? gameObject.GetComponent<TextMeshProUGUI>() : combineSelect;
        life = (gameObject.name == "Text_life") ? gameObject.GetComponent<TextMeshProUGUI>() : life;

        //DEAD SCREEN
        dead = (gameObject.name == "GameOver_Text") ? gameObject.GetComponent<TextMeshProUGUI>() : dead;

        //SET DEFAULT RESOLUTION
        setResolution = (gameObject.name == "TextResNotSup") ? gameObject.GetComponent<TextMeshProUGUI>() : setResolution;

        //SET END GAME TEXT
        endGameStory = (gameObject.name == "EndGameStory") ? gameObject.GetComponent<TextMeshProUGUI>() : endGameStory;
        credits = (gameObject.name == "Credits") ? gameObject.GetComponent<TextMeshProUGUI>() : credits;

        SetTranslation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetTranslation()
    {
        //MAIN MENU
        if (play != null)
            play.text = Translation_Manager.play;

        if (load != null)
            load.text = Translation_Manager.load;

        if (option != null)
            option.text = Translation_Manager.option;

        if (exit != null)
            exit.text = Translation_Manager.exit;

        //OPTION
        if (optionTitle != null)
            optionTitle.text = Translation_Manager.option;

        if (fullScreen != null)
            fullScreen.text = Translation_Manager.fullScreen;

        if (resolution != null)
            resolution.text = Translation_Manager.resolution;

        if (volume != null)
            volume.text = Translation_Manager.volume;

        if (language != null)
            language.text = Translation_Manager.language;

        //LOAD & SAVE GAME
        if (loadTitle != null)
            loadTitle.text = Translation_Manager.load;

        if (loadEmptySlot1 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot1File}"))
            loadEmptySlot1.text = Translation_Manager.emptySlot;

        if (loadEmptySlot2 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot2File}"))
            loadEmptySlot2.text = Translation_Manager.emptySlot;

        if (loadEmptySlot3 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot3File}"))
            loadEmptySlot3.text = Translation_Manager.emptySlot;
        
        if (saveTitle != null)
            saveTitle.text = Translation_Manager.save;

        if (saveEmptySlot1 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot1File}"))
            saveEmptySlot1.text = Translation_Manager.emptySlot;

        if (saveEmptySlot2 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot2File}"))
            saveEmptySlot2.text = Translation_Manager.emptySlot;
        
        if (saveEmptySlot3 != null && !File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot3File}"))
            saveEmptySlot3.text = Translation_Manager.emptySlot;

        //OVERLOAD SAVE REQUEST

        if (requestSaveHard != null)
            requestSaveHard.text = Translation_Manager.requestSaveHard;
        
        if (yes_SaveHard != null)
            yes_SaveHard.text = Translation_Manager.yes;
        
        if (no_SaveHard != null)
            no_SaveHard.text = Translation_Manager.no;

        if (requestOverloadSave != null)
            requestOverloadSave.text = Translation_Manager.requestOverloadSave;

        if (yes_OverLoadSave != null)
            yes_OverLoadSave.text = Translation_Manager.yes;

        if (no_OverLoadSave != null)
            no_OverLoadSave.text = Translation_Manager.no;

        //SELECT DIFFICULTY 
        if (difficultyTitle != null)
            difficultyTitle.text = Translation_Manager.difficultyTitle;

        if (choiceHardMod != null)
            choiceHardMod.text = Translation_Manager.choiceHardMod;

        if (choiceEasyMod != null)
            choiceEasyMod.text = Translation_Manager.choiceEasyMod;

        //GAME MENU
        if (invenctory != null)
            invenctory.text = Translation_Manager.invenctory;

        if (backToMainMenu != null)
            backToMainMenu.text = Translation_Manager.backToMainMenu;

        if (gameMenuOptions != null)
            gameMenuOptions.text = Translation_Manager.option;

        //BACK TO MAIN MENU REQUEST
        if (requestMainMenu != null)
            requestMainMenu.text = Translation_Manager.requestMainMenu;

        if (yes != null)
            yes.text = Translation_Manager.yes;

        if (no != null)
            no.text = Translation_Manager.no;

        //INVENCTORY
        if (objEquipped != null)
            objEquipped.text = Translation_Manager.objEquipped;
        
        if (checkSelect != null)
            checkSelect.text = Translation_Manager.checkSelect;

        if (combineSelect != null)
            combineSelect.text = Translation_Manager.combineSelect;

        if (life != null)
            life.text = Translation_Manager.life;

        //DEAD SCREEN
        if (dead != null)
            dead.text = Translation_Manager.dead;
        
        //SET DEFAULT RESOLUTION
        if (setResolution != null)
            setResolution.text = Translation_Manager.setResolution;

        //SET ENDGAME STORY
        if (endGameStory != null)
            endGameStory.text = Translation_Manager.endGameStory;

        if (credits != null)
            credits.text = Translation_Manager.credits;
    }
}
