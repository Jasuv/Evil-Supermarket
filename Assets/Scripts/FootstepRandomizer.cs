using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FootstepRandomizer : MonoBehaviour
{
    public AudioClip[] tileStep;
    private AudioSource source;
    private float timer = 0.0f;
    [SerializeField] float waitTime = 0.4f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        Time.timeScale = 1f; 
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {

            timer += Time.deltaTime;
            if(timer > waitTime) { 
            source.clip = tileStep[Random.Range(0, tileStep.Length)];
            source.pitch = Random.Range(1 - 0.2f, 1 + 0.2f);
            source.Play();

                timer = timer - waitTime;
                Time.timeScale = 1f;

            }
        }
        
    }
}
