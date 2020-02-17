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
    public AudioSource audioSource;
    public AudioMixer mixer;

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


    IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }



    public void OpenOptions()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }



    public void Quit() { Application.Quit(); }



    public void BackToMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
