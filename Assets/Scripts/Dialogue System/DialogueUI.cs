using UnityEngine;
using TMPro;
using System.Collections;
using JetBrains.Annotations;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDiagule;
    [SerializeField] private GameObject dialogueBox;

    private TyperWriterEffect typerWriterEffect;
    private ResponseHandler responseHandler;

    private void Start()
    {
        typerWriterEffect = GetComponent<TyperWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
        ShowDialogue(testDiagule);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }


    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        //yield return new WaitForSeconds(2);

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typerWriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break; //dialogueobject 'HasResponses'

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));               //hit 'space' bar to get to next dialogue

        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else
        {
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }









}