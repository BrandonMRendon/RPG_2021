using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public AudioSource audioSource, audioSource2, rainAudio, speaker;
    public AudioClip rain, mission, stronghold, bells, bells2;
    public Animator anim, animNPC, fader;
    public Text text, dialogtext, dialogName;
    public GameObject dialogBox, dungeonUI, gameplayUI;
    public bool isAnimating = false;
    public int messageIndex = 0;
    public bool skip, started;
    PlayerController player;
    public Collider2D colOne, colTwo;

    int nameIndex = 0;
    string[] names = new string[] { "Andor","Burkhart", "Burkhart", "Burkhart", "Burkhart", "Burkhart", "Burkhart", "Burkhart", "Knight", "Burkhart", "Knight", "Burkhart" };
    string[] messages = new string[] { "There's... something going on out there?",
        "Brother, I’m glad to see that you’re unharmed.",
        "I’m afraid there isn’t much time to explain.",
        "What’s important to know is that the castle is under attack.",
        "By whom? It is as of yet unknown. Regardless, I have a task that I can only entrust to you.",
        "Pick up your sword and spell book and make your way to the castle’s dungeon. ",
        "Inside you’ll find a labyrinth, as well as the enemy’s forces, no doubt.",
        "Retrieve that which we’ve guarded for generations, and go forth to inform Her Grace of the situation.",
        "Your Majesty, they’ve breached the inner wall. It saddens me to say but we must retreat.",
        "So the situation is as dire as I had feared. Captain, inform the royal guard to divert as much attention as they can from Prince Andor. His success is of utmost importance.",
        "Yes sir!",
        "Andor, my dear brother, please be cautious..."};
    // Start is called before the first frame update
    void Start()
    {
        rainAudio.PlayOneShot(rain);
        started = false;
        if (!skip)
        {
        
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
    }


    // Update is called once per frame
    void Update()
    {
        if (skip)
        {
            if(messageIndex == 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().playerIsFrozen = false;
                messageIndex += 1;
                audioSource.clip = mission;
                audioSource.Play();
                gameplayUI.SetActive(true);
                fader.Play("FadeOut");
            }
            return;
        }
        
        if(messageIndex<13)
        {
            if (messageIndex == 0 && !started)
            {
                started = true;
                player.dirHeld = (int)PlayerController.walkingDirection.UP;
                audioSource.Play();
                gameplayUI.SetActive(true);
                fader.Play("FadeOut");
                StartNextText();


            }
            if (Input.GetButtonDown("Submit")  && !isAnimating)
            {
                if(messageIndex == 1)
                {
                    
                    animNPC.Play("Burkhart1");
                    dialogBox.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 272, 0);
                    dialogBox.SetActive(false);
                    player.dirHeld = (int)PlayerController.walkingDirection.DOWN;
                    return;
                }
                if(messageIndex < 8)
                {
                    StartNextText();
                    return;
                }
                if (messageIndex == 8)
                {
                    dialogBox.SetActive(false);
                    animNPC.Play("Captain");
                    return;
                }
                if (messageIndex < 10)
                {
                    StartNextText();
                    return;
                }
                if (messageIndex == 10)
                {
                    StartNextText();
                    animNPC.Play("Captain2");
                }
                if(messageIndex == 11)
                {
                }
                if (messageIndex == 12)
                {
                    dialogBox.SetActive(false);
                    animNPC.Play("Burkhart2");
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().playerIsFrozen = false;
                    dialogBox.SetActive(false);
                }
                
            }
            
        }
    }
    public void StartNextText()
    {
        dialogBox.SetActive(true);
        StartCoroutine("DialogSpeed");
        
        messageIndex += 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            if(colOne != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(mission);
                Destroy(colOne);
            }
            else
            {
                StartCoroutine("EnterDungeon");
                audioSource.Stop();
                audioSource.PlayOneShot(stronghold);
                Destroy(colTwo);
            }
            
        }
    }
    IEnumerator EnterDungeon()
    {
        yield return new WaitForSeconds(3);
        dungeonUI.SetActive(true);
        rainAudio.Stop();
        text.text = "";
        text.color = Color.black;
        text.fontSize = 40;
        anim.Play("TextFadeIn");
        
        string dungeonName = "Fahtum Castle";
        for (int i = 0; i < dungeonName.Length; i++)
        {
            text.text += dungeonName[i];
            yield return new WaitForSeconds(.05f);
        
        }
        yield return new WaitForSeconds(3);
        anim.Play("TextIntro");
    }
    IEnumerator DialogSpeed()
    {
        isAnimating = true;
        int messageDex = messageIndex;
        dialogtext.text = "";
        dialogName.text = names[nameIndex];
        nameIndex += 1;
        for (int i = 0; i < messages[messageDex].Length; i++)
        {
            dialogtext.text += messages[messageDex][i];
            speaker.Play();
            yield return new WaitForSeconds(.05f);
        }
        
        isAnimating = false;
    }
}
