using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walk_speed;
    [SerializeField] float run_speed;
    [SerializeField] float sensitivity_x;
    [SerializeField] float sensitivity_y;
    [SerializeField] float range;
    [SerializeField] float animation_time;
    public KeyCode brush = KeyCode.Mouse0;

    private float speed;
    private Rigidbody rig;
    private Transform hand;

    private float yaw;
    private float pitch;
    private Vector3 input;


    private bool in_animation = false; // Update doesn't wait for couritines
    private bool holding = false; // Toggles pick up & put down action

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        hand = transform.GetChild(1).transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Move
    private void FixedUpdate()
    {
        if (!in_animation)
        {
            Vector3 pos = rig.position;
            speed = (Input.GetKey(KeyCode.LeftShift)) ? run_speed : walk_speed;
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 posPrime = pos + rig.transform.TransformDirection(input.normalized) * speed * Time.fixedDeltaTime;
            rig.position = posPrime;
        }
    }

    // Look + Pick-up
    void Update()
    {
        if (!in_animation)
        {
            yaw = Input.GetAxis("Mouse X") * sensitivity_x;
            pitch += Input.GetAxis("Mouse Y") * sensitivity_y;
            pitch = Mathf.Clamp(pitch, -80, 80);
            Camera.main.transform.localRotation = Quaternion.Euler(-pitch, 0, 0);
            transform.rotation *= Quaternion.Euler(0, yaw, 0);

            if (Input.GetKeyDown(KeyCode.E)) StartCoroutine("Pickup");
        }
    }

    // Pick up function
    private IEnumerator Pickup() 
    {
        in_animation = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, range)) 
        {
            if (!holding)
            {
                // Pick up object
                if (hit.collider.CompareTag("object"))
                {
                    hit.collider.transform.SetParent(hand.transform);
                    yield return StartCoroutine(SmoothLerp(animation_time, hit.collider.transform, hand));
                    holding = true;
                }

                // Pick up broom
                if (hit.collider.CompareTag("broom"))
                {
                    hit.collider.transform.SetParent(hand.transform);
                    yield return StartCoroutine(SmoothLerp(animation_time, hit.collider.transform, hand));
                    hit.collider.GetComponent<BroomScript>().canSweep = true;
                    holding = true;
                }
            }

            else 
            {
                // Put down object
                if (hit.collider.CompareTag("place") && hand.transform.GetChild(0).CompareTag("object"))
                {
                    yield return StartCoroutine(SmoothLerp(animation_time, hand.transform.GetChild(0), hit.collider.transform));
                    hand.transform.GetChild(0).SetParent(null);
                    holding = false;
                }

                // Put down broom
                if (hit.collider.CompareTag("holster") && hand.transform.GetChild(0).CompareTag("broom"))
                {
                    yield return StartCoroutine(SmoothLerp(animation_time, hand.transform.GetChild(0), hit.collider.transform));
                    hand.transform.GetChild(0).GetComponent<BroomScript>().canSweep = false;
                    hand.transform.GetChild(0).SetParent(null);
                    holding = false;
                }
            }
        }
        in_animation = false;
    }

    // Animation
    private IEnumerator SmoothLerp(float time, Transform thing, Transform end)
    {
        Vector3 start = thing.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            thing.position = Vector3.Lerp(start, end.position, (elapsedTime / time));
            thing.rotation = Quaternion.Lerp(thing.rotation, end.rotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
