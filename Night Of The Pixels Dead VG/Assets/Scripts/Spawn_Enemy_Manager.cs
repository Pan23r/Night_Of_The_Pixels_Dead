using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy_Manager : MonoBehaviour
{
    public static bool[] SpawnZombie = new bool[6];
    public static bool setFirstSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        SetTrueAllZombies();
        
        for(int i = 0; i < SpawnZombie.Length; i++)
        {
            if (GameObject.Find($"Zombie_{i}") != null)
                GameObject.Find($"Zombie_{i}").SetActive(SpawnZombie[i]);

            if (i != 3 && GameObject.Find($"Zombie_{i}") != null && !Change_room_Manager.CellarIsOpen)
                GameObject.Find($"Zombie_{i}").SetActive(false);
        }
    }

    void SetTrueAllZombies()
    {
        if (setFirstSpawn)
        {
            for(int i = 0; i < SpawnZombie.Length; i++)
            {
                SpawnZombie[i] = true;
            }
            setFirstSpawn = false;
        }
    }

    public static void RestoreAll()
    {
        for (int i = 0; i < SpawnZombie.Length; i++)
        {
            SpawnZombie[i] = true;
        }
    }

    //PER DISATTTIVARE GLI ZOMBIE CONTROLLARE LA CLASSE Enemy_Manager
}
