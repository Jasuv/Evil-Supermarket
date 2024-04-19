using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
    private Light scanner;
    private Transform body;
    private float pitch;
    private float yaw;
    private bool can_sweep = false;
    private bool can_scan = false;


    void Start()
    {
        sweep = GameObject.FindGameObjectWithTag("Broom").GetComponent<Animator>();
        scanner = GameObject.FindGameObjectWithTag("Scanner").transform.GetChild(0).GetComponent<Light>();
        body = GameObject.FindGameObjectWithTag("Player").transform;
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // look, call pick up, sweep
    void Update()
    {
        transform.position = new Vector3 (body.position.x, body.position.y + 0.5f, body.position.z);

        pitch += Input.GetAxis("Mouse Y") * sensitivity_y;
        yaw += Input.GetAxis("Mouse X") * sensitivity_x;
        transform.localRotation = Quaternion.Euler(-Mathf.Clamp(pitch, -80, 80), yaw, 0);
        if (Input.GetKeyDown(grab_key)) StartCoroutine("Interact");
        if (Input.GetKeyDown(use_key) && can_sweep) sweep.Play("Sweep");
        if (Input.GetKeyDown(use_key) && can_scan) StartCoroutine("Scan", scanner);
    }

    // pick up
    private IEnumerator Interact()
    {
        in_animation = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, grab_range))
        {
            Collider col = hit.collider;

            // pick up object or broom
            if (!holding)
            {
                if (col.CompareTag("Object") || col.CompareTag("Broom") || col.CompareTag("Scanner"))
                {
                    yield return StartCoroutine(SmoothLerp(animation_time, hit.collider.transform, hand));
                    holding = col.gameObject;
                    holding.transform.SetParent(hand.transform);
                    if (col.CompareTag("Broom")) can_sweep = true;
                    if (col.CompareTag("Scanner")) can_scan = true;
                }
            }

            else
            {
                // put down object
                if (hit.collider.CompareTag("Placement") && holding.CompareTag("Object"))
                {
                    holding.transform.SetParent(null);
                    yield return StartCoroutine(SmoothLerp(animation_time, holding.transform, hit.collider.transform));
                    holding = null;
                }

                // put down broom
                if (hit.collider.CompareTag("Holster") && (holding.CompareTag("Broom") || holding.CompareTag("Scanner")))
                {
                    holding.transform.SetParent(null);
                    can_sweep = false;
                    can_scan = false;
                    yield return StartCoroutine(SmoothLerp(animation_time, holding.transform, hit.collider.transform));
                    holding = null;
                }
            }

            // "open" door
            if (hit.collider.CompareTag("Door"))
            {
                GameObject.FindObjectOfType<SupermarketEvents>().toBasement();
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

    private IEnumerator Scan(Light light)
    {
        light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        light.enabled = false;
        StopCoroutine("Scan");
    }
}
