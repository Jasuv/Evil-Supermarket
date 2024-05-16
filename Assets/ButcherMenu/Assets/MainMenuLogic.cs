using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// @author: NightFury2415

public class MainMenuLogic : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject credits;
    public AudioSource buttonSound;
    
    void Start()
    {
        mainMenu = GameObject.Find("Main menu screen");
        credits = GameObject.Find("Credits screen");
        mainMenu.GetComponent<Canvas>().enabled = true;
        credits.GetComponent<Canvas>().enabled = false;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Day1");
    }

    public void CreditsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        credits.GetComponent<Canvas>().enabled = true;
        credits.transform.GetChild(1).GetComponent<Animation>().Play();
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        credits.GetComponent<Canvas>().enabled = false;
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }
 }