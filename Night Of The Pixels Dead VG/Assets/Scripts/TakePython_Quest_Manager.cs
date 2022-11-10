using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePython_Quest_Manager : MonoBehaviour
{
    public static bool canTakeColt = false;
    public static bool coltIsTaken = false;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canTakeColt)
        {
            timer += Time.deltaTime;
            
            if(PlayerMovement.ObjectCollideName == "Tomba_Text" && timer >= 3 && !coltIsTaken && KeyBoardOrPadController.Key_Space && Timer_KeyPress_Manager.timer >= Timer_KeyPress_Manager.timerCharge && !GameMenu_Manager.menuIsActive)
            {
                coltIsTaken = true;
                ObjectInInventory_Manager.PutObject(Translation_Manager.nameMagnum, "Magnum_Obj", "ColtPython", true, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj, Shooter_Manager.magnumAmmunition, -10, -110);
                FMOD_Sound_Manager.PlayTakeObjSound();
                GameObject.Find("Tomba_Text").SetActive(false);
            }
        }
    }

    public static void RestoreAll()
    {
        canTakeColt = false;
        coltIsTaken = false;
    }
}
