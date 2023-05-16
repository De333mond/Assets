using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public void _LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void _Quit()
    {
        Application.Quit();
    }
}
