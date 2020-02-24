using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    private void GoBack()
    {
        GameManager.Get().levelLoader.LoadMenu();
        //GameManager.Get().CleanEverything();
    }
}
