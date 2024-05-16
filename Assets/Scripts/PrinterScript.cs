using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterScript : MonoBehaviour
{
    public GameObject paper1;
    public GameObject paper2;
    private ChoreEvents choreEvents;

    void Start()
    {
        choreEvents = FindFirstObjectByType<ChoreEvents>();
    }

    public IEnumerator print()
    {
        choreEvents.printed = true;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2);
        paper1.GetComponent<Animator>().Play("print1");
        for (int i = 0; i < 7; i++) 
        {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(2);
            paper2.GetComponent<Animator>().Play("print2");
        }
        yield return null;
    }
}
