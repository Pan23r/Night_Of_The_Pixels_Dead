using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_KeyPress_Manager : MonoBehaviour
{
    public static float timer;
    public const float timerCharge = 0.3f;//tempo di ricarica prima di poter premere nuovamente un tasto
    public static float switchTimer = 2f;//tempo passato dall'ultimo switch
    public const float timeChargeForMusic = 2f; //Tempo di ricarica per passare da una scena all'altra per far sfumare la musica

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switchTimer += Time.deltaTime;
    }
}
