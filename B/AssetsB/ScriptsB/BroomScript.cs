using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomScript : MonoBehaviour
{
    private Animator anim;
    public bool canSweep = false;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && canSweep)
        {
            anim.Play("Sweep");
        }
    }
}
