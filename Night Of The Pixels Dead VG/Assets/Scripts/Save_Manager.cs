using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


enum SaveMenu { slot1, slot2, slot3}
public class Save_Manager : MonoBehaviour
{
    string CharacterSelect = "Barbara"; //NOME DEL PLAYER DA DOVER CAMBIARE
    public static GameObject saveMenu;
    public static bool isOpen = false;
    public static bool canOpenSave = false;

    SaveMenu slotSelection;
    static TextMeshProUGUI slot1_Text;
    static TextMeshProUGUI slot2_Text;
    static TextMeshProUGUI slot3_Text;
    static float alphaSlotNull = 0.5f;

    Image backGround1;
    Image backGround2;
    float startPositionX = -960f;
    float startPositionY = 540f;
    float velocityImage = 100f;

    // Start is called before the first frame update
    void Start()
    {
        slotSelection = SaveMenu.slot1;
        saveMenu = GameObject.Find("SaveMenu");
        slot1_Text = GameObject.Find("SaveState_1").GetComponent<TextMeshProUGUI>();
        slot2_Text = GameObject.Find("SaveState_2").GetComponent<TextMeshProUGUI>();
        slot3_Text = GameObject.Find("SaveState_3").GetComponent<TextMeshProUGUI>();
        backGround1 = GameObject.Find("Save_BackGround1").GetComponent<Image>();
        backGround2 = GameObject.Find("Save_BackGround2").GetComponent<Image>();

        backGround1.rectTransform.localPosition = new Vector2(startPositionX, startPositionY);
        backGround2.rectTransform.localPosition = new Vector2(backGround1.rectTransform.localPosition.x + backGround1.rectTransform.rect.width, startPositionY);

        DrawAllSavesSlot();

        saveMenu.SetActive(false);
    }

    public static void DrawAllSavesSlot()
    {
        //SLOT 1
        if (!File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot1File}"))
        {
            if (slot1_Text != null)
                slot1_Text.alpha = alphaSlotNull;
        }
        else
            XmlFileReader(Load_Manager.slot1File, slot1_Text);

        //SLOT 2
        if (!File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot2File}"))
        {
            if (slot2_Text != null)
                slot2_Text.alpha = alphaSlotNull;
        }
        else
            XmlFileReader(Load_Manager.slot2File, slot2_Text);

        //SLOT 3
        if (!File.Exists($"{Load_Manager.pathSaveSlot}{Load_Manager.slot3File}"))
        {
            if (slot3_Text != null)
                slot3_Text.alpha = alphaSlotNull;
        }
        else
            XmlFileReader(Load_Manager.slot3File, slot3_Text);
    }
        

    #region Xml File Reader
    static void XmlFileReader(string slot, TextMeshProUGUI textSlot)
    {
        string namePlayerText = "";
        string dateText = "";
        string timeText = "";
        string gameMode = "";

        XmlTextReader XmlTR = new XmlTextReader($"{Load_Manager.pathSaveSlot}{slot}");

        while (XmlTR.Read())
        {
            if (XmlTR.MoveToAttribute("namePlayer"))
            {
                namePlayerText = XmlTR.ReadContentAsString();
            }
            else if (XmlTR.MoveToAttribute("date"))
            {
                dateText = XmlTR.ReadContentAsString();
            }
            else if (XmlTR.MoveToAttribute("time"))
            {
                timeText = XmlTR.ReadContentAsString();
            }
            else if (XmlTR.MoveToAttribute("hardMode")) 
            {
                gameMode = (XmlTR.ReadContentAsBoolean())? Translation_Manager.easy : Translation_Manager.hard;
            }
        }

        textSlot.text = $"{namePlayerText} {dateText} {timeText} - {gameMode}";
    }
    #endregion
    
    // Update is called once per frame
    void Update()
    {
        UI_Game_Manager.uiCanOpen = false;
        if (!QuestionOverload_Manager.isOpen)
        {
            SaveMenuController();
            SelectSlotController();
        }

        BackGroundController();
    }

    #region Save Menu Controller
    void SaveMenuController()
    {
        if (KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge || KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayBackSound();
            saveMenu.SetActive(false);
            PlayerMovement.canMove = true;
            BaseForExit();
            UI_Game_Manager.uiCanOpen = true;
            Timer_KeyPress_Manager.timer = 0;
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
            FMOD_Sound_Manager.PlayCursorSound();
            switch (slotSelection)
            {
                case SaveMenu.slot1:
                    slotSelection = SaveMenu.slot2;
                    break;

                case SaveMenu.slot2:
                    slotSelection = SaveMenu.slot3;
                    break;

                case SaveMenu.slot3:
                    slotSelection = SaveMenu.slot1;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            switch (slotSelection)
            {
                case SaveMenu.slot1:
                    slotSelection = SaveMenu.slot3;
                    break;

                case SaveMenu.slot2:
                    slotSelection = SaveMenu.slot1;
                    break;

                case SaveMenu.slot3:
                    slotSelection = SaveMenu.slot2;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }

        switch (slotSelection)
        {
            case SaveMenu.slot1:
                Slot1Select();
                break;

            case SaveMenu.slot2:
                Slot2Select();
                break;

            case SaveMenu.slot3:
                Slot3Select();
                break;
        }
    }
    #endregion

    #region Slot 1 Select
    void Slot1Select()
    {
        slot1_Text.color = Color.yellow;
        slot2_Text.color = Color.white;
        slot3_Text.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            //CREAZIONE DEL FILE SAVE
            SaveState(Load_Manager.slot1File, slot1_Text);
            Timer_KeyPress_Manager.timer = 0;
        }

        CheckOverrideSave(Load_Manager.slot1File, slot1_Text);
    }
    #endregion

    #region Slot 2 Select
    void Slot2Select()
    {
        slot2_Text.color = Color.yellow;
        slot1_Text.color = Color.white;
        slot3_Text.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            //CREAZIONE DEL FILE SAVE
            SaveState(Load_Manager.slot2File, slot2_Text);
            Timer_KeyPress_Manager.timer = 0;
        }

        CheckOverrideSave(Load_Manager.slot2File, slot2_Text);
    }
    #endregion

    #region Slot 3 Select
    void Slot3Select()
    {
        slot3_Text.color = Color.yellow;
        slot2_Text.color = Color.white;
        slot1_Text.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            //CREAZIONE DEL FILE SAVE
            SaveState(Load_Manager.slot3File, slot3_Text);
            Timer_KeyPress_Manager.timer = 0;
        }

        CheckOverrideSave(Load_Manager.slot3File, slot3_Text);
    }
    #endregion

    #region Create Xml
    void CreateXml(XmlDocument doc, XmlElement root, string nameAttributeString, string valueString)
    {
        XmlElement tile = doc.CreateElement("tile");
        root.AppendChild(tile);
        XmlAttribute attribute = doc.CreateAttribute(nameAttributeString);

        if (valueString == "True")
            valueString = "true";
        else if(valueString == "False")
            valueString = "false";

        attribute.Value = valueString;
        tile.Attributes.Append(attribute);
    }
    #endregion

    #region Save State
    void SaveState(string slotFile, TextMeshProUGUI textToUpdate)
    {
        if (!File.Exists($"{Load_Manager.pathSaveSlot}{slotFile}"))
        {
            //VIENE CREATO
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("data");
            doc.AppendChild(root);

            //Nome del Player Selezionato
            CreateXml(doc, root, "namePlayer", CharacterSelect);

            //Data
            CreateXml(doc, root, "date", System.DateTime.Now.ToString("dd/MM/yyyy"));

            //Ora
            CreateXml(doc, root, "time", System.DateTime.Now.ToString("HH:mm:ss"));

            //Posizione Player
            CreateXml(doc, root, "positionX", GameObject.Find("Player").GetComponent<Transform>().transform.position.x.ToString().Replace(',', '.'));            
            CreateXml(doc, root, "positionY", GameObject.Find("Player").GetComponent<Transform>().transform.position.y.ToString().Replace(',', '.'));
            
            //SAVE Life Player
            CreateXml(doc, root, "Life", PlayerLife_Manager.life.ToString());

            //ROOM LOAD & BOOL
            CreateXml(doc, root, "room", SceneManager.GetActiveScene().name);

            CreateXml(doc, root, "clellarIsOp", Change_room_Manager.CellarIsOpen.ToString());
            CreateXml(doc, root, "sallyRoomIsOp", Change_room_Manager.BedRoomGirlIsOpen.ToString());
            CreateXml(doc, root, "labIsOp", Change_room_Manager.LabIsOpen.ToString());

            CreateXml(doc, root, "triggerTextOpenCellar", TriggerSoundOpenCellar.triggerTextOpenCellar.ToString());
            CreateXml(doc, root, "trigger2CellarOpen", TriggerSoundOpenCellar.trigger2CellarOpen.ToString());
            
            CreateXml(doc, root, "IntroVideoIsPlayed", VideoManager.IntroVideoIsPlayed.ToString());
            CreateXml(doc, root, "deadBodyVideoIsPlayed", VideoManager.deadBodyVideoIsPlayed.ToString());

            CreateXml(doc, root, "fileIsTaken", FloppyQuestTrigger_Manager.fileIsTaken.ToString());
            CreateXml(doc, root, "firePlaceIsOpen", FloppyQuestTrigger_Manager.firePlaceIsOpen.ToString());
            CreateXml(doc, root, "pianoPlayed", FloppyQuestTrigger_Manager.pianoPlayed.ToString());
            CreateXml(doc, root, "useFloppy", FloppyQuestTrigger_Manager.useFloppy.ToString());
            CreateXml(doc, root, "useMusicSheet", FloppyQuestTrigger_Manager.useMusicSheet.ToString());

            CreateXml(doc, root, "StrongBoxIsOpen", StrongBox_manager.StrongBoxIsOpen.ToString());

            CreateXml(doc, root, "hardMode", ChoiceOfDifficultyMenu_Manager.easyMode.ToString());

            CreateXml(doc, root, "canTakeColt", TakePython_Quest_Manager.canTakeColt.ToString());
            CreateXml(doc, root, "coltIsTaken", TakePython_Quest_Manager.coltIsTaken.ToString());

            //ENEMY SPAWN
            for (int i = 0; i< Spawn_Enemy_Manager.SpawnZombie.Length; i++)
            {
                CreateXml(doc, root, $"SpawnZombie_{i}", Spawn_Enemy_Manager.SpawnZombie[i].ToString());
            }

            CreateXml(doc, root, $"SetFirstSpawnEnemy", Spawn_Enemy_Manager.setFirstSpawn.ToString());

            //OBJECTS TAKES
            CreateXml(doc, root, "gunIsTake", ObjectIsTakeDelete_manager.gunIsTaken.ToString());

            for (int i = 0; i < ObjectIsTakeDelete_manager.munitionIsTaken.Length; i++)
            {
                CreateXml(doc, root, $"ammunitionTake_{i}", ObjectIsTakeDelete_manager.munitionIsTaken[i].ToString());
            }
                       
            CreateXml(doc, root, "FloppyIsTaken", ObjectIsTakeDelete_manager.FloppyIsTaken.ToString());
            CreateXml(doc, root, "keyLabIsTaken", ObjectIsTakeDelete_manager.keyLabIsTaken.ToString());
            CreateXml(doc, root, "keySallyIsTaken", ObjectIsTakeDelete_manager.keySallyIsTaken.ToString());
            CreateXml(doc, root, "MedallionIsTaken", ObjectIsTakeDelete_manager.MedallionIsTaken.ToString());
            CreateXml(doc, root, "PieceOfClothIsTaken", ObjectIsTakeDelete_manager.pieceOfClothIsTaken.ToString());


            //MUNIZIONI E QUANTITA'
            CreateXml(doc, root, "gunAmmunition", Shooter_Manager.gunAmmunition.ToString());
            CreateXml(doc, root, "ammunitionInBox", Shooter_Manager.ammunitionBox.ToString());
            CreateXml(doc, root, "numberOfPOC", OpenSaveMenu_Manager.numberOfPOC.ToString());

            //EQUIP OBJECT
            if (ObjectInInventory_Manager.objToEquip != null)
                CreateXml(doc, root, "obj_Equiped", ObjectInInventory_Manager.objToEquip.name.ToString());
            else
                CreateXml(doc, root, "obj_Equiped", "");

            //INVENCTORY
            for (int i = 0; i < ObjectInInventory_Manager.myName.Length; i++)
            {
                if(ObjectInInventory_Manager.myName[i] != null)
                    CreateXml(doc, root, $"inv{i}_name", ObjectInInventory_Manager.myName[i]);
                else
                    CreateXml(doc, root, $"inv{i}_name", "");
                
                if (ObjectInInventory_Manager.nameGameObject[i] != null)
                    CreateXml(doc, root, $"inv{i}_nameGameObject", ObjectInInventory_Manager.nameGameObject[i]);
                else
                    CreateXml(doc, root, $"inv{i}_nameGameObject", "");

                CreateXml(doc, root, $"inv{i}_equipable", ObjectInInventory_Manager.equipable[i].ToString());

                if (ObjectInInventory_Manager.objectSpritePath[i] != null)
                    CreateXml(doc, root, $"inv{i}_imagePath", ObjectInInventory_Manager.objectSpritePath[i]);
                else
                    CreateXml(doc, root, $"inv{i}_imagePath", "");

                //SE L'OGGETTO HA UNA QUANTITA'
                if (ObjectInInventory_Manager.quantityTextName[i] != null)
                    CreateXml(doc, root, $"inv{i}_quantTextName", ObjectInInventory_Manager.quantityTextName[i]);
                else
                    CreateXml(doc, root, $"inv{i}_quantTextName", "");

                if(ObjectInInventory_Manager.quantityTextMeshPos[i] != null && ObjectInInventory_Manager.quantityTextName[i] != null)
                {
                    CreateXml(doc, root, $"inv{i}_quantTextMeshPosX", ObjectInInventory_Manager.quantityTextMeshPos[i].x.ToString().Replace(',', '.'));
                    CreateXml(doc, root, $"inv{i}_quantTextMeshPosY", ObjectInInventory_Manager.quantityTextMeshPos[i].y.ToString().Replace(',', '.'));
                }
                else
                {
                    CreateXml(doc, root, $"inv{i}_quantTextMeshPosX", "");
                    CreateXml(doc, root, $"inv{i}_quantTextMeshPosY", "");
                }
                    
            }
            
            doc.Save($"{Load_Manager.pathSaveSlot}{slotFile}");

            //AGGIORNA IL TESTO NEL MENU
            UpdateTextAfterSave(slotFile, textToUpdate);
            FMOD_Sound_Manager.PlayDecisionSound();
        }
        else
        {
            QuestionOverload_Manager.question.SetActive(true);
            QuestionOverload_Manager.isOpen = true;
        }
    }
    #endregion

    #region Update Text After Save
    void UpdateTextAfterSave(string slotFile, TextMeshProUGUI textToUpdate)
    {
        string nameLoad = "";
        string dateLoad = "";
        string hourLoad = "";
        string gameMode = "";

        XmlTextReader XmlTR = new XmlTextReader($"{Load_Manager.pathSaveSlot}{slotFile}");
        while (XmlTR.Read())
        {
            if (XmlTR.MoveToAttribute("namePlayer"))
                nameLoad = XmlTR.ReadContentAsString();

            if (XmlTR.MoveToAttribute("date"))
                dateLoad = XmlTR.ReadContentAsString();

            if (XmlTR.MoveToAttribute("time"))
                hourLoad = XmlTR.ReadContentAsString();

            if (XmlTR.MoveToAttribute("hardMode"))
                gameMode = (XmlTR.ReadContentAsBoolean()) ? Translation_Manager.easy : Translation_Manager.hard;
        }

        textToUpdate.text = $"{nameLoad} {dateLoad} {hourLoad} - {gameMode}";
    }
    #endregion

    #region Override Save State
    void OverrideSaveState(string slotFile, TextMeshProUGUI textToUpdate)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load($"{Load_Manager.pathSaveSlot}{slotFile}");
        //nome del player
        OverrideXml(doc, "namePlayer", CharacterSelect);
        //data
        OverrideXml(doc, "date", System.DateTime.Now.ToString("dd/MM/yyyy"));
        //ora
        OverrideXml(doc, "time", System.DateTime.Now.ToString("HH:mm:ss"));
        //posizione player
        OverrideXml(doc, "positionX", GameObject.Find("Player").GetComponent<Transform>().transform.position.x.ToString().Replace(',', '.'));
        OverrideXml(doc, "positionY", GameObject.Find("Player").GetComponent<Transform>().transform.position.y.ToString().Replace(',', '.'));
        
        //SAVE LIFE PLAYER
        OverrideXml(doc, "Life", PlayerLife_Manager.life.ToString());

        //ROOM LOAD & BOOL
        OverrideXml(doc, "room", SceneManager.GetActiveScene().name);

        OverrideXml(doc, "clellarIsOp", Change_room_Manager.CellarIsOpen.ToString());
        OverrideXml(doc, "sallyRoomIsOp", Change_room_Manager.BedRoomGirlIsOpen.ToString());
        OverrideXml(doc, "labIsOp", Change_room_Manager.LabIsOpen.ToString());

        OverrideXml(doc, "triggerTextOpenCellar", TriggerSoundOpenCellar.triggerTextOpenCellar.ToString());
        OverrideXml(doc, "trigger2CellarOpen", TriggerSoundOpenCellar.trigger2CellarOpen.ToString());
        
        OverrideXml(doc, "FloppyIsTaken", ObjectIsTakeDelete_manager.FloppyIsTaken.ToString());
        OverrideXml(doc, "keyLabIsTaken", ObjectIsTakeDelete_manager.keyLabIsTaken.ToString());
        OverrideXml(doc, "keySallyIsTaken", ObjectIsTakeDelete_manager.keySallyIsTaken.ToString());
        OverrideXml(doc, "MedallionIsTaken", ObjectIsTakeDelete_manager.MedallionIsTaken.ToString());
        OverrideXml(doc, "PieceOfClothIsTaken", ObjectIsTakeDelete_manager.pieceOfClothIsTaken.ToString());

        OverrideXml(doc, "IntroVideoIsPlayed", VideoManager.IntroVideoIsPlayed.ToString());
        OverrideXml(doc, "deadBodyVideoIsPlayed", VideoManager.deadBodyVideoIsPlayed.ToString());
        
        OverrideXml(doc, "fileIsTaken", FloppyQuestTrigger_Manager.fileIsTaken.ToString());
        OverrideXml(doc, "firePlaceIsOpen", FloppyQuestTrigger_Manager.firePlaceIsOpen.ToString());
        OverrideXml(doc, "pianoPlayed", FloppyQuestTrigger_Manager.pianoPlayed.ToString());
        OverrideXml(doc, "useFloppy", FloppyQuestTrigger_Manager.useFloppy.ToString());
        OverrideXml(doc, "useMusicSheet", FloppyQuestTrigger_Manager.useMusicSheet.ToString());

        OverrideXml(doc, "StrongBoxIsOpen", StrongBox_manager.StrongBoxIsOpen.ToString());
        OverrideXml(doc, "hardMode", ChoiceOfDifficultyMenu_Manager.easyMode.ToString());

        OverrideXml(doc, "canTakeColt", TakePython_Quest_Manager.canTakeColt.ToString());
        OverrideXml(doc, "coltIsTaken", TakePython_Quest_Manager.coltIsTaken.ToString());

        //ENEMY SPAWN
        for (int i = 0; i < Spawn_Enemy_Manager.SpawnZombie.Length; i++)
        {
            OverrideXml(doc, $"SpawnZombie_{i}", Spawn_Enemy_Manager.SpawnZombie[i].ToString());
        }

        OverrideXml(doc, "SetFirstSpawnEnemy", Spawn_Enemy_Manager.setFirstSpawn.ToString());

        //OBJECTS TAKES
        OverrideXml(doc, "gunIsTake", ObjectIsTakeDelete_manager.gunIsTaken.ToString());

        for (int i = 0; i < ObjectIsTakeDelete_manager.munitionIsTaken.Length; i++)
        {
            OverrideXml(doc, $"ammunitionTake_{i}", ObjectIsTakeDelete_manager.munitionIsTaken[i].ToString());
        }
        //MUNIZIONI E QUANTITA'
        OverrideXml(doc, "gunAmmunition", Shooter_Manager.gunAmmunition.ToString());
        OverrideXml(doc, "ammunitionInBox", Shooter_Manager.ammunitionBox.ToString());
        OverrideXml(doc, "numberOfPOC", OpenSaveMenu_Manager.numberOfPOC.ToString());

        //EQUIP OBJECT
        if (ObjectInInventory_Manager.objToEquip != null)
            OverrideXml(doc, "obj_Equiped", ObjectInInventory_Manager.objToEquip.name.ToString());
        else
            OverrideXml(doc, "obj_Equiped", "");
        //INVENCTORY
        for (int i = 0; i < ObjectInInventory_Manager.myName.Length; i++)
        {
            if (ObjectInInventory_Manager.myName[i] != null)
                OverrideXml(doc, $"inv{i}_name", ObjectInInventory_Manager.myName[i]);
            else
                OverrideXml(doc, $"inv{i}_name", "");
            
            if (ObjectInInventory_Manager.nameGameObject[i] != null)
                OverrideXml(doc, $"inv{i}_nameGameObject", ObjectInInventory_Manager.nameGameObject[i]);
            else
                OverrideXml(doc, $"inv{i}_nameGameObject", "");

            OverrideXml(doc, $"inv{i}_equipable", ObjectInInventory_Manager.equipable[i].ToString());

            if (ObjectInInventory_Manager.objectSpritePath[i] != null)
                OverrideXml(doc, $"inv{i}_imagePath", ObjectInInventory_Manager.objectSpritePath[i]);
            else
                OverrideXml(doc, $"inv{i}_imagePath", "");

            //SE L'OGGETTO HA UNA QUANTITA'
            if (ObjectInInventory_Manager.quantityTextName[i] != null)
                OverrideXml(doc, $"inv{i}_quantTextName", ObjectInInventory_Manager.quantityTextName[i]);
            else
                OverrideXml(doc, $"inv{i}_quantTextName", "");

            if (ObjectInInventory_Manager.quantityTextMeshPos[i] != null)
            {
                OverrideXml(doc, $"inv{i}_quantTextMeshPosX", ObjectInInventory_Manager.quantityTextMeshPos[i].x.ToString().Replace(',', '.'));
                OverrideXml(doc, $"inv{i}_quantTextMeshPosY", ObjectInInventory_Manager.quantityTextMeshPos[i].y.ToString().Replace(',', '.'));
            }
            else
            {
                OverrideXml(doc, $"inv{i}_quantTextMeshPosX", "");
                OverrideXml(doc, $"inv{i}_quantTextMeshPosY", "");
            }
        }

        doc.Save($"{Load_Manager.pathSaveSlot}{slotFile}");

        //AGGIORNA IL TESTO NEL MENU
        UpdateTextAfterSave(slotFile, textToUpdate);
        FMOD_Sound_Manager.PlayDecisionSound();
    }
    #endregion

    #region Override Xml
    void OverrideXml(XmlDocument doc, string nameValue, string value)
    {
        XmlAttribute attribute = (XmlAttribute)doc.SelectSingleNode($"data/tile/@{nameValue}");

        if (value == "True")
            value = "true";
        else if (value == "False")
            value = "false";

        attribute.Value = value;
    }
    #endregion

    void BaseForExit()
    {
        isOpen = false;
        slotSelection = SaveMenu.slot1;
    }

    void CheckOverrideSave(string slotFile, TextMeshProUGUI textToUpdate)
    {
        if (QuestionOverload_Manager.overrideSave)
        {
            OverrideSaveState(slotFile, textToUpdate);
            QuestionOverload_Manager.overrideSave = false;
        }
    }
}
