using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextNote_Manager : Text_DirectionPlayer_Manager
{
    public static bool triggerIsActive = false;
    public static GameObject noteBox;
    static Image backGroundImage;
    static Image arrowLeft;
    static Image arrowRight;
    static TextMeshProUGUI nameNoteText;
    static TextMeshProUGUI textNote;
    protected static int page = 0;
    static string[] pageTextOfNote = new string[1];

    // Start is called before the first frame update
    void Start()
    {
        noteBox = GameObject.Find("NoteBox");
        backGroundImage = GameObject.Find("BackGround_Note").GetComponent<Image>();
        nameNoteText = GameObject.Find("Note_Name").GetComponent<TextMeshProUGUI>();
        textNote = GameObject.Find("Text_Note").GetComponent<TextMeshProUGUI>();
        arrowLeft = GameObject.Find("ArrowLeft").GetComponent<Image>();
        arrowRight = GameObject.Find("ArrowRight").GetComponent<Image>();

        arrowLeft.color = new Color(arrowLeft.color.r, arrowLeft.color.g, arrowLeft.color.b, 0.7f);
        arrowRight.color = new Color(arrowRight.color.r, arrowRight.color.g, arrowRight.color.b, 0.7f);
        backGroundImage.color = new Color(backGroundImage.color.r, backGroundImage.color.g, backGroundImage.color.b, 0.7f);
        noteBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !GameMenu_Manager.menuIsActive && triggerIsActive && PlayerDirectionText())
        {
            if (!noteBox.activeSelf)
            {
                FMOD_Sound_Manager.PlayNoteOpenAndCloseSound();
                noteBox.SetActive(true);
                PlayerMovement.canMove = false;
                Enemy_Manager.canMove = false;
                Enemy_Manager_NavMesh.canMove = false;
            }
            else if(noteBox.activeSelf && page == pageTextOfNote.Length -1)
            {
                FMOD_Sound_Manager.PlayNoteOpenAndCloseSound();
                page = 0;
                noteBox.SetActive(false);
                PlayerMovement.canMove = true;
                Enemy_Manager.canMove = true;
                Enemy_Manager_NavMesh.canMove = true;
            }

            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !GameMenu_Manager.menuIsActive && triggerIsActive && noteBox.activeSelf)
        {
            FMOD_Sound_Manager.PlayNoteOpenAndCloseSound();
            page = 0;
            noteBox.SetActive(false);
            PlayerMovement.canMove = true;
            Enemy_Manager.canMove = true;
            Enemy_Manager_NavMesh.canMove = true;
            Timer_KeyPress_Manager.timer = 0;
        }
        else if(KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !GameMenu_Manager.menuIsActive && triggerIsActive && noteBox.activeSelf && page < pageTextOfNote.Length-1)
        {
            FMOD_Sound_Manager.PlayNoteSound();
            page += 1;
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !GameMenu_Manager.menuIsActive && triggerIsActive && noteBox.activeSelf && page > 0)
        {
            FMOD_Sound_Manager.PlayNoteSound();
            page -= 1;
            Timer_KeyPress_Manager.timer = 0;
        }

        if (page == 0)
        {
            arrowLeft.gameObject.SetActive(false);
            arrowRight.gameObject.SetActive((pageTextOfNote.Length == 1)? false : true);
        }
        else if (page == pageTextOfNote.Length -1)
        {
            arrowLeft.gameObject.SetActive(true);
            arrowRight.gameObject.SetActive(false);
        }
        else
        {
            arrowLeft.gameObject.SetActive(true);
            arrowRight.gameObject.SetActive(true);
        }

        textNote.text = pageTextOfNote[page];
    }

    public static bool ReturneNote(Collider2D collider)
    {
        switch (collider.name)
        {
            case "DeadBodySoldier":
                InstantiateNote(Translation_Manager.reportTitle, 7);
                pageTextOfNote[0] = Translation_Manager.reportPage1;
                pageTextOfNote[1] = Translation_Manager.reportPage2; 
                pageTextOfNote[2] = Translation_Manager.reportPage3;
                pageTextOfNote[3] = Translation_Manager.reportPage4; 
                pageTextOfNote[4] = Translation_Manager.reportPage5; 
                pageTextOfNote[5] = Translation_Manager.reportPage6; 
                pageTextOfNote[6] = Translation_Manager.reportPage7; 
                return true;

            case "Child_Note":
                InstantiateNote(Translation_Manager.sallyNoteTitle, 1);
                pageTextOfNote[0] = Translation_Manager.sallyNote;
                return true;

            case "SchoolHomework":
                InstantiateNote(Translation_Manager.sallyHomeworkTitle, 3);
                pageTextOfNote[0] = Translation_Manager.sallyHomeworkPage1;
                pageTextOfNote[1] = Translation_Manager.sallyHomeworkPage2;
                return true;
        }

        return false;
    }

    static void InstantiateNote(string nameNote, int arrayLenght)
    {
        nameNoteText.text = nameNote;
        pageTextOfNote = new string[arrayLenght];
    }
}
