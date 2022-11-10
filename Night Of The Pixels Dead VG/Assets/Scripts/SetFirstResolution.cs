using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFirstResolution : MonoBehaviour
{
    Vector2[] resolutions = new Vector2[3];
    GameObject resolutionWarning;
    public static bool WarningIsOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        resolutions[0] = new Vector2(1920, 1080);
        resolutions[1] = new Vector2(1280, 720);
        resolutions[2] = new Vector2(640, 360);

        resolutionWarning = GameObject.Find("ResolutionNotSupport");
        resolutionWarning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemInfo.deviceType != DeviceType.Handheld && ReturnScreenRes() && !OptionMenu_Manager.isOpen)
        {
            Screen.SetResolution((int)resolutions[2].x, (int)resolutions[2].y, false);
            WarningIsOpen = true;
            resolutionWarning.SetActive(true);
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld && ReturnScreenRes() && !OptionMenu_Manager.isOpen)
        {
            Screen.SetResolution((int)SaveOptions_Manager.ReturnResolutionSaved(resolutions[2]).x, (int)SaveOptions_Manager.ReturnResolutionSaved(resolutions[2]).y, true);
            OptionMenu_Manager.RecreateOption();
        }

        if(WarningIsOpen && KeyBoardOrPadController.Key_Enter && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge ||
            WarningIsOpen && KeyBoardOrPadController.Key_Esc && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge)
        {
            WarningIsOpen = false;
            resolutionWarning.SetActive(false);
            Timer_KeyPress_Manager.timer = 0;
            OptionMenu_Manager.RecreateOption();
        }
    }

    bool ReturnScreenRes()
    {
        Vector2 screenRes = new Vector2(Screen.width, Screen.height);
        
        for(int i = 0; i < resolutions.Length; i++)
        {
            if (screenRes == resolutions[i])
                return false;
        }
        
        return true;
    }
}
