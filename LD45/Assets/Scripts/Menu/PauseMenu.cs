using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_menu;

    bool disabledToggle = false;

    // Start is called before the first frame update
    void Start()
    {
        Continue();
    }
   
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            disabledToggle = !disabledToggle;
            Camera.main.gameObject.GetComponent<ScreenFX>().SetDisabled(disabledToggle);
            m_menu.SetActive(disabledToggle);

        }
    }

    public void Continue()
    {
        Camera.main.gameObject.GetComponent<ScreenFX>().SetDisabled(false);
        m_menu.SetActive(false);
        disabledToggle = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
