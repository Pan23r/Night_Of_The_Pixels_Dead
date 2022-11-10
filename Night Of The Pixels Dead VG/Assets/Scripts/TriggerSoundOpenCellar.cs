using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSoundOpenCellar : MonoBehaviour
{
    public static bool triggerTextOpenCellar = false;
    public static bool trigger2CellarOpen = true;

    public static void RestoreAll()
    {
        triggerTextOpenCellar = false;
        trigger2CellarOpen = true;
    }
    
    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigger2CellarOpen && collision.tag == "Player" && triggerTextOpenCellar)
        {
            FMOD_Sound_Manager.PlayCellarOpenSound();
            Change_room_Manager.CellarIsOpen = true;
            trigger2CellarOpen = false;
        }
    }
}
