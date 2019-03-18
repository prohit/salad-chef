using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Transform mainTransform;
    [SerializeField] int totalPlayers;
    int playerTimeOverCount;
    Action gameOverEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //called when time is over for any player
    public void TimeOver(string playerName, int score, int bestScore)
    {
        mainTransform.Find(string.Format("{0}/Score", playerName)).GetComponent<Text>().text = "Score: " + score;
        mainTransform.Find(string.Format("{0}/Best", playerName)).GetComponent<Text>().text = "Best: " + bestScore;
        playerTimeOverCount++;
        if(playerTimeOverCount >= totalPlayers)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        gameOverEvent?.Invoke();
        mainTransform.gameObject.SetActive(true);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void AddGameOverEvent(Action pEvent)
    {
        gameOverEvent += pEvent;
    }

    public void RemoveGameOverEvent(Action pEvent)
    {
        if(gameOverEvent != null)
        {
            gameOverEvent -= pEvent;
        }
    }
}
