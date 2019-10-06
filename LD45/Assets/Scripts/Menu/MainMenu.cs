using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Begin()
    {
        GameObject timer = GameObject.Find("Timer");
        if (timer)
        {
            Destroy(timer);
        }
        

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();

    }
}
