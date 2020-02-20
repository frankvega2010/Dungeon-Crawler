using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTextInteraction : MonoBehaviour
{
    public string dialogue;
    public Image dialogueBox;
    public TextMeshProUGUI dialogueText;
    public float timeOfText;
    public float fadeSpeed;

    public static bool isInUse;
    public bool isPressed;
    public bool isDone;
    public float fadeTimer;
    public float textTimer;
    private Color fadeEffectText;
    private Color fadeEffectBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = dialogue;
        fadeEffectBox = new Vector4(dialogueBox.color.r, dialogueBox.color.g, dialogueBox.color.b, 0);
        fadeEffectText = new Vector4(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);
        dialogueBox.color = fadeEffectBox;
        dialogueText.color = fadeEffectText;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed)
        {
            if (!isDone)
            {
                if(fadeTimer >= 1)
                {
                    textTimer += Time.deltaTime;

                    if(textTimer > timeOfText)
                    {
                        isDone = true;
                    }
                    // algo
                }
                else
                {
                    fadeTimer += Time.deltaTime * fadeSpeed;

                    fadeEffectBox = new Vector4(fadeEffectBox.r, fadeEffectBox.g, fadeEffectBox.b, fadeTimer-0.25f);
                    fadeEffectText = new Vector4(fadeEffectText.r, fadeEffectText.g, fadeEffectText.b, fadeTimer);
                    dialogueBox.color = fadeEffectBox;
                    dialogueText.color = fadeEffectText;
                }
            }
            else
            {
                fadeTimer -= Time.deltaTime * fadeSpeed;
                fadeEffectBox = new Vector4(fadeEffectBox.r, fadeEffectBox.g, fadeEffectBox.b, fadeTimer);
                fadeEffectText = new Vector4(fadeEffectText.r, fadeEffectText.g, fadeEffectText.b, fadeTimer);
                dialogueBox.color = fadeEffectBox;
                dialogueText.color = fadeEffectText;

                if (fadeTimer <= 0)
                {
                    isPressed = false;
                    isDone = false;
                    isInUse = false;
                    textTimer = 0;
                    fadeTimer = 0;
                }
            }
        }
        
    }

    public void CheckMessage()
    {
        if(!isInUse)
        {
            isInUse = true;
            isPressed = true;
        }
    }
}
