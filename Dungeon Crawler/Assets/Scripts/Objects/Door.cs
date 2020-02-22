using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public enum doorStates
    {
        open,
        closed,
        locked,
        unlocked,
        allStates
    }

    //public PlayerTextInteraction dialogue;
    public Animator animator;
    public doorStates currentState;
    public bool lockedDoor;
    public int idOfKey;
    public AudioSource[] doorSounds;
    public bool sceneChanger;
    public string nameOfScene;
    public GameObject dialoguePrefab;
    public GameObject dialogueObject;
    public PlayerTextInteraction dialogueEvent;


    // Start is called before the first frame update
    void Start()
    {
        if(!dialogueObject)
        {
            dialogueObject = Instantiate(dialoguePrefab);
            dialogueObject.transform.SetParent(GameObject.Find("Canvas").transform,false);
            dialogueEvent = dialogueObject.GetComponent<PlayerTextInteraction>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Interact(int keyID)
    {
        bool couldOpen = false;

        switch (currentState)
        {
            case doorStates.open:
                if (animator)
                {
                    animator.SetTrigger("Close");
                }
                
                currentState = doorStates.closed;
                if (doorSounds.Length > 0)
                {
                    doorSounds[(int)doorStates.closed].Play();
                }
                
                break;
            case doorStates.closed:
                if(lockedDoor)
                {
                    if(keyID == idOfKey)
                    {
                        Debug.Log("DOOR IS NOW OPEN");
                        if (doorSounds.Length > 0)
                        {
                            doorSounds[(int)doorStates.unlocked].Play();
                        }
                        
                        lockedDoor = false;
                        couldOpen = true;
                    }
                    else
                    {
                        //dialogueEvent.CheckMessage();
                    }
                    
                }

                if (!lockedDoor && !couldOpen)
                {
                    if (animator)
                    {
                        animator.SetTrigger("Open");
                    }
                    
                    currentState = doorStates.open;
                    if (doorSounds.Length > 0)
                    {
                        doorSounds[(int)doorStates.open].Play();
                    }

                    if (sceneChanger)
                    {
                        Invoke("ChangeScene", 1f);
                    }

                    couldOpen = true;
                }
                else if (!lockedDoor && couldOpen)
                {
                    Debug.Log("OPENING");
                    Invoke("OpenDoor", 1.7f);
                }
                break;
            default:
                break;
        }

        return couldOpen;
    }

    private void OpenDoor()
    {
        Debug.Log("opening door");

        if(animator)
        {
            animator.SetTrigger("Open");
        }
        
        currentState = doorStates.open;

        if(doorSounds.Length > 0)
        {
            doorSounds[(int)doorStates.open].Play();
        }

        if (sceneChanger)
        {
            Invoke("ChangeScene", 1f);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(nameOfScene);
        if(SceneManager.GetActiveScene().name == "Dungeon Beta")
        {
            GameManager.Get().spawnInForest = true;
            GameManager.Get().RelocatePlayer();
        }
        
    }
}
