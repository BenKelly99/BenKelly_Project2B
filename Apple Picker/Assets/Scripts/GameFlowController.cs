using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField]
    bool running;
    int level;
    [SerializeField]
    GameObject player, dispenser;
    Vector3 playerStart, dispenserStart;
    [SerializeField]
    Text levelText;
    [SerializeField]
    GameObject settings;
    // Start is called before the first frame update
    void Start()
    {
        settings.SetActive(false);
        running = false;
        level = 1;
        playerStart = player.transform.position;
        dispenserStart = dispenser.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit(0);
        levelText.text = " Level " + level;
        if(!running && Input.GetKeyDown(KeyCode.Mouse0))
        {
            running = true;
            dispenser.GetComponent<DispenserController>().ResetThrownTime();
            return;
        }
        
    }

    public bool GetRunning() { return running; }
    public void SetRunning(bool r) { running = r; }
    
    public int GetScoreThresh() {
        return 5 * level * (level + 9);
    }

    public void EndGame()
    {
        List<int> scores = new List<int>();
        for(int i = 0; i < 10; i++)
        {
            scores.Add(PlayerPrefs.GetInt("score" + i));
        }
        scores.Add(player.GetComponent<PlayerController>().GetScore());
        scores.Sort();
        for(int i = 10; i > 0; i--)
        {
            PlayerPrefs.SetInt("score" + (10-i), scores[i]);
            print("score" + (10 - i) + "  " + scores[i]);
        }
        SceneManager.LoadScene("EndScreen");
    }

    public void NewGame()
    {
        level = 0;
        player.GetComponent<PlayerController>().Reset();
        dispenser.GetComponent<DispenserController>().Reset();
        ResetStage();
    }
    public void ResetStage()
    {
        level += 1;
        running = false;
        player.transform.position = playerStart;
        dispenser.transform.position = dispenserStart;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Apples"))
            Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlusLife"))
            Destroy(obj);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PlusSpeed"))
            Destroy(obj);
    }
    public void OpenCloseSettings()
    {
        settings.SetActive(!settings.activeSelf);
        Time.timeScale = settings.activeSelf ? 0 : 1;
    }
}
