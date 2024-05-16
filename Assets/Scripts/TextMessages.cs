using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextMessages : MonoBehaviour
{
    public RawImage msgbox;
    public RawImage newspaper;
    public Text day;
    public ChoreEvents choreEvents;
    public SupermarketEvents gameEvents;

    public Texture task1;
    public Texture task2;

    private Animator anim;

    private bool wait = false;
    public bool seen = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        choreEvents = FindFirstObjectByType<ChoreEvents>();
        gameEvents = FindFirstObjectByType<SupermarketEvents>();
        anim = transform.GetChild(1).GetComponent<Animator>();
        day.enabled = false;
        msgbox.enabled = false;
        newspaper.enabled = false;
        StartCoroutine("DayDisplay", gameEvents.day);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && wait == false && seen == true)
        {
            if (gameEvents.day < 3)
            {
                StartCoroutine("Tasks");
            }
            else
            {
                StartCoroutine("Newspaper");
            }
        }
    }

    IEnumerator DayDisplay(int num) {
        yield return new WaitForSeconds(1);
        day.enabled = true;
        day.GetComponent<Animator>().Play("in");
        day.rectTransform.localScale = new Vector3(0.7f, 0.7f, 1);
        day.rectTransform.localScale = new Vector3(0.7f, 0.7f, 1);
        day.text = "Day " + num.ToString();
        yield return new WaitForSeconds(4);
        day.GetComponent<Animator>().Play("fade");
        yield return new WaitForSeconds(0.5f);
        day.enabled = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && seen == false) 
        { 
            if (gameEvents.day < 3) 
            { 
                StartCoroutine("Tasks"); 
            } 
            else 
            { 
                StartCoroutine("Newspaper"); 
            } 
        }
    }

    IEnumerator Tasks() 
    {
        wait = true;
        seen = true;
        msgbox.enabled = true;
        msgbox.texture = (gameEvents.day == 1) ? task1 : task2;
        anim.Play("open_note");
        yield return new WaitForSeconds(10);
        anim.Play("close_note");
        yield return new WaitForSeconds(0.5f);
        msgbox.enabled = false;
        wait = false;
    }

    IEnumerator Newspaper()
    {
        wait = true;
        seen = true;
        newspaper.enabled = true;
        newspaper.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(17);
        wait = false;
    }
}
