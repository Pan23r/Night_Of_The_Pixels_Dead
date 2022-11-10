using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum QuestionOverload { yes, no}
public class QuestionOverload_Manager : MonoBehaviour
{
    public static GameObject question;
    public static bool isOpen = false;
    public static bool overrideSave = false;
    QuestionOverload selectAnswer;
    TextMeshProUGUI question_text;
    TextMeshProUGUI yes_text;
    TextMeshProUGUI no_text;

    // Start is called before the first frame update
    void Start()
    {
        selectAnswer = QuestionOverload.yes;
        question = GameObject.Find("OverloadSaveQuestion");
        question_text = GameObject.Find("OverloadSaveQuestion").GetComponent<TextMeshProUGUI>();
        yes_text = GameObject.Find("Yes_OverSave").GetComponent<TextMeshProUGUI>();
        no_text = GameObject.Find("No_OverSave").GetComponent<TextMeshProUGUI>();

        question.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            question.SetActive(false);
        }

        Controller();
    }

    void Controller()
    {
        if(KeyBoardOrPadController.Key_Right && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && selectAnswer == QuestionOverload.yes)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            selectAnswer = QuestionOverload.no;
        }
        else if (KeyBoardOrPadController.Key_Left && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && selectAnswer == QuestionOverload.no)
        {
            FMOD_Sound_Manager.PlayCursorSound();
            selectAnswer = QuestionOverload.yes;
        }

        switch (selectAnswer)
        {
            case QuestionOverload.yes:
                YesAnsware();
                break;

            case QuestionOverload.no:
                NoAnsware();
                break;
        }
    }

    void YesAnsware()
    {
        yes_text.color = Color.yellow;
        no_text.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            isOpen = false;
            overrideSave = true;
            question.SetActive(false);
            Timer_KeyPress_Manager.timer = 0;
        }
    }

    void NoAnsware()
    {
        no_text.color = Color.yellow;
        yes_text.color = Color.white;

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            FMOD_Sound_Manager.PlayBackSound();
            isOpen = false;
            question.SetActive(false);
            selectAnswer = QuestionOverload.yes;
            Timer_KeyPress_Manager.timer = 0;
        }
    }
}
