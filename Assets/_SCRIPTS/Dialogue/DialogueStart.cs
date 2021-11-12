using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DialogueStart : MonoBehaviour
{
    //Dialogue start must access scene, and serve dialogue to a single dialogue zone
    public Dialogue dialogue;//allows dialogue trigger to use dialogue constructor class we set up
    public string scene;
    private int index = 0;

    //PUBLIC options for Inspector
    [SerializeField]
    private bool oneTimeDialogue;
    [SerializeField]
    private bool typeSentence;
    public bool ready = false;
    [SerializeField]
    private GameObject talkIcon;

    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        dialogue = new Dialogue(scene, "Conversation");//we now have the dialogue
        talkIcon.transform.localScale.Set(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioManager.instance.PlayDialogue("moth4");
            ready = true;
            talkIcon.SetActive(true);
            talkIcon.transform.DOScale(1f, .3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ready = false;
            talkIcon.transform.DOScale(0f, .3f).OnComplete(()=>talkIcon.SetActive(false));
            ExitDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && Input.GetKeyDown(KeyCode.E))
        {
            DialogueZone.instance.gameObject.SetActive(true);
            DialogueZone.instance.transform.DOMoveY(80, .2f);
            NextSentence();
        }
    }

    private void NextSentence()
    {
        //set speaker
        string s = dialogue.sentences[index];
        while (dialogue.sentences[index].Contains("//"))
        {
            if (dialogue.sentences[index].Contains("//:"))
            {
                StoryManager.instance.Execute(s.TrimStart('/', ':'));
                index++;
            }
            else
            {
                switchSpeaker(dialogue.sentences[index]);
            }
        }



        AudioManager.instance.PlayDialogue("moth" + character.sounds[character.soundIndex++ % character.sounds.Length]);
        if (!typeSentence) DialogueZone.instance.content.text = dialogue.sentences[index];
        else { StopAllCoroutines(); StartCoroutine(TypeSentence(dialogue.sentences[index])); }

        //next sentence, check if conversation is over
        index++;
        if (index >= dialogue.sentences.Length)
        {
            ExitDialogue();
            if (oneTimeDialogue) ready = false;
        }
    }

    private void ExitDialogue()
    {
        index = 0;
        DialogueZone.instance.transform.DOMoveY(-80f, .3f).OnComplete(() => DialogueZone.instance.gameObject.SetActive(false));
    }

    private void switchSpeaker(string s)
    {
        DialogueZone.instance.transform.DOMoveY(-80, .2f);
        s = s.Trim('/');
        index++;//skip the speaker tag so it does not display
        character = Resources.Load<Character>("CHARACTERS/" + s) ?? Resources.Load<Character>("CHARACTERS/DEFAULT");
        DialogueZone.instance.speakerName.color = character.textColor;
        DialogueZone.instance.content.color = character.textColor;

        DialogueZone.instance.speakerName.text = s;
        DialogueZone.instance.portrait.sprite = character.portrait;
        DialogueZone.instance.transform.DOMoveY(80, .2f);

    }

    IEnumerator TypeSentence(string line)
    {
        DialogueZone.instance.content.text = "";
        string s = "";
        char letter;
        for (int i = 0; i < line.Length; i++)
        {
            letter = line.ToCharArray()[i];
            if (letter == '<')
            {
                while (letter != '>' && i + 1 < line.Length)
                {

                    s += letter;
                    i++;
                    letter = line.ToCharArray()[i];
                }
                s += letter;
                DialogueZone.instance.content.text += s;
                yield return null;
            }
            else
            {
                DialogueZone.instance.content.text += letter;
                yield return null;
            }
        }
    }
}

