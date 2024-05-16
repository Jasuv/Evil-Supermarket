using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseEvents : MonoBehaviour
{
    ButcherScript butcher;

    public SupermarketEvents eventManager;
    public ChoreEvents choreEvents;
    public TextMessages textMessages;
    public CharacterController characterController;
    public CameraController cameraController;

    void Start()
    {
        eventManager = FindFirstObjectByType<SupermarketEvents>();
        choreEvents = FindFirstObjectByType<ChoreEvents>();
        textMessages = FindFirstObjectByType<TextMessages>();
        characterController = FindFirstObjectByType<CharacterController>();
        cameraController = FindFirstObjectByType<CameraController>();
        butcher = GameObject.FindGameObjectWithTag("Butcher").GetComponent<ButcherScript>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) butcher.StartCoroutine("prep");
        if (col.CompareTag("Player") && tag == "End")
        {
            if (eventManager != null) Destroy(eventManager.gameObject);
            if (choreEvents != null) Destroy(choreEvents.gameObject);
            if (textMessages != null) Destroy(textMessages.gameObject);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
