using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeadScreen : MonoBehaviour
{
    Image WhiteScreen;
    TextMeshProUGUI textDead;
    GameObject BackGround;
    float color = 0f;
    const float scaleDeltaTime = 0.5f;
    bool scaleAlpha = false;
    bool playSound = false;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        WhiteScreen = GameObject.Find("DeadScreen_White").GetComponent<Image>();
        BackGround = GameObject.Find("DeadScreen_BackGround");
        textDead = GameObject.Find("GameOver_Text").GetComponent<TextMeshProUGUI>();

        WhiteScreen.color = new Color(WhiteScreen.color.r, WhiteScreen.color.g, WhiteScreen.color.b, 0);
        BackGround.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLife_Manager.life <= 0)
        {
            timer += Time.deltaTime;

            if (!playSound)
            {
                FMOD_Sound_Manager.Stop();
                FMOD_Sound_Manager.PlayDeadBarbaraSound();
                playSound = true;
            }

            if (!scaleAlpha)
            {
                color += Time.deltaTime * scaleDeltaTime;
                WhiteScreen.color = new Color(WhiteScreen.color.r, WhiteScreen.color.g, WhiteScreen.color.b, color);
                scaleAlpha = (WhiteScreen.color.a >= 1) ? true : false;
            }
            else if (scaleAlpha && color > 0)
            {
                BackGround.SetActive(true);
                color -= Time.deltaTime * scaleDeltaTime;
                WhiteScreen.color = new Color(WhiteScreen.color.r, WhiteScreen.color.g, WhiteScreen.color.b, color);
            }

            if(timer >= 5f)
            {
                textDead.CrossFadeAlpha(0f, 1.5f, false);
            }

            if(timer >= 10f)
            {
                //ReturnToMainMenu.RestoreAllVariables();
                SceneManager.LoadScene("Menu");
            }
        }        
    }
}
