using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HereComesTrouble : MonoBehaviour
{
    public AudioSource audioSource, audioSource2, rainAudio;
    public AudioClip YWKON, pew, victory;
    public Animator anim;
    public Text dialogtext;
    public GameObject dialogBox, Boss, nameBox;
    public bool started;
    // Start is called before the first frame update
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (Input.GetButton("Submit"))
            {
                dialogBox.SetActive(false);
                dialogtext.text = "";
                anim.Play("RelicPickup");
            }
        }
    }
    public void PlayPew()
    {
        rainAudio.PlayOneShot(pew);
    }
    public void StartFight()
    {
        Boss.SetActive(true);
        audioSource.Play();
        audioSource2.clip = victory;
        audioSource2.volume = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            audioSource.Stop();
            audioSource2.Stop();
            rainAudio.Stop();
            audioSource.clip = YWKON;
            dialogBox.SetActive(true);
            nameBox.SetActive(false);
            started = true;
            dialogtext.text = "Is this what Burkhart was talking about?";

        }
    }
}
