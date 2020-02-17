using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void PlayButtonClicked()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
       StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start"); 
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
