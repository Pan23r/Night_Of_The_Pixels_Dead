using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


enum LoadMenu { slot1, slot2, slot3}

public class Load_Manager : MonoBehaviour
{
    public static GameObject loadMenu;
    public static bool isOpen = false;
    public static bool loadGame = false; //booleano che si attiva quando si vuole caricare il gioco e permette al player e all'inventario di ricrearsi 
    public static string fileToLoad = "";

    float startPositionX = -960f;
    float startPositionY = 540f;
    float velocityImage = 100f;

    LoadMenu slotSelection;
    Image backGround1;
    Image backGround2;
    static TextMeshProUGUI slot1_Text;
    static TextMeshProUGUI slot2_Text;
    static TextMeshProUGUI slot3_Text;
    static float alphaSlotNull = 0.5f;

    //NOMI DEI SAVESLOT DA CERCARE (SI MODIFICANO ANCHE NEL Save_Manager)
    public const string savesFolder = "Night Of The Pixels Dead VG_Data/Saves/";

#if UNITY_ANDROID
    public static string pathSaveSlot;
#elif UNITY_EDITOR
    public static string pathSaveSlot = "Assets/Resources/Saves/";
#else
    public static string pathSaveSlot = $"{System.Environment.CurrentDirectory}/{savesFolder}";
#endif

    public static string slot1File = "SaveSlot1.xml";
    public static string slot2File = "SaveSlot2.xml";
    public static string slot3File = "SaveSlot3.xml";

    public static bool playMusicOnLoad = false;

    /*private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            pathSaveSlot = $"{Application.persistentDataPath}/{_savesFolder}";
    }*/

    // Start is called before the first frame update
    void Start()
    {
        loadMenu = GameObject.Find("Menu_Load");
        slot1_Text = GameObject.Find("LoadState_1").GetComponent<TextMeshProUGUI>();
        slot2_Text = GameObject.Find("LoadState_2").GetComponent<TextMeshProUGUI>();
        slot3_Text = GameObject.Find("LoadState_3").GetComponent<TextMeshProUGUI>();
        backGround1 = GameObject.Find("BackGround_Load_1").GetComponent<Image>();
        backGround2 = GameObject.Find("BackGround_Load_2").GetComponent<Image>();

        backGround1.rectTransform.localPosition = new Vector2(startPositionX, startPositionY);
        backGround2.rectTransform.localPosition = new Vector2(backGround1.rectTransform.localPosition.x + backGround1.rectTransform.rect.width, startPositionY);

        DrawAllSavesSlot();

        CheckSlotNotNull();        
        loadMenu.SetActive(false);
    }

    public static void DrawAllSavesSlot()
    {
        //SLOT 1
        if (!File.Exists($"{pathSaveSlot}{slot1File}"))
            slot1_Text.alpha = alphaSlotNull;
        else
            XmlFileReader(slot1File, slot1_Text);

        //SLOT 2
        if (!File.Exists($"{pathSaveSlot}{slot2File}"))
            slot2_Text.alpha = alphaSlotNull;
        else
            XmlFileReader(slot2File, slot2_Text);

        //SLOT 3
        if (!File.Exists($"{pathSaveSlot}{slot3File}"))
            slot3_Text.alpha = alphaSlotNull;
        else
            XmlFileReader(slot3File, slot3_Text);
    }

#region Xml File Reader
    static void XmlFileReader(string slot, TextMeshProUGUI textSlot)
    {
        string namePlayerText = ""; 
        string dateText = "";
        string timeText = "";
        string gameMode = "";

        XmlTextReader XmlTR = new XmlTextReader($"{pathSaveSlot}{slot}");

        while (XmlTR.Read())
        {
            if (XmlTR.MoveToAttribute("namePlayer"))
            { 
                namePlayerText = XmlTR.ReadContentAsString();
            }
            else if(XmlTR.MoveToAttribute("date"))
            {
                dateText = XmlTR.ReadContentAsString();
            }
            else if (XmlTR.MoveToAttribute("time"))
            {
                timeText = XmlTR.ReadContentAsString();
            }
            else if (XmlTR.MoveToAttribute("hardMode"))
            {
                gameMode = (XmlTR.ReadContentAsBoolean()) ? Translation_Manager.easy : Translation_Manager.hard;
            }
        }

        textSlot.text = $"{namePlayerText} {dateText} {timeText} - {gameMode}";
    }
#endregion

#region Check Slot Not Null
    void CheckSlotNotNull()
    {
        if (File.Exists($"{pathSaveSlot}{slot1File}"))
            slotSelection = LoadMenu.slot1;
        else if (File.Exists($"{pathSaveSlot}{slot2File}"))
            slotSelection = LoadMenu.slot2;
        else if (File.Exists($"{pathSaveSlot}{slot3File}"))
            slotSelection = LoadMenu.slot3;
        else
            slotSelection = LoadMenu.slot1;
    }
#endregion

    // Update is called once per frame
    void Update()
    {
        FMOD_Sound_Manager.GameMenu(1);

        LoadMenuController();
        BackGroundController();
        SelectSlotController();
    }

#region Load Menu Controller
    void LoadMenuController()
    {
        if(KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.switchTimer >= Timer_KeyPress_Manager.timeChargeForMusic /*&& !MainMenu_Manager.soundtrackMenu.isPlaying*/ || KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.switchTimer >= Timer_KeyPress_Manager.timeChargeForMusic /*&& !MainMenu_Manager.soundtrackMenu.isPlaying*/)
        {
            FMOD_Sound_Manager.PlayBackSound();
            loadMenu.SetActive(false);
            BaseForExit();
            Timer_KeyPress_Manager.timer = 0;
            Timer_KeyPress_Manager.switchTimer = 0;
        }
    }
#endregion

#region BackGround Controller
    void BackGroundController()
    {
        float currentDeltaTime = Time.deltaTime;

        float positionX_1 = backGround1.rectTransform.anchoredPosition.x - (velocityImage * currentDeltaTime);
        backGround1.rectTransform.anchoredPosition = new Vector2(positionX_1, backGround1.rectTransform.anchoredPosition.y);

        float positionX_2 = backGround2.rectTransform.anchoredPosition.x - (velocityImage * currentDeltaTime);
        backGround2.rectTransform.anchoredPosition = new Vector2(positionX_2, backGround2.rectTransform.anchoredPosition.y);

        if (backGround1.rectTransform.anchoredPosition.x < startPositionX - backGround1.rectTransform.rect.width)
        {
            backGround1.rectTransform.anchoredPosition = new Vector2(backGround2.rectTransform.anchoredPosition.x + backGround1.rectTransform.rect.width, backGround1.rectTransform.anchoredPosition.y);
        }
        else if (backGround2.rectTransform.anchoredPosition.x < startPositionX - backGround2.rectTransform.rect.width)
        {
            backGround2.rectTransform.anchoredPosition = new Vector2(backGround1.rectTransform.anchoredPosition.x + backGround2.rectTransform.rect.width, backGround2.rectTransform.anchoredPosition.y);
        }
    }
#endregion

#region Select Slot Controller
    void SelectSlotController()
    {
        if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            switch (slotSelection)
            {
                case LoadMenu.slot1:

                    if (File.Exists($"{pathSaveSlot}{slot2File}"))
                    {
                        slotSelection = LoadMenu.slot2;
                        FMOD_Sound_Manager.PlayCursorSound();
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot3File}"))
                    {
                        slotSelection = LoadMenu.slot3;
                        FMOD_Sound_Manager.PlayCursorSound();
                    }

                    break;

                case LoadMenu.slot2:

                    if (File.Exists($"{pathSaveSlot}{slot3File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot3;
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot1File}"))
                    {
                        slotSelection = LoadMenu.slot1;
                        FMOD_Sound_Manager.PlayCursorSound();
                    }

                    break;

                case LoadMenu.slot3:

                    if (File.Exists($"{pathSaveSlot}{slot1File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot1;
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot2File}"))
                    {
                        slotSelection = LoadMenu.slot2;
                        FMOD_Sound_Manager.PlayCursorSound();
                    }

                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            switch (slotSelection)
            {
                case LoadMenu.slot1:

                    if (File.Exists($"{pathSaveSlot}{slot3File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot3;
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot2File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot2;
                    }

                    break;

                case LoadMenu.slot2:

                    if (File.Exists($"{pathSaveSlot}{slot1File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot1;
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot3File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot3;
                    }

                    break;

                case LoadMenu.slot3:

                    if (File.Exists($"{pathSaveSlot}{slot2File}"))
                    {
                        FMOD_Sound_Manager.PlayCursorSound();
                        slotSelection = LoadMenu.slot2;
                    }
                    else if (File.Exists($"{pathSaveSlot}{slot1File}"))
                    {
                        slotSelection = LoadMenu.slot1;
                        FMOD_Sound_Manager.PlayCursorSound();
                    }

                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }

        switch (slotSelection)
        {
            case LoadMenu.slot1:
                Slot1Select();
                break;

            case LoadMenu.slot2:
                Slot2Select();
                break;

            case LoadMenu.slot3:
                Slot3Select();
                break;
        }
    }
#endregion

#region Slot 1 Slect
    void Slot1Select()
    {
        if (File.Exists($"{pathSaveSlot}{slot1File}"))
        {
            slot1_Text.color = Color.yellow;
            slot2_Text.color = Color.white;
            slot3_Text.color = Color.white;

            if (!File.Exists($"{pathSaveSlot}{slot2File}"))
                slot2_Text.alpha = alphaSlotNull; 
            
            if (!File.Exists($"{pathSaveSlot}{slot3File}"))
                slot3_Text.alpha = alphaSlotNull;

            if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
            {
                LoadState(slot1File);
                BaseForExit();
                Timer_KeyPress_Manager.timer = 0;
            }
        }
    }
#endregion

#region Slot 2 Slect
    void Slot2Select()
    {
        if (File.Exists($"{pathSaveSlot}{slot2File}"))
        {
            slot2_Text.color = Color.yellow;
            slot1_Text.color = Color.white;
            slot3_Text.color = Color.white;

            if (!File.Exists($"{pathSaveSlot}{slot1File}"))
                slot1_Text.alpha = alphaSlotNull;

            if (!File.Exists($"{pathSaveSlot}{slot3File}"))
                slot3_Text.alpha = alphaSlotNull;

            if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
            {
                LoadState(slot2File);
                BaseForExit();
                Timer_KeyPress_Manager.timer = 0;
            }
        }
    }
#endregion

#region Slot 3 Slect
    void Slot3Select()
    {
        if (File.Exists($"{pathSaveSlot}{slot3File}"))
        {
            slot3_Text.color = Color.yellow;
            slot2_Text.color = Color.white;
            slot1_Text.color = Color.white;

            if (!File.Exists($"{pathSaveSlot}{slot2File}"))
                slot2_Text.alpha = alphaSlotNull;

            if (!File.Exists($"{pathSaveSlot}{slot1File}"))
                slot1_Text.alpha = alphaSlotNull;

            if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
            {
                LoadState(slot3File);
                BaseForExit();
                Timer_KeyPress_Manager.timer = 0;
            }
        }
    }
#endregion

#region LoadState
    void LoadState(string slotFile)
    {
        loadGame = true;
        fileToLoad = slotFile; 

        XmlTextReader XmlTR = new XmlTextReader($"{pathSaveSlot}{slotFile}");
        while (XmlTR.Read())
        {
            //CHIAVI BOOL
            if (XmlTR.MoveToAttribute("Life")) //VITA PLAYER
                PlayerLife_Manager.life = XmlTR.ReadContentAsFloat();

            //CHIAVI BOOL
            if (XmlTR.MoveToAttribute("clellarIsOp")) //BOOL CHIAVE  CANTINA
                Change_room_Manager.CellarIsOpen = XmlTR.ReadContentAsBoolean();
           
            if (XmlTR.MoveToAttribute("sallyRoomIsOp")) //BOOL CHIAVE SALLY'S ROOM
                Change_room_Manager.BedRoomGirlIsOpen = XmlTR.ReadContentAsBoolean();
           
            if (XmlTR.MoveToAttribute("labIsOp")) //BOOL CHIAVE LABORATORIO
                Change_room_Manager.LabIsOpen = XmlTR.ReadContentAsBoolean();

            if(XmlTR.MoveToAttribute("triggerTextOpenCellar")) //BOOL SUL COMPUTER NEL LAB PER APRIRE LA CANTINA
                TriggerSoundOpenCellar.triggerTextOpenCellar = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("trigger2CellarOpen")) //BOOL PER VERIFICARE SE ESEGUIRE IL SUONO DI APERTURA CANTINA NEL LAB
                TriggerSoundOpenCellar.trigger2CellarOpen = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("StrongBoxIsOpen")) //BOOL CASSAFORTE
                StrongBox_manager.StrongBoxIsOpen = XmlTR.ReadContentAsBoolean();

            //STANZA DA CARICARE
            if (XmlTR.MoveToAttribute("room"))
                SceneManager.LoadScene(XmlTR.ReadContentAsString());

            //RICARICA INVENTARIO
            for (int i = 0; i < ObjectInInventory_Manager.myName.Length; i++)
            {
                if (XmlTR.MoveToAttribute($"inv{i}_name") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.myName[i] = XmlTR.ReadContentAsString();
                }
                else if (XmlTR.MoveToAttribute($"inv{i}_nameGameObject") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.nameGameObject[i] = XmlTR.ReadContentAsString();
                }
                else if(XmlTR.MoveToAttribute($"inv{i}_equipable") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.equipable[i] = XmlTR.ReadContentAsBoolean();
                }
                else if (XmlTR.MoveToAttribute($"inv{i}_imagePath") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.objectSpritePath[i] = XmlTR.ReadContentAsString();
                } // SE ESISTE L'OGGETTO HA UNA QUANTITA'
                else if (XmlTR.MoveToAttribute($"inv{i}_quantTextName") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.quantityTextName[i] = XmlTR.ReadContentAsString();
                }
                else if (XmlTR.MoveToAttribute($"inv{i}_quantTextMeshPosX") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.quantityTextMeshPos[i].x = XmlTR.ReadContentAsFloat();
                }
                else if (XmlTR.MoveToAttribute($"inv{i}_quantTextMeshPosY") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.quantityTextMeshPos[i].y = XmlTR.ReadContentAsFloat();
                }
            }

            //OBJECTS TAKE BOOL
            if (XmlTR.MoveToAttribute("FloppyIsTaken"))
                ObjectIsTakeDelete_manager.FloppyIsTaken = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("keyLabIsTaken"))
                ObjectIsTakeDelete_manager.keyLabIsTaken = XmlTR.ReadContentAsBoolean();
            
            if (XmlTR.MoveToAttribute("keySallyIsTaken"))
                ObjectIsTakeDelete_manager.keySallyIsTaken = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("gunIsTake"))
            {
                ObjectIsTakeDelete_manager.gunIsTaken = XmlTR.ReadContentAsBoolean();
            }

            for (int i = 0; i < ObjectIsTakeDelete_manager.munitionIsTaken.Length; i++)
            {
                if (XmlTR.MoveToAttribute($"ammunitionTake_{i}"))
                {
                    ObjectIsTakeDelete_manager.munitionIsTaken[i] = XmlTR.ReadContentAsBoolean();
                }
            }

            if (XmlTR.MoveToAttribute("MedallionIsTaken"))
                ObjectIsTakeDelete_manager.MedallionIsTaken = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("PieceOfClothIsTaken"))
                ObjectIsTakeDelete_manager.pieceOfClothIsTaken = XmlTR.ReadContentAsBoolean();

            //BOOL
            if (XmlTR.MoveToAttribute("IntroVideoIsPlayed"))
                VideoManager.IntroVideoIsPlayed = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("deadBodyVideoIsPlayed"))
                VideoManager.deadBodyVideoIsPlayed = XmlTR.ReadContentAsBoolean();
            
            if (XmlTR.MoveToAttribute("fileIsTaken"))
                FloppyQuestTrigger_Manager.fileIsTaken = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("firePlaceIsOpen"))
                FloppyQuestTrigger_Manager.firePlaceIsOpen = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("pianoPlayed"))
                FloppyQuestTrigger_Manager.pianoPlayed = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("useFloppy"))
                FloppyQuestTrigger_Manager.useFloppy = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("useMusicSheet"))
                FloppyQuestTrigger_Manager.useMusicSheet = XmlTR.ReadContentAsBoolean();
            
            if (XmlTR.MoveToAttribute("hardMode"))
                ChoiceOfDifficultyMenu_Manager.easyMode = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("canTakeColt"))
                TakePython_Quest_Manager.canTakeColt = XmlTR.ReadContentAsBoolean();

            if (XmlTR.MoveToAttribute("coltIsTaken"))
                TakePython_Quest_Manager.coltIsTaken = XmlTR.ReadContentAsBoolean();

            //ENEMY SPAWN
            for (int i = 0; i < Spawn_Enemy_Manager.SpawnZombie.Length; i++)
            {
                if (XmlTR.MoveToAttribute($"SpawnZombie_{i}"))
                    Spawn_Enemy_Manager.SpawnZombie[i] = XmlTR.ReadContentAsBoolean();
            }

            if (XmlTR.MoveToAttribute("SetFirstSpawnEnemy"))
                Spawn_Enemy_Manager.setFirstSpawn = XmlTR.ReadContentAsBoolean();

            //MUNIZIONI E QUANTITA'
            if (XmlTR.MoveToAttribute("gunAmmunition"))
            {
                Shooter_Manager.gunAmmunition = XmlTR.ReadContentAsInt();
            }
            else if (XmlTR.MoveToAttribute("ammunitionInBox"))
            {
                Shooter_Manager.ammunitionBox = XmlTR.ReadContentAsInt();
            }
            else if (XmlTR.MoveToAttribute("numberOfPOC"))
            {
                OpenSaveMenu_Manager.numberOfPOC = XmlTR.ReadContentAsInt();
            }
        }

        FMOD_Sound_Manager.Stop();
        FMOD_Sound_Manager.PlayDecisionSound();
        playMusicOnLoad = true;
        UI_Game_Manager.uiCanOpen = true;
        PlayerMovement.canMove = true;
    }
#endregion

    void BaseForExit()
    {
        isOpen = false;
        CheckSlotNotNull();
    }
}
