using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseEvents : MonoBehaviour
{
    ButcherScript butcher;
    public string supermarket;

    void Start()
    {
        butcher = GameObject.FindGameObjectWithTag("Butcher").GetComponent<ButcherScript>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) butcher.StartCoroutine("prep");
        if (col.CompareTag("Player") && this.tag == "End") SceneManager.LoadScene(supermarket); ;
    }
}
