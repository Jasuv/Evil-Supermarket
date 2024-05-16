using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SupermarketEvents : MonoBehaviour
{
    public ChoreEvents choreEvents;
    public TextMessages textManager;
    public int day;

    public GameObject basement_door;
    public GameObject speaker;

    public void Start()
    {
        basement_door = GameObject.FindGameObjectWithTag("Door");
        speaker = GameObject.FindGameObjectWithTag("Speaker");
        DontDestroyOnLoad(gameObject);
        choreEvents = FindFirstObjectByType<ChoreEvents>();
        textManager = FindFirstObjectByType<TextMessages>();

        basement_door.GetComponent<AudioSource>().enabled = false;
        
        choreEvents.Day1Chores();
    }

    public void Update()
    {
        basement_door = GameObject.FindGameObjectWithTag("Door");
        speaker = GameObject.FindGameObjectWithTag("Speaker");
        foreach (GameObject light in GameObject.FindGameObjectsWithTag("Flickering")) 
        {
            float chance = Random.Range(0.0f, 5.0f);
            if (chance < 0.01f) StartCoroutine("flicker", light);
        }
    }

    // light flicker
    IEnumerator flicker(GameObject light)
    {
        light.GetComponent<Light>().range = 4f;
        light.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        light.GetComponent<Light>().range = 6f;
        light.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    // eerie door sound
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            basement_door.GetComponent<AudioSource>().enabled = true;
            if (speaker != null)
                speaker.GetComponent<AudioSource>().enabled = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            basement_door.GetComponent<AudioSource>().enabled = false;
            if (speaker != null)
                speaker.GetComponent<AudioSource>().enabled = true;
        }
    }

    public void nextDay() {
        SceneManager.LoadScene("Day" + ++day);
        textManager.StartCoroutine("DayDisplay", day);
        textManager.seen = false;
        basement_door.GetComponent<AudioSource>().enabled = false;
        if (day == 2) 
        { 
            choreEvents.Day2Chores();
            textManager.seen = false;
        }
    }

    // end cutscene
    public void toEnd()
    {
        Application.Quit();
    }
}
