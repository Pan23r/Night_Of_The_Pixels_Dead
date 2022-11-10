using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectInInventory_Manager : MonoBehaviour
{
    public static string[] myName = new string[7]; 
    public static string[] nameGameObject = new string[7]; // AGGIUNGERE AL LOAD E SAVE MANAGER
    public static Image[] objectImage = new Image[7];
    public static bool[] equipable = new bool[7];

    public static string[] quantityTextName = new string[7];
    public static Vector2[] quantityTextMeshPos = new Vector2[7];

    public static GameObject[] gameObjRef = new GameObject[7];
    public static GameObject[] textQuantObjRef = new GameObject[7];
    public static string[] objectSpritePath = new string[7];

    public static Image showEquipObj;
    public static Sprite objToEquip;

    #region Put Object in Inventory
    public static bool PutObject(string name, string nameGameObj ,string spriteName,bool isEquipable, ref GameObject gameObjReference, GameObject parent, int quantity = -1, float quantTextPosX = 0, float quantTextPosY = 0)
    {
        for (int i = 0; i < myName.Length; i++)
        {
            if (myName[i] == null && i < myName.Length - 1)
            {
                myName[i] = name;
                nameGameObject[i] = nameGameObj;
                equipable[i] = isEquipable;
                objectSpritePath[i] = spriteName;

                gameObjReference = new GameObject(nameGameObj);
                gameObjReference.AddComponent<CanvasRenderer>();
                Image image = gameObjReference.AddComponent<Image>();
                gameObjReference.transform.parent = parent.transform;
                int sizeImage = (OptionMenu_Manager.currentWindowSize == 0) ? 322 : (OptionMenu_Manager.currentWindowSize == 1) ? 214 : 107;
                image.rectTransform.sizeDelta = new Vector2(sizeImage, sizeImage);
                image.overrideSprite = Resources.Load<Sprite>($"Sprites/{spriteName}");//NON AGGIUNGERE .png
                image.rectTransform.anchoredPosition = Inventory_Manager.selectionPosition[i];

                objectImage[i] = gameObjReference.GetComponent<Image>();
                gameObjRef[i] = gameObjReference;

                if (quantity > -1)
                {
                    //CREA OGGETTO CON COMPONETE TESTO CHE ABBIA COME PARENT IL gameObjReference
                    quantityTextName[i] = $"text_{nameGameObj}";
                    GameObject text_Quantity = new GameObject($"text_{nameGameObj}");
                    text_Quantity.AddComponent<TextMeshProUGUI>();
                    float sizeText = (OptionMenu_Manager.currentWindowSize == 0) ? 36 : (OptionMenu_Manager.currentWindowSize == 1) ? 24 : 12;
                    text_Quantity.GetComponent<TextMeshProUGUI>().fontSize = sizeText;
                    text_Quantity.GetComponent<TextMeshProUGUI>().text = quantity.ToString();
                    text_Quantity.transform.parent = gameObjReference.transform;

                    text_Quantity.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta = ((OptionMenu_Manager.currentWindowSize == 0) ? new Vector2(200, 50) : (OptionMenu_Manager.currentWindowSize == 1) ? new Vector2(133, 33) : new Vector2(66, 16));

                    Vector2 scalePosText = (OptionMenu_Manager.currentWindowSize == 0) ? new Vector2(quantTextPosX, quantTextPosY) : (OptionMenu_Manager.currentWindowSize == 1) ? ScalePosition(new Vector2(quantTextPosX, quantTextPosY), new Vector2(1920,1080), new Vector2 (1280,720)) : ScalePosition(new Vector2(quantTextPosX, quantTextPosY), new Vector2(1920, 1080), new Vector2(640, 360));
                    text_Quantity.GetComponent<TextMeshProUGUI>().rectTransform.anchoredPosition = scalePosText;

                    quantityTextMeshPos[i] = new Vector2(quantTextPosX, quantTextPosY);
                }

                //Debug.Log("è stato inserito");

                TextBoxObjectTake_Manager.ActivateTextBox(name, true);
                FMOD_Sound_Manager.PlayTakeObjSound();
                return true;
            }

            if (i >= myName.Length - 1)
            {
                Debug.Log("INVENTARIO PIENO");
                return false;
            }
        }
        return false;
    }
    #endregion

    #region Scale Position
    static Vector2 ScalePosition(Vector2 startPos, Vector2 baseScreenSize, Vector2 newScreenSize)
    {
        return new Vector2((startPos.x * newScreenSize.x) / baseScreenSize.x, (startPos.y * newScreenSize.y) / baseScreenSize.y);
    }
    #endregion

    #region Inventory Recreator
    public static void InventoryRecreate()
    {
        for (int i = 0; i < myName.Length; i++)
        {
            if (myName[i] != null && i < myName.Length - 1)
            {
                gameObjRef[i] = new GameObject(nameGameObject[i]);
                gameObjRef[i].AddComponent<CanvasRenderer>();
                Image image = gameObjRef[i].AddComponent<Image>();
                image.overrideSprite = Resources.Load<Sprite>($"Sprites/{objectSpritePath[i]}");
                gameObjRef[i].transform.parent = Inventory_Manager.InventoryObj.transform; 
                int sizeImage = (OptionMenu_Manager.currentWindowSize == 0) ? 322 : (OptionMenu_Manager.currentWindowSize == 1) ? 214 : 107;
                image.rectTransform.sizeDelta = new Vector2(sizeImage, sizeImage);
                image.rectTransform.anchoredPosition = Inventory_Manager.selectionPosition[i];
                objectImage[i] = image;

                if(quantityTextName[i] != null)
                {
                    textQuantObjRef[i] = new GameObject(quantityTextName[i]);
                    textQuantObjRef[i].AddComponent<CanvasRenderer>();
                    TextMeshProUGUI text = textQuantObjRef[i].AddComponent<TextMeshProUGUI>();
                    text.text = ReturnObjectText(nameGameObject[i]);
                    float sizeText = (OptionMenu_Manager.currentWindowSize == 0) ? 36 : (OptionMenu_Manager.currentWindowSize == 1) ? 24 : 12;
                    text.GetComponent<TextMeshProUGUI>().fontSize = sizeText;
                    text.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta = ((OptionMenu_Manager.currentWindowSize == 0) ? new Vector2(200, 50) : (OptionMenu_Manager.currentWindowSize == 1) ? new Vector2(133, 33) : new Vector2(66, 16));
                    textQuantObjRef[i].transform.parent = gameObjRef[i].transform;
                    Vector2 scalePosText = (OptionMenu_Manager.currentWindowSize == 0) ? quantityTextMeshPos[i] : (OptionMenu_Manager.currentWindowSize == 1) ? ScalePosition(quantityTextMeshPos[i], new Vector2(1920, 1080), new Vector2(1280, 720)) : ScalePosition(quantityTextMeshPos[i], new Vector2(1920, 1080), new Vector2(640, 360));
                    text.rectTransform.anchoredPosition = scalePosText;
                }
            }
        }
    }
    #endregion

    #region Return Object Text
    static string ReturnObjectText(string objectInInv)
    {
        switch (objectInInv)
        {
            case "Gun_Obj":
                return Shooter_Manager.gunAmmunition.ToString();

            case "Eagle_Obj":
                return Shooter_Manager.ammunitionBox.ToString();

            case "PieceOfCloth":
                return OpenSaveMenu_Manager.numberOfPOC.ToString();
        }

        return null;
    }
    #endregion

    #region Take Object
    private static void TakeObject(GameObject gameObj)
    {
        if (KeyBoardOrPadController.Key_Space && !GameMenu_Manager.menuIsActive)
        {
            if (ObjectIstantiator_Manager.ReturnObj())
            {
                //TESTO PRESO OGGETTO
                PlayerMovement.dialogTriggerEnter = true;
                Text_Creator.putText = "OGGETTO RACCOLTO";

                Destroy(gameObj);
            }
            else
            {
                //TESTO INVENTARIO PIENO
                PlayerMovement.dialogTriggerEnter = true;
                Text_Creator.putText = "INVENTARIO PIENO";
                FMOD_Sound_Manager.PlayNullSound();
            }
        }        
    }
    #endregion

    private void Update()
    {
        if (PlayerMovement.takebleObjTrigger)
        {
            TakeObject(PlayerMovement.objectCollide);
        }
    }

    #region Delete Object
    /// <summary>
    /// Cancella un ogetto nel numero di blocco inserito
    /// </summary>
    /// <param name="numberInInvenctory"></param>
    public static void DeleteObject(int numberInInvenctory)
    {
        myName[numberInInvenctory] = null;
        nameGameObject[numberInInvenctory] = null;
        objectImage[numberInInvenctory] = null;
        equipable[numberInInvenctory] = false;
        Destroy(gameObjRef[numberInInvenctory]);
        gameObjRef[numberInInvenctory] = null;
        objectSpritePath[numberInInvenctory] = null;

        quantityTextName[numberInInvenctory] = null;
        textQuantObjRef[numberInInvenctory] = null;
        objectSpritePath[numberInInvenctory] = null;
    }
    #endregion

    #region Search Obj And Delete
    /// <summary>
    /// Ricerca l'oggetto inserito (tramite nome) all'interno di tutti i blocchi dell'inventario e lo cancella
    /// </summary>
    /// <param name="nameObject"></param>
    public static void SearchObjAndDelete(string nameObject)
    {
        for (int i = 0; i < nameGameObject.Length; i++)
        {
            if (nameGameObject[i] == nameObject)
            {
                DeleteObject(i);
            }
        }

        ChangeObjPosition();
    }
    #endregion

    #region Change Obj Position
    /// <summary>
    /// Cambia la posizione degli oggetti nell'inventario non lasciando spazi vuoti
    /// </summary>
    static void ChangeObjPosition()
    {
        for (int i = 0; i < myName.Length; i++)
        {
            if(myName[i] == null && i+1 < myName.Length && myName[i+1] != null)
            {
                //Obj Override
                myName[i] = myName[i+1];
                nameGameObject[i] = nameGameObject[i + 1];
                objectImage[i] = objectImage[i+1];
                equipable[i] = equipable[i+1];
                gameObjRef[i] = gameObjRef[i+1];
                objectSpritePath[i] = objectSpritePath[i+1];

                quantityTextName[i] = quantityTextName[i+1];
                quantityTextMeshPos[i] = quantityTextMeshPos[i+1];
                textQuantObjRef[i] = textQuantObjRef[i+1];
                objectSpritePath[i] = objectSpritePath[i+1];

                //Obj delete
                myName[i+1] = null;
                nameGameObject[i + 1] = null;
                objectImage[i+1] = null;
                equipable[i+1] = false;
                //gameObjRef[i + 1].SetActive(false);
                Destroy(gameObjRef[i + 1]);
                gameObjRef[i+1] = null;
                objectSpritePath[i+1] = null;

                quantityTextName[i+1] = null;
                quantityTextMeshPos[i + 1] = Vector2.zero;
                textQuantObjRef[i+1] = null;
                objectSpritePath[i+1] = null;
            }
        }

        for (int i = 0; i < gameObjRef.Length; i++)
        {
            Destroy(gameObjRef[i]);
        }

        InventoryRecreate();
    }
    #endregion

    #region Search Obj
    public static bool SearchObj(string nameObject)
    {
        for (int i = 0; i < nameGameObject.Length; i++)
        {
            if (nameGameObject[i] == nameObject)
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Change Name Of Obj
    public static void ChangeNameOfObj(string OldNameGameObject, string newNameGameObject, string newNameObjectView)
    {
        for (int i = 0; i < nameGameObject.Length; i++)
        {
            if (nameGameObject[i] == OldNameGameObject)
            {
                nameGameObject[i] = newNameGameObject;
                myName[i] = newNameObjectView;
            }
        }
    }
    #endregion

    #region Name Object
    public static string NameObject(int numberInInvenctory)
    {
        return myName[numberInInvenctory];
    }
    #endregion

    #region 
    public static string ReturnMyNameFromNameGameObj(string nameObject)
    {
        for (int i = 0; i < nameGameObject.Length; i++)
        {
            if (nameGameObject[i] == nameObject)
            {
                return myName[i];
            }
        }

        return null;
    }
    #endregion

    #region Sprite Object
    public static Image SpriteObject(int numberInInvenctory)
    {
        return objectImage[numberInInvenctory];
    }
    #endregion

    #region Equip Object
    public static void EquipObject(Sprite objectToEquip)
    {
        showEquipObj.color = Color.white;
        objToEquip = objectToEquip;
    }
    #endregion

    #region Unequips Object
    public static void UnequipsObject()
    {
        if(objToEquip != null)
            objToEquip = null;

        if(showEquipObj != null)
        {
            showEquipObj.overrideSprite = null;
            showEquipObj.color = new Color32(45, 60, 90, 255);
        }
    }
    #endregion

}
