using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TyperWriterEffect : MonoBehaviour
{
    [SerializeField] private float tyerwriterSpeed = 50f;
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {

        //yield return new WaitForSeconds(2);
        textLabel.text = string.Empty;
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * tyerwriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;

        }
        textLabel.text = textToType;
    }





}