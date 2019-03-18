using UnityEngine;

public class ReawrdsController : MonoBehaviour
{
    [SerializeField] CustomerController customerController;
    [SerializeField] Reward[] rewardPrefabs;
    [SerializeField] float kitchenLength = 16f;
    [SerializeField] float kitchenWidth = 16f;

    // Start is called before the first frame update
    void Start()
    {
        customerController.AddCustomerLeftEvent(OnCustomerLeft);
    }

    private void OnDestroy()
    {
        if(customerController != null)
        {
            customerController.RemoveCustomerLeftEvent(OnCustomerLeft);
        }
    }

    //called once customer left
    void OnCustomerLeft(Customer customer)
    {
        switch(customer.OrderStatus)
        {
            case OrderStatus.FAST_SERVED:
                SpawnReward(customer.ServedBy);
                break;
        }
    }

    //spawn random rewards
    void SpawnReward(Player owner)
    {
        System.Random rand = new System.Random();
        int index = rand.Next(rewardPrefabs.Length);
        Reward reward = Instantiate(rewardPrefabs[index]);
        reward.Owner = owner;

        Debug.Log("Spawn Reward: " + reward.Type);

        var xPos = (float)(rand.NextDouble() - 0.5f) * kitchenWidth;
        var zPos = (float)(rand.NextDouble() - 0.5f) * kitchenWidth;
        reward.transform.position = new Vector3(xPos, 0, zPos);
        //TODO avoid player current position
    }
}
