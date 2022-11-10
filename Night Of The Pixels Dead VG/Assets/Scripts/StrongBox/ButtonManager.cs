using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public GameObject rightButtton;
    public GameObject leftButton;
    public GameObject upButton;
    public GameObject downButton;

    TextMeshProUGUI buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = gameObject.GetComponent<TextMeshProUGUI>();

        if(gameObject.name != "Button_1")
        {
            gameObject.GetComponent<ButtonManager>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        buttonText.color = Color.yellow;

        //BUTTON RIGHT
        SetButton(KeyBoardOrPadController.Key_Right, rightButtton);
        //BUTTON LEFT
        SetButton(KeyBoardOrPadController.Key_Left, leftButton);
        //BUTTON UP
        SetButton(KeyBoardOrPadController.Key_Up, upButton);
        //BUTTON DOWN
        SetButton(KeyBoardOrPadController.Key_Down, downButton);

        if(StrongBox_manager.setButton1 && gameObject.name != "Button_1")
        {
            gameObject.GetComponent<ButtonManager>().enabled = false;
            GameObject.Find("Button_1").GetComponent<ButtonManager>().enabled = true;
        }

        if (KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !StrongBox_manager.verifyCode)
        {
            switch (gameObject.name)
            {
                case "Button_Verify":
                    if (!StrongBox_manager.verifyCode)
                    {
                        StrongBox_manager.VerifyCode(StrongBox_manager.secretCode);
                        Timer_KeyPress_Manager.timer = 0;
                    }
                    break;

                case "Button_Canc":
                    StrongBox_manager.Canc();
                    Timer_KeyPress_Manager.timer = 0;
                    break;

                default:
                    StrongBox_manager.SetCode(buttonText.text);
                    Timer_KeyPress_Manager.timer = 0;
                    break;
            }
            FMOD_Sound_Manager.PlaySlectButtonSB();
        }
    }

    void SetButton(bool keyPress, GameObject buttonPress)
    {
        if (keyPress && buttonPress != null && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !StrongBox_manager.verifyCode)
        {
            buttonText.color = Color.white;

            buttonPress.GetComponent<ButtonManager>().enabled = true;
            Timer_KeyPress_Manager.timer = 0;
            gameObject.GetComponent<ButtonManager>().enabled = false;
            FMOD_Sound_Manager.PlayCursorButtonSB();
        }
    }
}
