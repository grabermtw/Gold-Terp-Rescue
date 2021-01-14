using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Opening : MonoBehaviour
{
    public GameObject gameplay;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    int currDialogue;


    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = dialogue[currDialogue];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currDialogue++;
            if (currDialogue < dialogue.Length)
            {
                dialogueText.text = dialogue[currDialogue];
            }
            else 
            {
                gameplay.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
