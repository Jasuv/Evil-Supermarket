using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;

    private List<GameObject> tempResponseButton = new List<GameObject>();

    public void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }
    public void ShowResponses(Response[] responses)
    {
        float responseBoxHeight = 0;

        // Unlock and show the cursor when showing responses
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            tempResponseButton.Add(responseButton);

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;

        }
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    public void OnPickedResponse(Response response)
    {

        // Lock and hide the cursor when responses are picked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButton)
        {
            Destroy(button);
        }
        tempResponseButton.Clear();

        dialogueUI.ShowDialogue(response.DialogueObject);

        //not nesscessary
        Debug.Log("Picked response: " + response.ResponseText);

    }








}