using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaladOrder
{
    readonly string orderName;

    public SaladOrder(List<VegType> items)
    {
        orderName = GetSaladName(items);
    }

    string GetSaladName(List<VegType> items)
    {
        items.Sort();
        string name = "";
        foreach (var item in items)
        {
            name += item + ", ";
        }
        name = name.Trim().Remove(name.Length - 2);
        return name;
    }

    public bool IsRightSalad(SaladItem item)
    {
        string other = GetSaladName(item.VegetableItemTypes);
        Debug.Log(string.Format("Order {0} and recieved order: {1}", orderName, other));
        return (orderName == other);
    }

    public string Getname()
    {
        return orderName;
    }

}

public class Customer : MonoBehaviour
{
    [SerializeField] Image waitingTimeBar;
    [SerializeField] Text labelOrder;

    const float awardTimePercentage = 70f;
    const float waitTimePerItem = 30; //seconds //TODO will be different for each item

    Animator animator;
    int wrongOrderServedHash = Animator.StringToHash("wrong_order_served");

    SaladOrder Order; // { get; private set; }
    float WaitingTime; // { get; private set; }
    float totalWaitTime;
    List<Player> wrongServePlayers;
    bool isServerd;

    public CustomerStand Stand { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Player ServedBy { get; private set; }

    Coroutine waitingTimeCoroutine;

    GameOverController gameOverController;
    bool isGameOver;

    void Start()
    {
        animator = GetComponent<Animator>();

        System.Random rand = new System.Random();
        int itemCount = rand.Next(2, 4); //item count will be 2 or 3 only, //TODO modify value based on user progress
        List<VegType> vegTypes = Enum.GetValues(typeof(VegType)).Cast<VegType>().ToList();
        var itemList = vegTypes.OrderBy(x => rand.Next()).Take(itemCount).ToList();
        Order = new SaladOrder(itemList);

        totalWaitTime = waitTimePerItem * itemList.Count;
        WaitingTime = totalWaitTime;
        labelOrder.text = Order.Getname();
        wrongServePlayers = new List<Player>();
        OrderStatus = OrderStatus.NOT_SERVED;
    }

    private void OnDestroy()
    {
        if(gameOverController != null)
        {
            gameOverController.RemoveGameOverEvent(OnGameOver);
        }
    }

    //set customer statnd to customer and wait for order
    public void GotoCustomerStand(CustomerStand stand)
    {
        Stand = stand;
        waitingTimeCoroutine = StartCoroutine(WaitingTmeCounter());
    }

    public void RegisterGameOver(GameOverController controller)
    {
        gameOverController = controller;
        gameOverController.AddGameOverEvent(OnGameOver);
    }

    //check for salad order is correct or not
    bool IsRightOrder(SaladItem saladItem)
    {
        if (isServerd) return false;
        if (WaitingTime <= 0) return false;
        return Order.IsRightSalad(saladItem);
    }

    //proces order recieved by player and take necessary actions
    //deuct time if wrong order, give points if correct order and reward user if fast delivery
    //customer left in case of correct order
    public void ProcessOrder(SaladItem saladItem, Player player)
    {
        bool correctOreder = IsRightOrder(saladItem);
        if (correctOreder)
        {
            isServerd = true;
            if (waitingTimeCoroutine != null)
            {
                StopCoroutine(waitingTimeCoroutine);
            }

            float compltedTimePercentage = 100f - waitingTimeBar.fillAmount * 100f;
            Debug.Log("Correct order, served in : " + compltedTimePercentage);
            if (compltedTimePercentage <= awardTimePercentage)
            {
                //TODO give rewards
                OrderStatus = OrderStatus.FAST_SERVED;
            }
            else
            {
                OrderStatus = OrderStatus.CORRECT_SALAD_SERVERD;
            }
            ServedBy = player;

            //customer leave
            if(wrongServePlayers.Contains(player))
            {
                wrongServePlayers.Remove(player);
            }
            transform.parent.GetComponent<CustomerController>().TriggerCustomerLeftEvent(this);
        }
        else
        {
            Debug.Log("Wrong order!");
            animator.SetTrigger(wrongOrderServedHash);
            OrderStatus = OrderStatus.WRONG_SALAD_SERVED;
            wrongServePlayers.Add(player);
        }
    }

    //waiting time counter, update waiting time bar
    IEnumerator WaitingTmeCounter()
    {
        yield return new WaitForEndOfFrame();
        while (WaitingTime > 0)
        {
            yield return new WaitForSeconds(1);
            WaitingTime--;

            //waiting time decrease faster in case of wrong salad server
            if (OrderStatus == OrderStatus.WRONG_SALAD_SERVED)
            {
                WaitingTime -= 0.5f;
            }
            waitingTimeBar.fillAmount = WaitingTime / totalWaitTime;
        }
        //time over customer will leave
        transform.parent.GetComponent<CustomerController>().TriggerCustomerLeftEvent(this);
    }

    public bool IsWronServed(Player player)
    {
        return wrongServePlayers.Contains(player);
    }

    void OnGameOver()
    {
        isGameOver = true;
        StopCoroutine(waitingTimeCoroutine);
    }
}
