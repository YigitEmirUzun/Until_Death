using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager Instance;

    public Queue<string> sentences = new Queue<string>();

    public TextMeshProUGUI dialoguetext;
    public TextMeshProUGUI name;
    public GameObject panel;

    public bool isDialogue = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        panel.SetActive(true);
        name.text = dialogue.name;
        isDialogue = true;
        
        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialoguetext.text = sentence;

    }

    public void EndDialogue()
    {
        isDialogue = false;
        panel.SetActive(false);
        FindAnyObjectByType<PlayerController>().enabled = true;
    }

    private void Update()
    {
        if (isDialogue)
        {
            FindAnyObjectByType<PlayerController>().enabled = false;
        }
    }

}
