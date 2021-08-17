using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject Title, Credits;

    string[] vowels = new string[] { "a", "e", "i", "o", "u", "y" };
    string[] consinants = new string[] { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
    public void LoadSceneCredits()
    {
        Title.SetActive(false);
        Credits.SetActive(true);
    }
    public void LoadSceneGame()
    {
        string name = consinants[Random.Range(0,21)] + vowels[Random.Range(0, 6)] + consinants[Random.Range(0, 21)] + vowels[Random.Range(0, 6)] + consinants[Random.Range(0, 21)]+ vowels[Random.Range(0, 6)];
        print(name);

        //SceneManager.LoadScene("Credits");
    }
    public void LoadSceneNewGame()
    {
        
        SceneManager.LoadScene("Intro");
    }
    public void LoadSceneBack()
    {
        Title.SetActive(true);
        Credits.SetActive(false);
    }

}
