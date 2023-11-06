using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public List<char> pauseCharacters;
    private Queue<string> sentences;
    private bool detectInput = false;
    private string currentSentence;
    private bool typing = false;
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if(detectInput) 
        {
            if (Input.GetKeyDown(KeyCode.Z) && !typing) DisplayNextSentence();
            if (Input.GetKey(KeyCode.X)) DisplayFullSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null ) { player.detectInput = false; }
        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.sentences) 
        { 
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        detectInput = true;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count <= 0) 
        {
            Invoke("EndDialogue", 0.1f);
            return;
        }
        string sentence = sentences.Dequeue();
        currentSentence = sentence;
        //dialogueText.text = sentence;
        StopAllCoroutines();
        //StartCoroutine(TypeSentence(sentence));
        StartCoroutine(TypeSentence(sentence, 0.05f));
        typing = true;
    }
    IEnumerator TypeSentence (string sentence, float speed)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            var tempSpeed = speed;
            dialogueText.text += letter;
            if (pauseCharacters.Contains(letter)) tempSpeed = (speed * 5);
            yield return new WaitForSeconds(tempSpeed);
        }
        typing = false;
    }
    public void DisplayFullSentence()
    {
        StopAllCoroutines();
        dialogueText.text = currentSentence;
        typing = false;
    }
    //IEnumerator TypeSentenceDelayed(string sentence, float delay)
    //{
    //    dialogueText.text = "";
    //    float timer = delay;
    //    int index = 0;
    //    var text = sentence.ToArray();

    //    while (dialogueText.text.Length < sentence.Length)
    //    {
    //        if (timer <= 0)
    //        {
    //            dialogueText.text += text[index];
    //            timer = delay;
    //            if (pauseCharacters.Contains(text[index])) timer += (delay * 5);
    //            index++;
    //            yield return null;
    //        }
    //        //Debug.Log(dialogueText.text);
    //        timer -= (Time.deltaTime * 0.0001f);
    //    }
    //}
    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null) { player.detectInput = true; }
        detectInput = false;
    }
}
