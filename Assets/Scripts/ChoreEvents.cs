using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoreEvents : MonoBehaviour
{
    public SupermarketEvents eventManager;
    public CameraController player;
    public bool end_day = false;

    private GameObject[] spots;
    public GameObject mess;

    public int obj_counter;
    public int cleaned;
    public bool printed;

    public bool chime;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        eventManager = FindObjectOfType<SupermarketEvents>();
        player = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        player = FindObjectOfType<CameraController>();
        if (cleaned > 2 && obj_counter > 5 && eventManager.day == 1) end_day = true;
        else if (printed && obj_counter > 5 && eventManager.day == 2) end_day = true;
        else end_day = false;

        if (end_day && !chime) 
        {
            chime = true;
            player.source.clip = player.sounds[3];
            player.source.Play();
        }
    }

    public void Day1Chores()
    {
        obj_counter = 0;
        cleaned = 0;
        chime = false;
        int i, r;
        GameObject tmp;
        // shuffle
        spots = GameObject.FindGameObjectsWithTag("Fluid");
        for (i = 0; i < spots.Length; i++)
        {
            tmp = spots[i];
            r = Random.Range(i, spots.Length);
            spots[i] = spots[r];
            spots[r] = tmp;
        }
        // spawn 3 mess
        for (i = 0; i < 3; i++)
            Instantiate(mess, spots[i].transform.position, spots[i].transform.rotation);
    }

    public void Day2Chores()
    { 
        obj_counter = 0;
        printed = false;
        chime = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "objective") obj_counter++;
    }
}
