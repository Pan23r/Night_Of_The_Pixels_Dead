using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class VideoManager : MonoBehaviour
{
    public float timeOfSource;
    VideoPlayer video;
    GameObject backGround;
    GameObject subTitleObj;
    TextMeshProUGUI videoSubtitle;
    public static bool videoIsPlaying = false;
    public static bool IntroVideoIsPlayed = false;
    public static bool deadBodyVideoIsPlayed = false;
    int currentSubtitle = 0;

    public static void RestoreAll()
    {
        IntroVideoIsPlayed = false;
        deadBodyVideoIsPlayed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Timer_KeyPress_Manager.timer = 0; 
        currentSubtitle = 0;

        if (SceneManager.GetActiveScene().name == "Piano2")
            video = GameObject.Find("DeadBodyVideo").GetComponent<VideoPlayer>();

        if (SceneManager.GetActiveScene().name == "Cucina")
            video = GameObject.Find("IntroVideo").GetComponent<VideoPlayer>();

        backGround = GameObject.Find("VideoBackGround");
        subTitleObj = GameObject.Find("VideoSuBTitle");
        videoSubtitle = subTitleObj.GetComponent<TextMeshProUGUI>();
        //videoSubtitle.CrossFadeAlpha(0f, 0f, false);
        videoSubtitle.alpha = 0f;
        videoSubtitle.outlineColor = Color.black;
        videoSubtitle.outlineWidth = 0.3f;
        subTitleObj.SetActive(false);
        backGround.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Piano2" && !deadBodyVideoIsPlayed)
        {
            PlayVideo();
            FMOD_Sound_Manager.InGame(10);
            deadBodyVideoIsPlayed = true;
        }
        else if (SceneManager.GetActiveScene().name == "Cucina" && !IntroVideoIsPlayed)
        {
            PlayVideo();
            subTitleObj.SetActive(true);
            IntroVideoIsPlayed = true;
            FMOD_Sound_Manager.InGame(1);
        }

        IntroSubtitles();

        if (video != null && video.isPlaying && KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            StopVideo();
            Timer_KeyPress_Manager.timer = 0;
        }

        if (video != null && video.time >= timeOfSource)
        {
            StopVideo();
        }
    }

    void StopVideo()
    {
        backGround.SetActive(false);
        PlayerMovement.canMove = true;
        UI_Game_Manager.uiCanOpen = true;
        Change_room_Manager.canChange = true;
        Text_Creator.canActive = true;
        videoIsPlaying = false;
        video.Stop();

        if (SceneManager.GetActiveScene().name == "Cucina")
        {
            FMOD_Sound_Manager.InGame(3);
            subTitleObj.SetActive(false);
        }

        if (SceneManager.GetActiveScene().name == "Piano2")
            FMOD_Sound_Manager.InGame(2);
    }

    void PlayVideo()
    {
        backGround.SetActive(true);
        PlayerMovement.canMove = false;
        Change_room_Manager.canChange = false;
        Text_Creator.canActive = false;
        videoIsPlaying = true;
        video.Play();
    }

    void CreateSubTitles(Vector3 position, float timeMinToSetAlphaOne, float timeMaxToSetAlphaOne, float timeTransitionIn, string text, float timeWait ,float timeTransitionOut)
    {
        videoSubtitle.rectTransform.localPosition = position;
        if(Timer_KeyPress_Manager.timer >= timeMinToSetAlphaOne && Timer_KeyPress_Manager.timer <= timeMaxToSetAlphaOne)
        {
            //videoSubtitle.CrossFadeAlpha(1f, timeTransitionIn, false); 
            videoSubtitle.alpha += (1f / timeTransitionIn) * Time.deltaTime;
        }

        videoSubtitle.text = text;

        if (Timer_KeyPress_Manager.timer >= timeWait)
        {
            //videoSubtitle.CrossFadeAlpha(0f, timeTransitionOut, false);

            if(videoSubtitle.alpha > 0)
            {
                videoSubtitle.alpha -= (1f / timeTransitionOut) * Time.deltaTime;
            }

            if (videoSubtitle.alpha <= 0f)
            {
                currentSubtitle += 1;
            }
        }
    }

    void IntroSubtitles()
    {
        if ( SceneManager.GetActiveScene().name == "Cucina")
        {
            switch (currentSubtitle)
            {
                case 0:
                    videoSubtitle.rectTransform.sizeDelta = new Vector2(1200f, 961.8181f);
                    CreateSubTitles(new Vector3(370f, -400f, subTitleObj.transform.position.z), 0.8f, 2f, 1f, Translation_Manager.introTitle1, 4f, 1f);
                    break;

                case 1:
                    videoSubtitle.rectTransform.sizeDelta = new Vector2(1049.3f, 961.8181f);
                    CreateSubTitles(new Vector3(420f, -110f, subTitleObj.transform.position.z), 6f, 12f, 1f, Translation_Manager.introTitle2, 20f, 1f);
                    break;

                case 2:
                    CreateSubTitles(new Vector3(370f, -209, subTitleObj.transform.position.z), 22f, 30f, 1f, Translation_Manager.introTitle3, 30f, 1f);
                    break;
            }
        }
    }
}
