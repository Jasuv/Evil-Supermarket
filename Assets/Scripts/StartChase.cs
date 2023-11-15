using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChase : MonoBehaviour
{
    public MonsterLightScript monster;
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) {
            monster.StartCoroutine("prep");
        }
    }
}
