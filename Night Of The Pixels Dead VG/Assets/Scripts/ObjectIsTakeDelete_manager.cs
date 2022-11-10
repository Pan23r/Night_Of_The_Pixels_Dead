using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIsTakeDelete_manager : MonoBehaviour
{
    public static bool gunIsTaken = false;
    public static bool keyLabIsTaken = false;
    public static bool keySallyIsTaken = false;
    public static bool[] munitionIsTaken = new bool[10]; //ultimo inserito _3
    public static bool FloppyIsTaken = false;
    public static bool MedallionIsTaken = false;
    public static bool pieceOfClothIsTaken = false;

    // Start is called before the first frame update
    void Start()
    {
        if(gunIsTaken)
            Destroy(GameObject.Find("Gun"));

        if(keyLabIsTaken)
            Destroy(GameObject.Find("Cassaforte"));

        if(keySallyIsTaken)
            Destroy(GameObject.Find("KeySally"));

        for (int i = 0; i < munitionIsTaken.Length; i++)
        {
            if (munitionIsTaken[i])
                Destroy(GameObject.Find($"EagleGold_{i}"));
        }

        if(FloppyIsTaken)
            Destroy(GameObject.Find("Floppy"));

        if (FloppyQuestTrigger_Manager.fileIsTaken)
            Destroy(GameObject.Find("ComputerLab"));

        if (FloppyQuestTrigger_Manager.musicSheetIsTaken)
            Destroy(GameObject.Find("ComputerGirl"));

        if (FloppyQuestTrigger_Manager.pianoPlayed)
            Destroy(GameObject.Find("Piano_Text"));
        
        if (MedallionIsTaken)
            Destroy(GameObject.Find("Medallion"));

        if (pieceOfClothIsTaken || ChoiceOfDifficultyMenu_Manager.easyMode)
            Destroy(GameObject.Find("PieceOfCloth"));
    }

    public static void RestoreAll()
    {
        gunIsTaken = false;
        keyLabIsTaken = false;
        keySallyIsTaken = false;

        for(int i = 0; i < munitionIsTaken.Length; i++)
            munitionIsTaken[i] = false;

        FloppyIsTaken = false;
        MedallionIsTaken = false;
        pieceOfClothIsTaken = false;
    }
}
