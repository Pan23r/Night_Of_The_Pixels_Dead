using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum ChoiceDifficulty { choice1, choice2 }
public class ChoiceOfDifficultyMenu_Manager : MonoBehaviour
{
    TextMeshProUGUI choice1;
    TextMeshProUGUI choice2;
    ChoiceDifficulty myChoice;
    bool playPress = false;
    Image blackScreen;
    public static bool easyMode = false;

    // Start is called before the first frame update
    void Start()
    {
        choice1 = GameObject.Find("ChoiceHardMod").GetComponent<TextMeshProUGUI>();
        choice2 = GameObject.Find("ChoiceEasyMod").GetComponent<TextMeshProUGUI>();
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        myChoice = ChoiceDifficulty.choice1;

        blackScreen.CrossFadeAlpha(0f, 1f, false);
    }

    // Update is called once per frame
    void Update()
    {
        ControllerMenu();

        switch (myChoice)
        {
            case ChoiceDifficulty.choice1:
                choice1.color = Color.yellow;
                choice2.color = Color.white;
                break;

            case ChoiceDifficulty.choice2:
                choice1.color = Color.white;
                choice2.color = Color.yellow;
                break;
        }   
    }

    void ControllerMenu()
    {
        if (KeyBoardOrPadController.Key_Down && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !playPress)
        {
            myChoice = ChoiceDifficulty.choice2;
            FMOD_Sound_Manager.PlayCursorSound();
            Timer_KeyPress_Manager.timer = 0;
        }
        else if (KeyBoardOrPadController.Key_Up && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !playPress)
        {
            myChoice = ChoiceDifficulty.choice1;
            FMOD_Sound_Manager.PlayCursorSound();
            Timer_KeyPress_Manager.timer = 0;
        }

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !playPress)
        {
            FMOD_Sound_Manager.PlayDecisionSound();
            switch (myChoice)
            {
                case ChoiceDifficulty.choice1:
                    easyMode = false;
                    break;

                case ChoiceDifficulty.choice2:
                    easyMode = true;
                    break;
            }

            playPress = true;
            Timer_KeyPress_Manager.timer = 0;
        }

        if (playPress)
        {
            blackScreen.CrossFadeAlpha(1f, 1f, false);

            if (Timer_KeyPress_Manager.timer >= 4f)
                SceneManager.LoadScene("Cucina");
        }
    }
}
