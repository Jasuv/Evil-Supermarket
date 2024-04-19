using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SupermarketEvents : MonoBehaviour
{
    public string supermarket;
    public string basement;

    public GameObject[] flicker_lights;
    public GameObject basement_door;
    public GameObject speaker;

    public void Start()
    {
        basement_door.GetComponent<AudioSource>().enabled = false;
    }

    public void Update()
    {
        foreach (GameObject light in flicker_lights) 
        {
            float chance = Random.Range(0.0f, 5.0f);
            if (chance < 0.01f) StartCoroutine("flicker", light);
        }
    }

    IEnumerator flicker(GameObject light)
    {
        light.GetComponent<Light>().range = 4f;
        light.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        light.GetComponent<Light>().range = 6f;
        light.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            basement_door.GetComponent<AudioSource>().enabled = true;
            speaker.GetComponent<AudioSource>().enabled = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            basement_door.GetComponent<AudioSource>().enabled = false;
            speaker.GetComponent<AudioSource>().enabled = true;
        }
    }

    public void toBasement() {
        SceneManager.LoadScene(basement);
    }

    public void toEnd()
    {
        Application.Quit();
    }
}
