using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp_flashlightScript : MonoBehaviour
{
    Light flashlight;

    void Start()
    {
        flashlight = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) flashlight.enabled = !flashlight.enabled;
    }
}
