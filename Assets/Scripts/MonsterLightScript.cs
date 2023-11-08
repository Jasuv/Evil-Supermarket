using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLightScript : MonoBehaviour
{
    public bool shakeTime = true;
    [SerializeField] float max;
    Light spot;
    Animator shake;

    void Start()
    {
        shake = Camera.main.GetComponent<Animator>();
        spot = GetComponent<Light>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("prep");
        }
    }

    IEnumerator prep() 
    {
        StartCoroutine("approach");
        StartCoroutine("shaker");
        StopCoroutine("prep");
        yield return null;
    }

    IEnumerator approach()
    {
        spot.range = 10;
        yield return new WaitForSeconds(2);
        transform.parent = Camera.main.transform.parent;
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
            print("hee");
            yield return new WaitForSeconds(i);
            shake.Play("Shake");
            i -= 0.5f;
        }
        while (shakeTime) {
            shake.Play("Shake");
            yield return new WaitForSeconds(0.1f);
        }
        StopCoroutine("shaker");
    }
}
