using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    const int GAME_TIME = 120; //seconds
    const int CUSTOMER_LEFT_PENALTY = -20;
    const int CORRECT_ORDER_POINT = 50;

    [SerializeField] Player player;
    [SerializeField] CustomerController customerController;
    [SerializeField] GameOverController gameOverController;

    int gameTime; 
    int gameScore;
    Text labelScore;
    Text labelTime;
    int bestScore;

    Transform carriedVegetablesTransform;

    //TODO show score chnage
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        bestScore = PlayerPrefs.GetInt(string.Format("{0}_Best_Score", name), 0);
        gameTime = GAME_TIME;
        labelScore = transform.Find("Score/Text").GetComponent<Text>();
        labelTime = transform.Find("Time/Text").GetComponent<Text>();
        carriedVegetablesTransform = transform.Find("CarriedVegetables");

        player.AddScoreUpdateEvent(OnScoreUpdate);
        player.AddUpdateTimeEvent(OnTimerUpdate);
        player.AddCarryVegetableEvent(OnCarryVegetableUpdate);

        customerController.AddCustomerLeftEvent(OnCustomerLeft);

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
        if(customerController != null)
        {
            customerController.RemoveCustomerLeftEvent(OnCustomerLeft);
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
        //time over
        player.IsTimeOver = true;
        if(gameScore > bestScore)
        {
            bestScore = gameScore;
            PlayerPrefs.SetInt(string.Format("{0}_Best_Score", name), gameScore);
        }
        gameOverController.TimeOver(name, gameScore, bestScore);
    }

    void OnScoreUpdate(int score)
    {
        Debug.Log("Score added: " + score);
        //TODO show animation
        gameScore += score;
        //if (gameScore < 0) gameScore = 0;
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

    void OnCustomerLeft(Customer customer)
    {
        if (player.IsTimeOver) return;
        switch(customer.OrderStatus)
        {
            case OrderStatus.NOT_SERVED:
                //do nothing for now
                break;

            case OrderStatus.WRONG_SALAD_SERVED:
                int penalty = (customer.IsWronServed(player) ? 2 : 1) * CUSTOMER_LEFT_PENALTY;
                OnScoreUpdate(penalty);
                break;

            case OrderStatus.CORRECT_SALAD_SERVERD:
            case OrderStatus.FAST_SERVED:
                if(player == customer.ServedBy)
                {
                    OnScoreUpdate(CORRECT_ORDER_POINT);
                }
                break;
        }
    }
}
