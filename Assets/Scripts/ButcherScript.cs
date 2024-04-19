using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ButcherScript : MonoBehaviour
{
    public bool shakeTime = true;
    [SerializeField] float max;
    Light spot;
    Animator shake;
    NavMeshAgent agent;
    Transform player;
    Collider trigger;
    private bool chase = false;
    public string lose;

    void Start()
    {
        trigger = GetComponent<Collider>();
        trigger.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shake = Camera.main.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spot = GetComponent<Light>();
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        transform.GetChild(0).transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, transform.up);
        if (chase) agent.SetDestination(player.position);
    }

    public IEnumerator prep()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = true;
        trigger.enabled = true;
        StartCoroutine("approach");
        StartCoroutine("shaker");
        StopCoroutine("prep");
        yield return null;
    }

    IEnumerator approach()
    {
        spot.range = 10;
        yield return new WaitForSeconds(2);
        chase = true;
        while (spot.range < max)
        {
            yield return new WaitForSeconds(0.1f);
            spot.range += 0.1f;
        }
        StopCoroutine("approach");
    }

    IEnumerator shaker()
    {
        float i = 3;
        while (i > 0)
        {
            yield return new WaitForSeconds(i);
            shake.Play("Shake");
            i -= 0.5f;
        }
        while (shakeTime)
        {
            shake.Play("Shake");
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("shaker");
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) SceneManager.LoadScene(lose);
    }
}
