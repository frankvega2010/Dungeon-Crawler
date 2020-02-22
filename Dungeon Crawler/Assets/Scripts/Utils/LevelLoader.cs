using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    private void Start()
    {
        if(!transition)
        {
            transition = GameManager.Get().canvasAnimator;
        }
        
    }

    public void PlayButtonClicked()
    {
       // LoadNextLevel();
    }

    /*public void LoadNextLevel()
    {
       StartCoroutine(LoadLevel(1));
    }*/

    public void LoadHouseLevel()
    {
        StartCoroutine(LoadLevel(2));
        MusicManager.Get().StopAllSongs();
        MusicManager.Get().gameplaySong.Play();
    }

    public void LoadDungeonLevel()
    {
        //LoadLevel(3);
        StartCoroutine(LoadLevel(3));
    }

    public void LoadMenu()
    {
        //LoadLevel(0);
        StartCoroutine(LoadLevel(0));
        MusicManager.Get().StopAllSongs();
        MusicManager.Get().menuSong.Play();
        if(GameManager.Get())
        {
            Destroy(GameManager.Get());
        }
        
    }

    public void LoadCredits()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void Quit()
    {
        //LoadLevel(-1);
        StartCoroutine(LoadLevel(-1));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start"); 
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
        if (sceneIndex != -1)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        switch (sceneIndex)
        {
            case 2:
                GameManager.Get().spawnInForest = true;
                GameManager.Get().RelocatePlayer();
                break;
            default:
                break;
        }


    }
}
