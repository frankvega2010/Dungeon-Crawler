using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicEvent : MonoBehaviour
{
    public PlayerTextInteraction[] dialogues;
    public bool eventHasStarted;
    public Camera cinematicCamera;

    private int currentDialogue = 0;
    private bool doOnce;
    private PlayerTextInteraction[] dialogueEvents;


    // Start is called before the first frame update
    void Start()
    {
        if (dialogues.Length > 0)
        {
            dialogueEvents = new PlayerTextInteraction[dialogues.Length];

            for (int i = 0; i < dialogues.Length; i++)
            {
                dialogueEvents[i] = Instantiate(dialogues[i]);
                dialogueEvents[i].gameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
                //dialogueEvent = dialogueObject.GetComponent<PlayerTextInteraction>();
            }
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ExecuteCinematic();
        }

        if(eventHasStarted)
        {
            if(!doOnce)
            {
                dialogueEvents[currentDialogue].isPressed = true;
                doOnce = true;
            }
            
            if(dialogueEvents[currentDialogue].isDone)
            {
                doOnce = false;
                currentDialogue++;
                if(currentDialogue >= dialogueEvents.Length)
                {
                    eventHasStarted = false;
                    currentDialogue = 0;
                    
                    if(cinematicCamera)
                    {
                        GameManager.Get().player.gameObject.SetActive(true);
                        cinematicCamera.gameObject.SetActive(false);
                    }
                    
                }
            }
        }
    }

    public void ExecuteCinematic()
    {
        if (cinematicCamera)
        {
            GameManager.Get().player.gameObject.SetActive(false);
            cinematicCamera.gameObject.SetActive(true);
        }

        eventHasStarted = true;
    }
}
