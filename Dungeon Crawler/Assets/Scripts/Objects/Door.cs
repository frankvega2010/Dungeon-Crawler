using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        
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
                animator.SetTrigger("Close");
                currentState = doorStates.closed;
                doorSounds[(int)doorStates.closed].Play();
                break;
            case doorStates.closed:
                if(lockedDoor)
                {
                    if(keyID == idOfKey)
                    {
                        Debug.Log("DOOR IS NOW OPEN");
                        doorSounds[(int)doorStates.unlocked].Play();
                        lockedDoor = false;
                        couldOpen = true;
                    }

                    //dialogue.isPressed = true;
                }

                if (!lockedDoor && !couldOpen)
                {
                    animator.SetTrigger("Open");
                    currentState = doorStates.open;
                    doorSounds[(int)doorStates.open].Play();
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
        animator.SetTrigger("Open");
        currentState = doorStates.open;
        doorSounds[(int)doorStates.open].Play();
    }
}
