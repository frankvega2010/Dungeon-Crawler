using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;




    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenOptions()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }



    public void BackToMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
