using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{
    public GameObject newGame, highScore, quit, back, highScoresText;
    // Start is called before the first frame update
    void Start()
    {
        if (highScoresText != null)
        {
            highScoresText.GetComponent<Text>().text = "";
            for (int i = 0; i < 10; i++)
            {
                highScoresText.GetComponent<Text>().text += (i + 1) + ". " + PlayerPrefs.GetInt("score" + i) + "\n";
            }
            highScoresText.SetActive(false);
        }
        if(newGame != null)
            newGame.SetActive(true);
        if(highScore != null)
            highScore.SetActive(true);
        if(quit != null)
            quit.SetActive(true);
        if(back != null)
            back.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit(0);
    }

    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ChangeMenu()
    {
        newGame.SetActive(!newGame.activeSelf);
        highScore.SetActive(!highScore.activeSelf);
        quit.SetActive(!quit.activeSelf);
        back.SetActive(!back.activeSelf);
        highScoresText.SetActive(!highScoresText.activeSelf);
    }
}
