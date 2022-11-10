using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGame_Manager : MonoBehaviour
{
    TextMeshProUGUI storyText;
    TextMeshProUGUI credits;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        storyText = GameObject.Find("EndGameStory").GetComponent<TextMeshProUGUI>();
        credits = GameObject.Find("Credits").GetComponent<TextMeshProUGUI>();
        storyText.CrossFadeAlpha(0,0,false);
        credits.CrossFadeAlpha(0, 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 50)
        {
            SceneManager.LoadScene("Menu");
        }
        else if (timer >= 40)
        {
            credits.CrossFadeAlpha(0, 1, true);
        }
        else if (timer >= 30)
        {
            credits.CrossFadeAlpha(1, 2, true);
        }
        else if (timer >= 25)
        {
            storyText.CrossFadeAlpha(0, 1, true);
        }
        else if (timer >= 1)
        {
            storyText.CrossFadeAlpha(1, 2, true);
        }

        timer += Time.deltaTime;
    }
}
