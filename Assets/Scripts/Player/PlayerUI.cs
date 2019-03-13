using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    const int GAME_TIME = 120; //seconds

    [SerializeField] Player player;

    int gameTime; 
    int gameScore;
    Text labelScore;
    Text labelTime;

    Transform carriedVegetablesTransform;

    // Start is called before the first frame update
    void Start()
    {
        gameTime = GAME_TIME;
        labelScore = transform.Find("Score/Text").GetComponent<Text>();
        labelTime = transform.Find("Time/Text").GetComponent<Text>();
        carriedVegetablesTransform = transform.Find("CarriedVegetables");

        player.AddScoreUpdateEvent(OnScoreUpdate);
        player.AddUpdateTimeEvent(OnTimerUpdate);
        player.AddCarryVegetableEvent(OnCarryVegetableUpdate);

        StartCoroutine(TimeCounter());
    }

    private void OnDestroy()
    {
        if(player != null)
        {
            player.RemoveScoreUpdateEvent(OnScoreUpdate);
            player.RemoveUpdateTimeEvent(OnTimerUpdate);
            player.RemoveCarryVegetableEvent(OnCarryVegetableUpdate);
        }
    }

    IEnumerator TimeCounter()
    {
        while (gameTime > 0)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
            labelTime.text = string.Format("{0}s", gameTime);
        }
        //game over
        //notify player
    }

    void OnScoreUpdate(int score)
    {
        gameScore += score;
        labelScore.text = "" + gameScore;
    }

    void OnTimerUpdate(int time)
    {
        gameTime += time;
        labelTime.text = string.Format("{0}s", gameTime);
    }

    void OnCarryVegetableUpdate(Command command, Sprite sprite)
    {
        Debug.Log("update carrying vegetable UI, command: " + command);
        if (command == Command.DROP)
        {
            var obj = carriedVegetablesTransform.Find("" + carriedVegetablesTransform.childCount);
            Debug.Log(obj);
            if(obj != null)
            {
                Destroy(obj.gameObject);
            }
        }
        else
        {
            Image image = (new GameObject()).AddComponent<Image>();
            image.sprite = sprite;
            image.transform.SetParent(carriedVegetablesTransform);
            image.name = "" + carriedVegetablesTransform.childCount;
        }
    }
}
