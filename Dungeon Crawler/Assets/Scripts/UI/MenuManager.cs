using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public static MenuManager singleton;
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public Animator transition;
    public float transitionTime;
    //public AudioSource audioSource;
    //public AudioMixer mixer;

    public void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            enabled = false;
        }
    }

    public void PlayButtonClicked()
    {
        //TODO call audio fade.
        LoadNextLevel();  
    }

    //TODO fade the audio to make it smooth
    //public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    //{
    //    float currentTime = 0;
    //    float currentVol;
    //    audioMixer.GetFloat(exposedParam, out currentVol);
    //    currentVol = Mathf.Pow(10, currentVol / 20);
    //    float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

    //    while (currentTime < duration)
    //    {
    //        currentTime += Time.deltaTime;
    //        float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
    //        audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
    //        yield return null;
    //    }
    //    yield break;
    //}



    public void LoadNextLevel() {  StartCoroutine(LoadLevel(1)); }


    public void LoadHouseLevel()
    {
        //LoadLevel(2);
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
    }

    public void LoadCredits()
    {
       // LoadLevel(1);
        StartCoroutine(LoadLevel(1));
    }

    public void Quit()
    {
        //LoadLevel(-1);
        StartCoroutine(LoadLevel(-1));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        Debug.Log("lol");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

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

    }



    public void OpenOptions()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }



    //public void Quit() { Application.Quit(); }



    public void BackToMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
