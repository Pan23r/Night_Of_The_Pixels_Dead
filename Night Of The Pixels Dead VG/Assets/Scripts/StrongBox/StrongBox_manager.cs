using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StrongBox_manager : MonoBehaviour
{
    public static string secretCode = "1975"; //OLD 1984
    static string[] numberCode = new string[4];
    public static bool setButton1 = false;
    static TextMeshProUGUI codeText;
    public static GameObject strongBox;
    public static bool verifyCode = false;
    public static bool canOpen = false;
    public static bool canEscape = true;
    public static bool StrongBoxIsOpen = false;
    float timerEscape = 0;
    float maxTimeEscape = 2f; // 2 sec.

    public static void RestoreAll()
    {
        verifyCode = false;
        canOpen = false;
        StrongBoxIsOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        codeText = gameObject.GetComponent<TextMeshProUGUI>();
        strongBox = GameObject.Find("StrongBox");
        strongBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!verifyCode)
            codeText.text = $"{ReturnTextCode(0)}{ReturnTextCode(1)}{ReturnTextCode(2)}{ReturnTextCode(3)}";

        PlayerMovement.canMove = false;

        if(KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && canEscape)
        {
            EscapeFromView();
        }

        //EscapeFromView After VerfiyCode
        if (!canEscape)
        {
            timerEscape += Time.deltaTime;

            if(timerEscape >= maxTimeEscape)
            {
                EscapeFromView();
                canEscape = true;
                timerEscape = 0;
            }
        }
    }

    void EscapeFromView()
    {
        for (int i = 0; i < numberCode.Length; i++)
        {
            numberCode[i] = null;
        }

        Timer_KeyPress_Manager.timer = 0;
        PlayerMovement.canMove = true;
        verifyCode = false;
        codeText.color = Color.white;
        strongBox.SetActive(false);
    }

    string ReturnTextCode(int i)
    {
        return (numberCode[i] != null) ? numberCode[i] : "";
    }

    public static void VerifyCode(string code)
    {
        string text =  (codeText.text == code) ?  "OPEN" : "ERROR";
        codeText.color = (text == "OPEN") ? Color.green : (text == "ERROR") ? Color.red : Color.white;
        StrongBoxIsOpen = (text == "OPEN") ? true : false;
        codeText.text = text;
        verifyCode = true;

        if (text == "OPEN")
            FMOD_Sound_Manager.PlayVerifyTrueSB();
        else
            FMOD_Sound_Manager.PlayVerifyFalseSB();

        //EscapeFromView (Ultima parte del update)
        canEscape = false;
    }

    public static void Canc()
    {
        for(int i = numberCode.Length - 1; i >= 0; i--)
        {
            if(numberCode[i] != null)
            {
                numberCode[i] = null;
                break;
            }
        }
    }

    public static void SetCode(string text)
    {
        for (int i = 0; i < numberCode.Length; i++)
        {
            if(numberCode[i] == null)
            {
                numberCode[i] = text;
                break;
            }
        }
    }

    void Open()
    {
        //PER APRIRE METODO NEL UI_GAME_MANAGER
    }
}
