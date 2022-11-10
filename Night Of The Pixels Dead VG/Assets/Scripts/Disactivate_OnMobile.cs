using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disactivate_OnMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
