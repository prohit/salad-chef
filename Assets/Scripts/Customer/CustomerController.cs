using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerController : MonoBehaviour
{
    [SerializeField] GameOverController gameOverController;
    [SerializeField] Customer customerPrefab;
    //[SerializeField] Transform transform;
    [SerializeField] CustomerStand[] customerStands;
    [SerializeField] Vector3 offset = new Vector3(0, 100, 0);
    [SerializeField] int minDelayInCustomerArriaval = 20; //seconds

    Action<Customer> customerLeaveEvent;
    int totalCustomerCount;
    bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        AddCustomerLeftEvent(OnCustomerLeft);
        gameOverController.AddGameOverEvent(OnGameOver);
        StartCoroutine(AssignCustomers());
    }

    private void OnDestroy()
    {
        RemoveCustomerLeftEvent(OnCustomerLeft);
        if(gameOverController != null)
        {
            gameOverController.AddGameOverEvent(OnGameOver);
        }
    }

    //assign customers to all customer stand
    IEnumerator AssignCustomers()
    {
        int index = 0;
        foreach(var customerStand in customerStands)
        {
            yield return StartCoroutine(AssignCustomer(customerStand, index++ * 20));        
        }
    }

    //create a new customer and assign it to a CustomerStand
    IEnumerator AssignCustomer(CustomerStand customerStand, int minDelay = 1)
    {
        yield return new WaitForSeconds(new System.Random().Next(minDelay, minDelay + 10));
        if (!isGameOver)
        {
            Debug.Log("Parent pos: " + (transform.position));
            Customer customer = Instantiate(customerPrefab);
            customer.name = customerStand.name;
            customer.transform.position = Camera.main.WorldToScreenPoint(customerStand.transform.position) - transform.position + offset;
            customer.transform.SetParent(transform, false);

            customer.GotoCustomerStand(customerStand);
            customerStand.CustomerArrived(customer);
            customer.RegisterGameOver(gameOverController);
            totalCustomerCount++;
        }
    }

    //called once a custmoer left
    void OnCustomerLeft(Customer customer)
    {
        if (isGameOver) return;
        customer.Stand.CustomerLeft();
        //reduce wait time between new customers to make game difficult over time
        int waitTime = Mathf.Clamp(5, minDelayInCustomerArriaval - totalCustomerCount, minDelayInCustomerArriaval); 
        StartCoroutine(AssignCustomer(customer.Stand, waitTime)); //assign new customer to empty customer stand
        Destroy(customer.gameObject, 0.5f);
    }

    //called when game over
    void OnGameOver()
    {
        isGameOver = true;
    }

    public void AddCustomerLeftEvent(Action<Customer> pEvent)
    {
        customerLeaveEvent += pEvent;
    }

    public void RemoveCustomerLeftEvent(Action<Customer> pEvent)
    {
        if(customerLeaveEvent != null)
        {
            customerLeaveEvent -= pEvent;
        }
    }

    public void TriggerCustomerLeftEvent(Customer customer)
    {
        customerLeaveEvent?.Invoke(customer);
    }
}
