using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] CustomerStand customerStand;

    float waitingTime;
    string order;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCustomerArrieved(Customer customer)
    {

    }
}
