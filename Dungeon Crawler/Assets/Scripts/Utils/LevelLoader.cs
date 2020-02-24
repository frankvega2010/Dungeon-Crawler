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
        MusicManager.Get().houseSong.Play();
    }

    public void LoadDungeonLevel()
    {
        //LoadLevel(3);
        StartCoroutine(LoadLevel(3));
        MusicManager.Get().StopAllSongs();
        MusicManager.Get().dungeonSong.Play();
    }

    public void LoadMenu()
    {
        //LoadLevel(0);
        Debug.Log("GOING TO MENU");
        StartCoroutine(LoadLevel(0));
        MusicManager.Get().StopAllSongs();
        MusicManager.Get().menuSong.Play();
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
            case 0:
                if (GameManager.Get().gameObject)
                {
                    Debug.Log("Deleting Everything");
                    GameManager.Get().CleanEverything();
                }
                break;
            case 2:
                //GameManager.Get().houseDoor.enabled = false;
                GameManager.Get().spawnInForest = true;
                GameManager.Get().RelocatePlayer();
                GameManager.Get().player.GetComponentInChildren<Camera>().farClipPlane = 300;
                break;
            default:
                break;
        }


    }
}
