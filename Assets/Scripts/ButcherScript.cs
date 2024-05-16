using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

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

    public CharacterController characterController;
    public CameraController cameraController;

    public AudioClip[] sounds;
    public AudioSource source1;
    public AudioSource source2;

    void Start()
    {
        characterController = FindFirstObjectByType<CharacterController>();
        cameraController = FindFirstObjectByType<CameraController>();

        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false); // mega ugo but idc at this point
        trigger = GetComponent<Collider>();
        trigger.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shake = Camera.main.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spot = GetComponent<Light>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Camera.main != null) 
        {
            Vector3 targetPostition = new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
            transform.GetChild(0).transform.LookAt(targetPostition);
        }
        if (chase && player != null) agent.SetDestination(player.position);
    }

    public IEnumerator prep()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        trigger.enabled = true;
        StartCoroutine("approach");
        StopCoroutine("prep");
        yield return null;
    }

    IEnumerator approach()
    {
        source1.clip = sounds[0];
        source1.Play();
        source2.clip = sounds[1];
        source2.Play();
        spot.range = 10;
        yield return new WaitForSeconds(2);
        StartCoroutine("shaker");
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
        float i = 2;
        while (i > 0.5f)
        {
            yield return new WaitForSeconds(i);
            source1.clip = sounds[Random.Range(0,2)+2];
            source1.Play();
            shake.Play("Shake");
            i -= 0.2f;
        }
        while (shakeTime)
        {
            yield return new WaitForSeconds(0.5f);
            source1.clip = sounds[Random.Range(0, 2) + 2];
            source1.Play();
            shake.Play("Shake");
        }
        StopCoroutine("shaker");
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine("kill_player");
        }
    }

    private IEnumerator kill_player()
    {
        source1.clip = sounds[0];
        source1.Play();
        source2.clip = sounds[4];
        source2.Play();
        StopCoroutine("shaker");
        Destroy(characterController.gameObject);
        Destroy(cameraController.gameObject);
        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Day3");
    }
}
