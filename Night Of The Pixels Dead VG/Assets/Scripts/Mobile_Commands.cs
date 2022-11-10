using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile_Commands : MonoBehaviour
{
    public FixedButton a_Button;
    public FixedButton b_Button;
    public FixedButton start_Button;
    public FixedButton l2_Button;
    public FixedButton r2_Button;

    public FixedButton dPad_Up_Button;
    public FixedButton dPad_Right_Button;
    public FixedButton dPad_Down_Button;
    public FixedButton dPad_Left_Button;


    // Start is called before the first frame update
    void Start()
    {
        if(SystemInfo.deviceType != DeviceType.Handheld)
        {
            gameObject.SetActive(false);
            //TODO: Riabilitare il SetActive
        }
    }

    public void DisableObj()
    {
        gameObject.SetActive(false);
    }

    public void EnableObj()
    {
        gameObject.SetActive(true);
    }
}
