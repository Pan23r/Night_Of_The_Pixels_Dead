using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloppyQuestTrigger_Manager : MonoBehaviour
{
    public static bool fileIsTaken = false;
    public static bool enterTrigger = false;
    public static bool firePlaceIsOpen = false;
    public static bool pianoPlayed = false;
    public static bool musicSheetIsTaken = false;

    public static bool useFloppy = false;
    public static bool useMusicSheet = false;

    float timer = 0;
    float timeToRecove = 3f; //Tempo per riprendere l'oggetto messo
    float timeToPlayPiano = 27f; //Tempo per riprodurre suono piano
    static string ObjectCollideName;
    GameObject refObjMusicSheet;
    bool SoundIsPlayed = false;
    public static bool dontOpenGameMenu = false;

    public static void RestoreAll()
    {
        fileIsTaken = false;
        enterTrigger = false;
        firePlaceIsOpen = false;
        pianoPlayed = false;
        musicSheetIsTaken = false;

        useFloppy = false;
        useMusicSheet = false;

        dontOpenGameMenu = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        refObjMusicSheet = new GameObject("Spartito");
    }

    // Update is called once per frame
    void Update()
    {
        if (enterTrigger && !GameMenu_Manager.menuIsActive)
        {
           if(gameObject.name == "ComputerLab" && !fileIsTaken && useFloppy)
           {
                PlayerMovement.canMove = false;
                if(timer > timeToRecove)
                {
                    ObjectInInventory_Manager.PutObject(Translation_Manager.nameFloppyWithMusicSheet, "FloppyDiskWithMusicSheet_Obj","Floppy_Disk", false, ref PlayerMovement.objectCollide, Inventory_Manager.InventoryObj);
                    fileIsTaken = true;
                    useFloppy = false;
                    PlayerMovement.canMove = true;
                    Destroy(GameObject.Find("ComputerLab"));
                    timer = 0;
                }
                timer += Time.deltaTime;
           }
           else if (gameObject.name == "ComputerGirl" && fileIsTaken && useFloppy)
           {
               PlayerMovement.canMove = false;
               if (timer > timeToRecove)
               {
                   ObjectInInventory_Manager.PutObject(Translation_Manager.nameMusicSheet, "MusicSheet_Obj","Spartito", false, ref refObjMusicSheet, Inventory_Manager.InventoryObj);
                   musicSheetIsTaken = true;
                   useFloppy = false;
                   PlayerMovement.canMove = true;
                    Destroy(GameObject.Find("ComputerGirl"));
                    timer = 0;
               }
               timer += Time.deltaTime;
           }
           else if (ObjectCollideName == "Piano_Text" && useMusicSheet)
           {
                if (!SoundIsPlayed)
                {
                    FMOD_Sound_Manager.InGame(9);
                    PlayerMovement.ChangeAnimAndIdle("PlayerWalkBack", "Back");
                    SoundIsPlayed = true;
                    PlayerMovement.questionMarkCollider.SetActive(false);
                }

                PlayerMovement.canMove = false;
                if (timer > timeToPlayPiano)
                {
                    FMOD_Sound_Manager.InGame(2);
                    dontOpenGameMenu = false;
                    useMusicSheet = false;
                    pianoPlayed = true;
                    firePlaceIsOpen = true;
                    PlayerMovement.canMove = true;
                    timer = 0;
                    Destroy(GameObject.Find("Piano_Text"));
                }
                timer += Time.deltaTime;
           }

           //PER USARE IL CAMINO GUARDARE Change_room_Manager
        }
    }

    public static bool CollideWithObj (Collider2D collision)
    {
        ObjectCollideName = collision.gameObject.name;
        switch (collision.gameObject.name)
        {
            case "ComputerLab":
                return true;

            case "ComputerGirl":
                return true;

            case "Piano_Text":
                return true;

            case "Camino_Text":
                return true;
        }
        return false;
    }
}
