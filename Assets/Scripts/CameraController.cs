using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Globalization;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] SupermarketEvents gameEvents;
    [SerializeField] ChoreEvents choreEvents;

    public AudioClip[] sounds = new AudioClip[4];
    public AudioSource source;

    [SerializeField] float sensitivity_x;
    [SerializeField] float sensitivity_y;
    [SerializeField] float grab_range;
    [SerializeField] float animation_time;

    [HideInInspector] public bool in_animation = false;

    public KeyCode use_key = KeyCode.Mouse0;
    public KeyCode grab_key = KeyCode.E;

    private Transform hand;
    private GameObject holding;
    private Animator sweep;
    private Transform body;
    private RawImage cursor;
    private float pitch;
    private float yaw;
    private bool can_sweep = false;
    private RaycastHit hit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        choreEvents = FindFirstObjectByType<ChoreEvents>();
        gameEvents = FindFirstObjectByType<SupermarketEvents>();

        body = GameObject.FindGameObjectWithTag("Player").transform;
        hand = transform.GetChild(0).transform;
        sweep = GameObject.FindGameObjectWithTag("Broom").GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        cursor = FindFirstObjectByType<TextMessages>().transform.GetChild(0).GetComponent<RawImage>();
    }

    // look, call pick up, sweep
    void Update()
    {
        transform.position = new Vector3 (body.position.x, body.position.y + 0.5f, body.position.z);

        pitch += Input.GetAxis("Mouse Y") * sensitivity_y;
        yaw += Input.GetAxis("Mouse X") * sensitivity_x;
        transform.localRotation = Quaternion.Euler(-Mathf.Clamp(pitch, -80, 80), yaw, 0);

        // interact
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, grab_range)) {
            Collider col = hit.collider;

            // show/hide cursor
            if (!holding && (col.CompareTag("Object") || col.CompareTag("Broom"))) cursor.enabled = true;
            else if (holding && (col.CompareTag("Placement") || col.CompareTag("Holster"))) cursor.enabled = true;
            else if (col.CompareTag("Monitor")) cursor.enabled = true;
            else if (choreEvents.end_day && col.CompareTag("Car")) cursor.enabled = true;
            else if (col.CompareTag("Door")) cursor.enabled = true;
            else cursor.enabled = false;

            // pick up/put down
            if (Input.GetKeyDown(grab_key)) StartCoroutine("Interact", col);
            // sweep mess
            if (col.CompareTag("Mess") && Input.GetKeyDown(use_key) && can_sweep) StartCoroutine("Sweep", col);
            // sweep mess
            if (col.CompareTag("Monitor") && Input.GetKeyDown(grab_key)) col.transform.GetChild(0).GetComponent<PrinterScript>().StartCoroutine("print");
            // end day
            if (col.CompareTag("Car") && choreEvents.end_day && Input.GetKeyDown(grab_key)) gameEvents.nextDay();
            // go to basement
            if (col.CompareTag("Door") && Input.GetKeyDown(grab_key)) toBasement();
        }
    }

    // pick up/putdown
    private IEnumerator Interact(Collider col)
    {
        in_animation = true;
        // pick up object or broom
        if (!holding)
        {
            if (col.CompareTag("Object") || col.CompareTag("Broom"))
            {
                source.clip = sounds[0];
                source.Play();
                yield return StartCoroutine(SmoothLerp(animation_time, col.transform, hand));
                holding = col.gameObject;
                holding.transform.SetParent(hand.transform);
                if (col.CompareTag("Broom")) can_sweep = true;
            }
        }
        else
        {
            // put down object
            if (col.CompareTag("Placement") && holding.CompareTag("Object"))
            {
                source.clip = sounds[1];
                source.Play();
                holding.transform.SetParent(null);
                yield return StartCoroutine(SmoothLerp(animation_time, holding.transform, col.transform));
                holding = null;
            }
            // put down broom
            if (col.CompareTag("Holster") && holding.CompareTag("Broom"))
            {
                source.clip = sounds[1];
                source.Play();
                holding.transform.SetParent(null);
                can_sweep = false;
                yield return StartCoroutine(SmoothLerp(animation_time, holding.transform, col.transform));
                holding = null;
            }
        }
        in_animation = false;
    }

    // animation
    private IEnumerator SmoothLerp(float time, Transform thing, Transform end)
    {
        Vector3 start = thing.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            thing.position = Vector3.Lerp(start, end.position, (elapsedTime / time));
            thing.rotation = Quaternion.Lerp(thing.rotation, end.rotation, (elapsedTime / time));
            elapsedTime++;
            yield return null;
        }

        thing.position = end.position;
        thing.rotation = end.rotation;
    }

    private IEnumerator Sweep(Collider col)
    {
        source.clip = sounds[2];
        source.Play();
        sweep.Play("Sweep");
        col.gameObject.GetComponent<Animator>().Play("cleanup");
        yield return new WaitForSeconds(2);
        Destroy(col.gameObject);
        choreEvents.cleaned++;
        yield return null;
    }

    public void toBasement()
    {
        SceneManager.LoadScene("Basement");
    }
}
