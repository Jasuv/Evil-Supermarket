using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string supermarket;
    public string basement;

    public void toBasement() {
        SceneManager.LoadScene(basement);
    }

    public void toEnd()
    {
        Application.Quit();
    }
}
