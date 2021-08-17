using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public AudioSource audioSource, audioSource2, rainAudio;
    public AudioClip rain, mission, stronghold, bells, bells2;
    public Animator anim, animNPC, fader;
    public Text text, dialogtext, dialogName;
    public GameObject dialogBox, dungeonUI, gameplayUI;
    bool isAnimating = false;
    public int messageIndex = 0;
    public bool skip;
    PlayerController player;

    int nameIndex = 0;
    string[] names = new string[] { "Jeriah", "Captain J. Spark", "Marie", "Alyssa", "Captain J. Spark" };
    string[] messages = new string[] { "On a small continent, isolated from the rest of the world, there once lived four kingdoms.", 
        "Each of the four kingdoms guarded a gift, granted to them by the gods of old.",
        "The power of these relics were without equal, and as such, the royal families of each kingdom built four strongholds to house such tremendous power, in fear it may fall into the hands of evil.",
        "For many generations, these four kingdoms lived in prosperity…",
        "Until…",
        "One dark and fateful day, malevolent forces rushed forth from the darkness of night.",
        "They mounted an assault on the good people of the Kingdom of Tempus, all in the name of their leader...",
        "The Goddess of Darkness.",
        "She burnt forest to ash, choked the land’s sweet springs, and murdered without hesitation.",
        "She did all this in her lust to take the ultimate power protected by the kingdom.",
        "The benevolent king of Tempus was slain, and his son, Prince Andor fled with his remaining forces to retrieve the relic of Tempus, and take refuge in the neighboring kingdom of Imperium…",//Restituo?
        "Here, your highness, wear this cloak. It’ll make you less of a target.",
        "Here we are, the stronghold of Tempus.",
        "It seems we weren’t the first to arrive… the enemy’s forces have already begun their assault.",
        "The stronghold’s defenses won’t be able to hold for much longer, we must hurry.",
        "Prince Andor, make your way to the stronghold and retrieve the relic. We’ll buy you as much time out here as we can!"};
    float[] textDelay = new float[] {10,10,20,10,5,10,15,7,10,10,30,5,5,5,5,5};
    // Start is called before the first frame update
    void Start()
    {
        rainAudio.PlayOneShot(rain);
        if (!skip)
        {
            StartCoroutine("Speaking");
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
        if(messageIndex == 3)
        {
            audioSource.Play();
        }
        if(messageIndex == 6)
        {
            audioSource2.Play();
        }
        if(messageIndex == 7)
        {
            
            audioSource.Stop();
            audioSource.clip = mission;
        }
        if(messageIndex == 10)
        {
            
            audioSource.Play();
            StartCoroutine(AudioFadeOut.FadeOut(audioSource2, 30));
        }
        if(messageIndex == 11)
        {
            fader.Play("FadeOut");
            player.dirHeld = (int)PlayerController.walkingDirection.RIGHT;
            dialogBox.SetActive(true);
            animNPC.Play("Jeriah");
            StartCoroutine("DialogSpeed");
            gameplayUI.SetActive(true);
            messageIndex += 1;

        }
        if(messageIndex > 11 && messageIndex<16)
        {
            if (Input.GetButtonDown("Submit")  && !isAnimating)
            {
                if(messageIndex == 12)
                {
                    player.dirHeld = (int)PlayerController.walkingDirection.UP;
                    animNPC.Play("Captain");
                }
                if (messageIndex == 13)
                {
                    player.dirHeld = (int)PlayerController.walkingDirection.LEFT;
                    animNPC.Play("Marie");
                }
                if (messageIndex == 14)
                {
                    animNPC.Play("Alyssa");
                }
                if (messageIndex == 15)
                {
                    player.dirHeld = (int)PlayerController.walkingDirection.UP;
                    animNPC.Play("Captain2");
                }
                
            }
            
        }
        if(messageIndex == 17)
        {
            if (Input.GetButtonDown("Submit"))
            {
                animNPC.Play("AllExit");
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().playerIsFrozen = false;
                dialogBox.SetActive(false);
            }
        }
    }
    public void StartNextText()
    {
        StartCoroutine("DialogSpeed");
        
        messageIndex += 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine("EnterDungeon");
            audioSource.Stop();
            audioSource.PlayOneShot(stronghold);
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
        
        string dungeonName = "Stronghold of Tempus";
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
            yield return new WaitForSeconds(.05f);
        }
        if (messageIndex == 16) messageIndex = 17;
        isAnimating = false;
    }
    IEnumerator Speaking()
    {

        Cursor.visible = false;
        for (messageIndex = 0; messageIndex < 11; messageIndex++)
        {
            text.text = messages[messageIndex];
            anim.Play("TextFadeIn");
            yield return new WaitForSeconds(textDelay[messageIndex]/1.5f);
            anim.Play("TextIntro");
            yield return new WaitForSeconds(1);
        }
        
    }
}
