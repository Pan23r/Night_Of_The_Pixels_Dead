using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class Translation_Manager : MonoBehaviour
{
    public const string translationsfolder = "Night Of The Pixels Dead VG_Data/Translation/";
#if UNITY_ANDROID
    public static string pathSaveSlot;
#elif UNITY_EDITOR
    public static string pathSaveSlot = "Assets/Resources/Translation/";
#else
    public static string pathSaveSlot = $"{System.Environment.CurrentDirectory}/{translationsfolder}";
#endif

    public static string translationName = "English.xml";

    //MAIN MENU
    public static string play;
    public static string load;
    public static string option;
    public static string exit;

    //OPTION
    public static string fullScreen;
    public static string resolution;
    public static string volume;
    public static string language;
    public static string on;
    public static string off;

    //LOAD & SAVE GAME
    public static string save;
    public static string emptySlot;

    //OVERLOAD SAVE REQUEST
    public static string requestSaveHard;
    public static string requestOverloadSave;

    //SELECT DIFFICULTY 
    public static string difficultyTitle;
    public static string choiceHardMod;
    public static string choiceEasyMod;
    public static string hard;
    public static string easy;

    //GAME MENU
    public static string invenctory;
    public static string backToMainMenu;

    //BACK TO MAIN MENU REQUEST
    public static string requestMainMenu;
    public static string yes;
    public static string no;

    //INVENCTORY
    public static string objEquipped;
    public static string useSelect;
    public static string EquipsdSelect;
    public static string checkSelect;
    public static string combineSelect;
    public static string life;

    //OBJECT NAME
    public static string nameGun;
    public static string nameSallyKey;
    public static string nameLabKey;
    public static string nameAmmunitionGun;
    public static string nameFloppy;
    public static string nameFloppyWithMusicSheet;
    public static string nameMusicSheet;
    public static string nameMedallion;
    public static string nameMagnum;
    public static string namepieceOfCloth;

    //OBJECT EXAMINATION
    public static string gunExamination;
    public static string sallysKeyExamination;
    public static string labKeyExamination;
    public static string ammunitionExamination;
    public static string floppyExamination;
    public static string floppyWhithMusicSheetExamination;
    public static string musicSheetExamination;
    public static string medallionExamination;
    public static string magnumExamination;
    public static string pieceOfClothExamination;

    //OBJECT USE OR TAKE
    public static string useObject_TextBox;
    public static string takeObject_TextBox;

    //INTRO
    public static string introTitle1;
    public static string introTitle2;
    public static string introTitle3;

    //TEXTBOX
    public static string phone;
    public static string lobbyNightTable;
    public static string piano1;
    public static string piano2;
    public static string fireplace;
    public static string shield;
    public static string sword;
    public static string rearExit;
    public static string cellarDoor;
    public static string cookBook;
    public static string sallyDoor;
    public static string labDoor;
    public static string orwellBook;
    public static string safe;
    public static string sheets;
    public static string computerSally1;
    public static string computerSally2;
    public static string wallSally;
    public static string computerLab;
    public static string karenBody;
    public static string tomb;
    public static string sallyDeadBody;
    public static string deadBodyMan;
    public static string doorBunker;
    public static string noPOCtoUse;

    //TEXT NOTE
    public static string reportTitle;
    public static string reportPage1;
    public static string reportPage2;
    public static string reportPage3;
    public static string reportPage4;
    public static string reportPage5;
    public static string reportPage6;
    public static string reportPage7;

    public static string sallyNoteTitle;
    public static string sallyNote;
    
    public static string sallyHomeworkTitle;
    public static string sallyHomeworkPage1;
    public static string sallyHomeworkPage2;

    //DEAD SCREEN
    public static string dead;

    //SET DEFAULT RESOLUTION
    public static string setResolution;

    //SET ENDGAME TEXT
    public static string endGameStory;
    public static string credits;

    private void Awake()
    {
        /*if (SystemInfo.deviceType == DeviceType.Handheld)
            pathSaveSlot = Check_Folder_Existence.pathForMobile;*/

        if (SaveOptions_Manager.ReturnFileExist())
            translationName = SaveOptions_Manager.ReturnLanguage();

        OptionMenu_Manager.allTranslations = Directory.GetFiles(pathSaveSlot, "*.xml");
        OptionMenu_Manager.currentLanguageSel = OptionMenu_Manager.ReturnCurrentLanguage();
        translationName = OptionMenu_Manager.allTranslations[OptionMenu_Manager.currentLanguageSel].Substring(pathSaveSlot.Length);
        LoadText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadText()
    {
        if (File.Exists($"{pathSaveSlot}{translationName}"))
        {
            XmlTextReader XmlTR = new XmlTextReader($"{pathSaveSlot}{translationName}");
            while (XmlTR.Read())
            {
                //MAIN MENU
                if (XmlTR.MoveToAttribute("Play_Text"))
                    play = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Load_Text_Main"))
                    load = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Option_Text"))
                    option = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Exit_Text"))
                    exit = XmlTR.ReadContentAsString();

                //OPTION
                if (XmlTR.MoveToAttribute("On"))
                    on = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Off"))
                    off = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Full_Screen"))
                    fullScreen = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Screen_Size"))
                    resolution = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Volume"))
                    volume = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Language"))
                    language = XmlTR.ReadContentAsString();

                //LOAD & SAVE GAME
                if (XmlTR.MoveToAttribute("Save_text"))
                    save = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Empty_Slot"))
                    emptySlot = XmlTR.ReadContentAsString();

                //OVERLOAD SAVE REQUEST
                if (XmlTR.MoveToAttribute("RequestSaveHard"))
                    requestSaveHard = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("RequestOverloadSave"))
                    requestOverloadSave = XmlTR.ReadContentAsString();

                //SELECT DIFFICULTY 
                if (XmlTR.MoveToAttribute("difficulty_Title"))
                    difficultyTitle = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ChoiceHardMod"))
                    choiceHardMod = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ChoiceEasyMod"))
                    choiceEasyMod = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Hard"))
                    hard = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Easy"))
                    easy = XmlTR.ReadContentAsString();

                //GAME MENU
                if (XmlTR.MoveToAttribute("Invenctory"))
                    invenctory = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("BackToMainMenu"))
                    backToMainMenu = XmlTR.ReadContentAsString();

                //BACK TO MAIN MENU REQUEST
                if (XmlTR.MoveToAttribute("Request"))
                    requestMainMenu = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Yes"))
                    yes = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("No"))
                    no = XmlTR.ReadContentAsString();

                //INVENCTORY
                if (XmlTR.MoveToAttribute("ObjEquipped"))
                    objEquipped = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("UseSelect"))
                    useSelect = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("EquipsdSelect"))
                    EquipsdSelect = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("CheckSelect"))
                    checkSelect = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("CombineSelect"))
                    combineSelect = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Life"))
                    life = XmlTR.ReadContentAsString();

                //NAME OBJECTS

                if (XmlTR.MoveToAttribute("NameGun"))
                    nameGun = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameSallyKey"))
                    nameSallyKey = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameLabKey"))
                    nameLabKey = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameAmmunitionGun"))
                    nameAmmunitionGun = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameFloppy"))
                    nameFloppy = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameFloppyWithMusicSheet"))
                    nameFloppyWithMusicSheet = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NameMusicSheet"))
                    nameMusicSheet = XmlTR.ReadContentAsString(); 
                
                if (XmlTR.MoveToAttribute("NameMedallion"))
                    nameMedallion = XmlTR.ReadContentAsString();
                
                if (XmlTR.MoveToAttribute("NameMagnum"))
                    nameMagnum = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NamepieceOfCloth"))
                    namepieceOfCloth = XmlTR.ReadContentAsString();

                //OBJECT EXAMINATION
                if (XmlTR.MoveToAttribute("GunExamination"))
                    gunExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("SallysKeyExamination"))
                    sallysKeyExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("LabKeyExamination"))
                    labKeyExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("AmmunitionExamination"))
                    ammunitionExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("FloppyExamination"))
                    floppyExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("FloppyWhithMusicSheetExamination"))
                    floppyWhithMusicSheetExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("MusicSheetExamination"))
                    musicSheetExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("MedallionExamination"))
                    medallionExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("MagnumExamination"))
                    magnumExamination = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("PieceOfClothExamination"))
                    pieceOfClothExamination = XmlTR.ReadContentAsString();

                //OBJECT USE OR TAKE
                if (XmlTR.MoveToAttribute("UseObject_TextBox"))
                    useObject_TextBox = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("TakeObject_TextBox"))
                    takeObject_TextBox = XmlTR.ReadContentAsString();

                //INTRO
                if (XmlTR.MoveToAttribute("IntroTitle1"))
                    introTitle1 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("IntroTitle2"))
                    introTitle2 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("IntroTitle3"))
                    introTitle3 = XmlTR.ReadContentAsString();

                //TEXTBOX

                if (XmlTR.MoveToAttribute("Phone"))
                    phone = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("LobbyNightTable"))
                    lobbyNightTable = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Piano1"))
                    piano1 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Piano2"))
                    piano2 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Fireplace"))
                    fireplace = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Shield"))
                    shield = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Sword"))
                    sword = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("RearExit"))
                    rearExit = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("CellarDoor"))
                    cellarDoor = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("CookBook"))
                    cookBook = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("SallyDoor"))
                    sallyDoor = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("LabDoor"))
                    labDoor = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("OrwellBook"))
                    orwellBook = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Safe"))
                    safe = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Sheets"))
                    sheets = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ComputerSally1"))
                    computerSally1 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ComputerSally2"))
                    computerSally2 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("WallSally"))
                    wallSally = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ComputerLab"))
                    computerLab = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("KarenBody"))
                    karenBody = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Tomb"))
                    tomb = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("SallyDeadBody"))
                    sallyDeadBody = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("DeadBodyMan"))
                    deadBodyMan = XmlTR.ReadContentAsString();
                
                if (XmlTR.MoveToAttribute("DoorBunker")) 
                     doorBunker = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("NoPOCtoUse"))
                    noPOCtoUse = XmlTR.ReadContentAsString();

                //TEXT NOTE

                if (XmlTR.MoveToAttribute("ReportTitle"))
                    reportTitle = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage1"))
                    reportPage1 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage2"))
                    reportPage2 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage3"))
                    reportPage3 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage4"))
                    reportPage4 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage5"))
                    reportPage5 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage6"))
                    reportPage6 = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("ReportPage7"))
                    reportPage7 = XmlTR.ReadContentAsString();

                //--------------------------------------------

                if (XmlTR.MoveToAttribute("SallyNoteTitle"))
                    sallyNoteTitle = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("SallyNote"))
                    sallyNote = XmlTR.ReadContentAsString();
                //--------------------------------------------

                if (XmlTR.MoveToAttribute("sallyHomeworkTitle"))
                    sallyHomeworkTitle = XmlTR.ReadContentAsString();
                
                if (XmlTR.MoveToAttribute("sallyHomeworkPage1"))
                    sallyHomeworkPage1 = XmlTR.ReadContentAsString();
                
                if (XmlTR.MoveToAttribute("sallyHomeworkPage2"))
                    sallyHomeworkPage2 = XmlTR.ReadContentAsString();

                //DEAD SCREEN
                if (XmlTR.MoveToAttribute("Dead"))
                    dead = XmlTR.ReadContentAsString();

                //SET DEFAULT RESOLUTION
                if (XmlTR.MoveToAttribute("SetResolution"))
                    setResolution = XmlTR.ReadContentAsString();

                //SET ENDGAME TEXT
                if (XmlTR.MoveToAttribute("EndGameStory"))
                    endGameStory = XmlTR.ReadContentAsString();

                if (XmlTR.MoveToAttribute("Credits"))
                    credits = XmlTR.ReadContentAsString();
            }
        }        
    }
}
