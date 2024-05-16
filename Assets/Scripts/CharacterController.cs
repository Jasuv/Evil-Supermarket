using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController: MonoBehaviour
{
    [SerializeField] float walk_speed;
    [SerializeField] float run_speed;
    [SerializeField] float intervals;
    [SerializeField] bool toggleablelight;
    [SerializeField] AudioClip[] tileStep;

    private AudioSource source;
    private CameraController cam;
    private Rigidbody rig;
    private Vector3 input;
    private float timer;
    private float speed;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
        rig = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        timer = 0;
    }

    // move
    private void FixedUpdate()
    {
        if (toggleablelight)
            cam.gameObject.GetComponent<Light>().range = (transform.position.z > 7) ? 12 : 3;
        transform.forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        if (!cam.in_animation)
        {
            speed = (Input.GetKey(KeyCode.LeftShift)) ? run_speed : walk_speed;
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            rig.velocity = (transform.TransformDirection(input.normalized) * speed);
        }

        if (rig.velocity.magnitude > 0.01)
        {
            timer += 1f;
            if (timer > (Input.GetKey(KeyCode.LeftShift) ? intervals * 10 / 2f : intervals * 10))
            {
                source.clip = tileStep[Random.Range(0, tileStep.Length)];
                source.pitch = Input.GetKey(KeyCode.LeftShift) ? Random.Range(0.8f, 1.2f) : Random.Range(0.4f, 0.5f);
                source.volume = Input.GetKey(KeyCode.LeftShift) ? 0.2f : 0.05f;
                source.Play();
                timer = 0;
            }
        }
    }
}
