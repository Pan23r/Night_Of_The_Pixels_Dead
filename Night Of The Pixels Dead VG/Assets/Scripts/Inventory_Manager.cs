using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

enum ObjectSelectMenu { equip, check, combines}
public class Inventory_Manager : MonoBehaviour
{
    public static bool isOpen = false;
    public static GameObject InventoryObj;
    public static int currentQuadPosition = 0;
    public static Vector2[] selectionPosition = new Vector2[7];
    public static GameObject objSelectMenu;

    bool selectObjectMenuIsActive = false;
    bool checkIsActive = false;
    bool combinesIsActive = false;
    string combinesObj1;
    string combinesObj2;

    Image selectionQuad;
    Image selectionQuadCombine;
    TextMeshProUGUI objectInInventory;
    ObjectSelectMenu selectAction;
    TextMeshProUGUI equipText;
    TextMeshProUGUI checkText;
    TextMeshProUGUI combinesText;

    // Start is called before the first frame update
    void Start()
    {
        InventoryObj = GameObject.Find("Inventory");
        selectionQuad = GameObject.Find("Selection_Quad").GetComponent<Image>();
        selectionQuadCombine = GameObject.Find("Selection_Quad_Combine").GetComponent<Image>();
        objectInInventory = GameObject.Find("Text_Descrizione").GetComponent<TextMeshProUGUI>();

        //sotto menu per gestire gli oggetti
        objSelectMenu = GameObject.Find("ObjSelectMenu");
        equipText = GameObject.Find("Equip").GetComponent<TextMeshProUGUI>();
        checkText = GameObject.Find("Check").GetComponent<TextMeshProUGUI>();
        combinesText = GameObject.Find("Combines").GetComponent<TextMeshProUGUI>();

        selectAction = ObjectSelectMenu.equip;

        //Prendo oggetto che fa vedere cosa ho equipaggiato
        ObjectInInventory_Manager.showEquipObj = GameObject.Find("ObjEquiped").GetComponent<Image>();

        if(ObjectInInventory_Manager.objToEquip != null)
            ObjectInInventory_Manager.showEquipObj.color = new Color(45, 60, 90);

        CreateAllPositionSelection();

        ObjectInInventory_Manager.InventoryRecreate();

        if (Load_Manager.loadGame)
        {
            XmlTextReader XmlTR = new XmlTextReader($"{Load_Manager.pathSaveSlot}{Load_Manager.fileToLoad}");
            while (XmlTR.Read())
            {
                //OBJECT EQUIPED
                if (XmlTR.MoveToAttribute("obj_Equiped") && XmlTR.ReadContentAsString() != "")
                {
                    ObjectInInventory_Manager.EquipObject(Resources.Load<Sprite>($"Sprites/{XmlTR.ReadContentAsString()}"));
                }
            }
        }

        if (InventoryObj != null)
            InventoryObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("text_Gun_Obj") != null)
            GameObject.Find("text_Gun_Obj").GetComponent<TextMeshProUGUI>().text = Shooter_Manager.gunAmmunition.ToString();

        if (GameObject.Find("text_Magnum_Obj") != null)
            GameObject.Find("text_Magnum_Obj").GetComponent<TextMeshProUGUI>().text = Shooter_Manager.magnumAmmunition.ToString();

        if (GameObject.Find("text_Eagle_Obj") != null)
            GameObject.Find("text_Eagle_Obj").GetComponent<TextMeshProUGUI>().text = Shooter_Manager.ammunitionBox.ToString();
        
        if (GameObject.Find("text_PieceOfCloth_Obj") != null)
            GameObject.Find("text_PieceOfCloth_Obj").GetComponent<TextMeshProUGUI>().text = OpenSaveMenu_Manager.numberOfPOC.ToString();

        if (KeyBoardOrPadController.Key_Esc && !selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge || KeyBoardOrPadController.Key_Enter && !selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            ExitFromInvenctory(true);
        }
        InventoryController();
        selectionQuad.rectTransform.anchoredPosition = selectionPosition[currentQuadPosition];

        if (!combinesIsActive)
            selectionQuadCombine.enabled = false;

        if (!checkIsActive)
        {
            objectInInventory.text = ObjectInInventory_Manager.NameObject(currentQuadPosition);

            if(!combinesIsActive)
                ObjectSelectMenuController();
        }
        ObjectSelectMenuAction();
        
        if (ObjectInInventory_Manager.objToEquip != null)
            ObjectInInventory_Manager.showEquipObj.overrideSprite = ObjectInInventory_Manager.objToEquip;
    }

    #region Inventory Controller
    private void InventoryController()
    {
        if (KeyBoardOrPadController.Key_Space && !selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            if (ObjectInInventory_Manager.myName[currentQuadPosition] != null)
            {
                FMOD_Sound_Manager.PlayDecisionInvenctorySound();
                selectObjectMenuIsActive = true;
                objSelectMenu.SetActive(true);
                Timer_KeyPress_Manager.timer = 0;
            }
        }

        if(!selectObjectMenuIsActive || combinesIsActive)
            SelectObjectCOntroller();
    }
    #endregion

    #region Slect Object Controller
    void SelectObjectCOntroller()
    {
        if (KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (currentQuadPosition)
            {
                case 0:
                    currentQuadPosition = 1;
                    break;

                case 1:
                    currentQuadPosition = 2;
                    break;

                case 2:
                    currentQuadPosition = 3;
                    break;

                case 3:
                    currentQuadPosition = 4;
                    break;

                case 4:
                    currentQuadPosition = 5;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (currentQuadPosition)
            {
                case 1:
                    currentQuadPosition = 0;
                    break;

                case 2:
                    currentQuadPosition = 1;
                    break;

                case 3:
                    currentQuadPosition = 2;
                    break;

                case 4:
                    currentQuadPosition = 3;
                    break;

                case 5:
                    currentQuadPosition = 4;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (currentQuadPosition)
            {
                case 0:
                    currentQuadPosition = 2;
                    break;

                case 2:
                    currentQuadPosition = 4;
                    break;

                case 1:
                    currentQuadPosition = 3;
                    break;

                case 3:
                    currentQuadPosition = 5;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (currentQuadPosition)
            {
                case 4:
                    currentQuadPosition = 2;
                    break;

                case 2:
                    currentQuadPosition = 0;
                    break;

                case 5:
                    currentQuadPosition = 3;
                    break;

                case 3:
                    currentQuadPosition = 1;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    #region Create all position Selection
    private void CreateAllPositionSelection()
    {
        selectionPosition[0] = new Vector2(562f, 321);
        selectionPosition[1] = new Vector2(905f, 321);
        selectionPosition[2] = new Vector2(565f, -26.5f);
        selectionPosition[3] = new Vector2(905f, -26.5f);
        selectionPosition[4] = new Vector2(565f, -375.6f);
        selectionPosition[5] = new Vector2(905f, -375.2f);
    }
    #endregion

    #region Object Select Menu Action
    void ObjectSelectMenuAction()
    {
        switch (selectAction)
        {
            case ObjectSelectMenu.equip:
                EquipController();
                break;

            case ObjectSelectMenu.check:
                CheckController();
                break;

            case ObjectSelectMenu.combines:
                CombinesController();
                break;
        }
    }
    #endregion

    #region Object Select Menu Controller
    void ObjectSelectMenuController()
    {
        if (KeyBoardOrPadController.Key_Right && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (selectAction)
            {
                case ObjectSelectMenu.equip:
                    selectAction = ObjectSelectMenu.check;
                    break;

                case ObjectSelectMenu.check:
                    selectAction = ObjectSelectMenu.combines;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayCursorInvenctorySound();
            switch (selectAction)
            {
                case ObjectSelectMenu.combines:
                    selectAction = ObjectSelectMenu.check;
                    break;

                case ObjectSelectMenu.check:
                    selectAction = ObjectSelectMenu.equip;
                    break;
            }
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Esc && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge || KeyBoardOrPadController.Key_Enter && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayBackSound();
            ExitFromObjSelectMenu();
        }
    }
    #endregion

    #region Equip Controller
    void EquipController()
    {
        equipText.color = Color.yellow;
        checkText.color = Color.white;
        combinesText.color = Color.white;

        if (ObjectInInventory_Manager.equipable[currentQuadPosition])
            equipText.text = Translation_Manager.EquipsdSelect;
        else
            equipText.text = Translation_Manager.useSelect;

        if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && ObjectInInventory_Manager.objToEquip == null && ObjectInInventory_Manager.equipable[currentQuadPosition] && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge ||
            KeyBoardOrPadController.Key_Space && ObjectInInventory_Manager.showEquipObj.overrideSprite != null && ObjectInInventory_Manager.objectImage[currentQuadPosition] != null && ObjectInInventory_Manager.objectImage[currentQuadPosition].overrideSprite != ObjectInInventory_Manager.showEquipObj.overrideSprite && ObjectInInventory_Manager.equipable[currentQuadPosition] && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayEquipSound();
            ObjectInInventory_Manager.EquipObject(Resources.Load<Sprite>($"Sprites/{ObjectInInventory_Manager.objectSpritePath[currentQuadPosition]}"));
            Timer_KeyPress_Manager.timer = 0;
            ExitFromObjSelectMenu();
        }
        else if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && ObjectInInventory_Manager.objectImage[currentQuadPosition].overrideSprite == ObjectInInventory_Manager.showEquipObj.overrideSprite && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayUnequipSound();
            ObjectInInventory_Manager.UnequipsObject();
            Timer_KeyPress_Manager.timer = 0;
            ExitFromObjSelectMenu();
        }
        else if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && ObjectIstantiator_Manager.ReturnUsableObj(ObjectInInventory_Manager.nameGameObject[currentQuadPosition]))
        {
            FMOD_Sound_Manager.PlayDecisionInvenctorySound();
            GameObject objToDespawn = GameObject.Find(PlayerMovement.ObjectCollideName);
            objToDespawn.SetActive(false);

            TextBoxObjectTake_Manager.ActivateTextBox(ObjectInInventory_Manager.myName[currentQuadPosition], false);
            ObjectInInventory_Manager.SearchObjAndDelete(ObjectInInventory_Manager.nameGameObject[currentQuadPosition]);
            Timer_KeyPress_Manager.timer = 0;
            ExitFromObjSelectMenu();
            ExitFromInvenctory(false);
            GameMenu_Manager.ExitFromGameMenu(false);
            objToDespawn.SetActive(true);
        }
        else if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !ObjectIstantiator_Manager.ReturnUsableObj(ObjectInInventory_Manager.nameGameObject[currentQuadPosition]))
        {
            FMOD_Sound_Manager.PlayNullSound();
            Timer_KeyPress_Manager.timer = 0;
        }
    }
    #endregion

    #region Check Controller
    void CheckController()
    {
        equipText.color = Color.white;
        checkText.color = Color.yellow;
        combinesText.color = Color.white; 
        
        if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && !checkIsActive &&Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayDecisionInvenctorySound();
            checkIsActive = true;
            objectInInventory.text = ObjectIstantiator_Manager.ReturnExaminationText(ObjectInInventory_Manager.nameGameObject[currentQuadPosition]);
            Timer_KeyPress_Manager.timer = 0;
        }
        else if(ReturnKey() && selectObjectMenuIsActive && checkIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayBackSound();
            checkIsActive = false;
            objectInInventory.text = ObjectInInventory_Manager.myName[currentQuadPosition];
            Timer_KeyPress_Manager.timer = 0;
        }
    }

    bool ReturnKey()
    {
        if (KeyBoardOrPadController.Key_Space)
            return true;
        else if (KeyBoardOrPadController.Key_Esc)
            return true;
        else if (KeyBoardOrPadController.Key_Enter)
            return true;

        return false;
    }
    #endregion

    #region Combines Controller
    void CombinesController()
    {
        equipText.color = Color.white;
        checkText.color = Color.white;
        combinesText.color = Color.yellow;

        if (KeyBoardOrPadController.Key_Space && selectObjectMenuIsActive && !combinesIsActive && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayDecisionInvenctorySound();
            combinesIsActive = true;
            selectionQuadCombine.enabled = true;
            selectionQuadCombine.rectTransform.anchoredPosition = selectionQuad.rectTransform.anchoredPosition;
            Timer_KeyPress_Manager.timer = 0;
        }

        if (combinesIsActive)
        {
            if (combinesObj1 == null)
            {
                combinesObj1 = ObjectInInventory_Manager.nameGameObject[currentQuadPosition];
                //Debug.Log("INSERITO OGGETTO 1");
            }

            if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && combinesObj2 == null)
            {
                combinesObj2 = ObjectInInventory_Manager.nameGameObject[currentQuadPosition];
                //Debug.Log("INSERITO OGGETTO 2");

                Timer_KeyPress_Manager.timer = 0;
            }
            else if (combinesObj1 != null && combinesObj2 != null)
            {
                if (ObjectIstantiator_Manager.CombineObjects(combinesObj1, combinesObj2))
                {
                    FMOD_Sound_Manager.PlayDecisionInvenctorySound();
                    ExitFromCombinesMod();
                    ExitFromObjSelectMenu();
                    //Debug.Log("COMBINAZIONE AVVENUTA / USCITA");
                }
                else
                {
                    FMOD_Sound_Manager.PlayNullSound();
                    combinesObj2 = null;
                    //Debug.Log("INSERIRE NUOVAMENTE OGGETTO 2");
                }
            }

            if (KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge || KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
            {
                ExitFromCombinesMod();
                //Debug.Log("USCITA DALLA COMBINES MOD");
            }
        }
    }
    #endregion

    #region Exit From CombinesMod
    void ExitFromCombinesMod()
    {
        combinesObj1 = null;
        combinesObj2 = null;
        combinesIsActive = false;
    }
    #endregion

    void ExitFromObjSelectMenu()
    {
        selectAction = ObjectSelectMenu.equip;
        selectObjectMenuIsActive = false;
        objSelectMenu.SetActive(false);
        Timer_KeyPress_Manager.timer = 0;
    }

    void ExitFromInvenctory(bool playSound)
    {
        if(playSound)
            FMOD_Sound_Manager.PlayBackSound();
        
        InventoryObj.SetActive(false);
        isOpen = false;
        GameMenu_Manager.menuInGame.SetActive(true);
        Timer_KeyPress_Manager.timer = 0;
        currentQuadPosition = 0;
    }
}
