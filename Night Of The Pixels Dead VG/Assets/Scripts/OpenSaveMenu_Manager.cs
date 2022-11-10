using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OpenSaveMenu_Manager : MonoBehaviour
{
    bool usedPiece = false;
    public static int numberOfPOC = 2;
    GameObject questionUsePoc;
    TextMeshProUGUI questionUsePocText;
    TextMeshProUGUI yesPocText;
    TextMeshProUGUI noPocText;

    static bool questIsOpen = false;
    bool yesSelect = true;

    // Start is called before the first frame update
    void Start()
    {
        questionUsePoc = GameObject.Find("questionUsePoc");
        questionUsePocText = questionUsePoc.GetComponent<TextMeshProUGUI>();
        yesPocText = GameObject.Find("Yes_POC").GetComponent<TextMeshProUGUI>();
        noPocText = GameObject.Find("No_POC").GetComponent<TextMeshProUGUI>();
        yesPocText.color = Color.yellow;
        questionUsePoc.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!questIsOpen && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !ChoiceOfDifficultyMenu_Manager.easyMode && ObjectInInventory_Manager.SearchObj("PieceOfCloth_Obj") && !Save_Manager.isOpen && Save_Manager.canOpenSave)
        {
            questIsOpen = true;
            PlayerMovement.canMove = false;
            Timer_KeyPress_Manager.timer = 0;
        }
        
        /*else if(!questIsOpen && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !ChoiceOfDifficultyMenu_Manager.easyMode && !ObjectInInventory_Manager.SearchObj("PieceOfCloth_Obj") && !Save_Manager.isOpen && Save_Manager.canOpenSave)
        {
            //ATTIVA TESTO
            //Debug.Log("NON HAI OGGETTO");
            
            Timer_KeyPress_Manager.timer = 0;
        }*/

        if (questIsOpen)
            AnswarsUsePOC();


        if (ChoiceOfDifficultyMenu_Manager.easyMode && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !Save_Manager.isOpen && Save_Manager.canOpenSave || usedPiece)
        {
            FMOD_Sound_Manager.PlayDecisionInvenctorySound();
            Save_Manager.isOpen = true;
            PlayerMovement.canMove = false;
            Save_Manager.saveMenu.SetActive(true);
            Timer_KeyPress_Manager.timer = 0;
            usedPiece = false;
        }
    }

    void AnswarsUsePOC()
    {
        questionUsePoc.SetActive(true);

        if (KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            yesSelect = false;
            yesPocText.color = Color.white;
            noPocText.color = Color.yellow;
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            yesSelect = true;
            noPocText.color = Color.white;
            yesPocText.color = Color.yellow;
            Timer_KeyPress_Manager.timer = 0;
        }

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            if (yesSelect)
            {
                usedPiece = true;
                numberOfPOC -= 1;

                if (numberOfPOC <= 0)
                    ObjectInInventory_Manager.SearchObjAndDelete("PieceOfCloth_Obj");
            }
            else
            {
                yesSelect = true;
                noPocText.color = Color.white;
                yesPocText.color = Color.yellow;
                PlayerMovement.canMove = true;
            }
            questIsOpen = false;
            questionUsePoc.SetActive(false);
            Timer_KeyPress_Manager.timer = 0;
        }
    }

    public static bool CreateTextNoPOC()
    {
        return (!questIsOpen && !ChoiceOfDifficultyMenu_Manager.easyMode && !ObjectInInventory_Manager.SearchObj("PieceOfCloth_Obj") && !Save_Manager.isOpen);        
    }
}
