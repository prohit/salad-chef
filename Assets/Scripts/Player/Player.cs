using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] string horizontalAxis;
    [SerializeField] string verticalAxis;
    [SerializeField] KeyCode pickupDropKey;
    [SerializeField] KeyCode choppingKey;

    [Header("Speed")]
    [SerializeField] float moveSpeed = 6.0f;
    [SerializeField] float rotateSpeed = 10.0f;

    [Header("Interaction")]
    [SerializeField] LayerMask kitchenStandLayer;
    //[SerializeField] float contactDistance = 2f;
    [SerializeField] int vegCarryLimit = 2;

    [Header("Reward")]
    [SerializeField] LayerMask rewardLayerMask;
    [SerializeField] float speedBoostDuration = 60; //seconds

    CharacterController characterController;
    KitchenStand currentStandInContact;
    PlayerState currentState;
    Carrier carrier;
    float speedBoost;

    Action<int> updateScoreEvent;
    Action<int> updateTimeEvent;
    Action<Command, Sprite> carryVegetableEvent;

    public bool IsTimeOver;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>() ?? gameObject.AddComponent<CharacterController>();
        carrier = GetComponent<Carrier>();
    }

    void Move()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw(horizontalAxis), 0, Input.GetAxisRaw(verticalAxis)).normalized;
        characterController.Move(dir * (moveSpeed + speedBoost) * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, dir, rotateSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTimeOver)
        {
            if (currentState != PlayerState.CHOPPING)
            {
                Move();
                ProcessCommands();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsTimeOver)
        {
            CheckForKitchenStands();
            CheckForRewards();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == kitchenStandLayer)
        {
            currentStandInContact.SetPlayerContactEnable(false);
            currentStandInContact = null;
        }
    }

    //do the physics calculation to check user is in contact with any kitchen stand
    void CheckForKitchenStands()
    {
        //RaycastHit hit;
        //var result = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, contactDistance, kitchenStandLayer);
        //Debug.DrawRay(transform.position, result ? hit.transform.position : transform.forward, result ? Color.red : Color.green);
        var colliders = Physics.OverlapCapsule(transform.position + Vector3.down * characterController.height / 2,
            transform.position + Vector3.up * characterController.height / 2, characterController.radius, kitchenStandLayer);
           
        if(colliders?.Length > 0)
        {
            var kitchenStand = colliders[0].GetComponentInParent<KitchenStand>();
            if (kitchenStand != null)
            {
                //Debug.Log("Kitchen Stand: " + hit.collider.name);

                if(!(currentStandInContact == null || currentStandInContact == kitchenStand))
                {
                    currentStandInContact.SetPlayerContactEnable(false);
                }
                else
                {
                    kitchenStand.SetPlayerContactEnable(true);
                }
                currentStandInContact = kitchenStand;
            }
        }
        else
        {
            if(currentStandInContact != null)
            {
                currentStandInContact.SetPlayerContactEnable(false);
                currentStandInContact = null;
            }
        }
    }

    //check user is in contact with rewards
    void CheckForRewards()
    {    
        var colliders = Physics.OverlapCapsule(transform.position + Vector3.down * characterController.height / 2,
        transform.position + Vector3.up * characterController.height / 2, characterController.radius, rewardLayerMask);
        if (colliders?.Length > 0)
        {
            var reward = colliders[0].GetComponentInParent<Reward>();// hit.collider.GetComponentInParent<Reward>();
            if (reward.Owner == this)
            {
                colliders[0].enabled = false;
                CollectReward(reward);
            }
        }
    }

    //process user commands - Pickup, Drop, Chopping
    void ProcessCommands()
    {
        if(currentStandInContact != null)
        {
            if(Input.GetKeyDown(pickupDropKey)) 
            {
                Debug.Log("Command Pickup/Drop");
                if (carrier.IsCarrying())
                {
                    //if player can carry it will pickup item from vegetable stand otherwise it will drop the first item
                    if ( currentStandInContact is VegetableStand && carrier.GetCarryingItem().Type == PickableType.VEGETABLE && carrier.ItemsCount() < vegCarryLimit)
                    {
                        Debug.Log("Carrying, Command Pickup");
                        PickupMore();
                    }
                    else
                    {
                        Debug.Log("Carrying, Command Drop");
                        Drop();
                    }
                }
               else
                {
                    Debug.Log("Not Carrying, Command Pickup");
                    Pickup();
                }
            }
            else if(Input.GetKeyDown(choppingKey))
            {
                Chopping();
            }
        }
    }

    void Pickup()
    {
        bool success = currentStandInContact.Execute(Command.PICKUP, carrier);
        if(success)
        {
            if (currentStandInContact is VegetableStand)
            {
                currentState = PlayerState.CARRYING_VEGETABLE;
                carryVegetableEvent?.Invoke(Command.PICKUP, ((VegetableItem)carrier.GetLastCarryingItem()).Icon);
            }
            else if (currentStandInContact is ChoppingStand) currentState = PlayerState.CARRYING_CHOPPED_VEGETABLE;
            else if (currentStandInContact is PlateStand) currentState = PlayerState.CARRYING_PLATE;
        }     
    }

    void PickupMore()
    {
        bool success = currentStandInContact.Execute(Command.PICKUP, carrier);
        if (success)
        {
            currentState = PlayerState.CARRYING_VEGETABLE;
            carryVegetableEvent?.Invoke(Command.PICKUP, ((VegetableItem)carrier.GetLastCarryingItem()).Icon);
        }
        Debug.Log("Current state: " + currentState);
    }

    void Drop()
    {
        if((currentState == PlayerState.CARRYING_VEGETABLE && currentStandInContact is ChoppingStand)
            || (currentState == PlayerState.CARRYING_CHOPPED_VEGETABLE && currentStandInContact is PlateStand)
            || (currentState == PlayerState.CARRYING_PLATE && currentStandInContact is CustomerStand))
        {
            bool success = currentStandInContact.Execute(Command.DROP, carrier);
            if(success)
            {
                if (currentState == PlayerState.CARRYING_VEGETABLE)
                {
                    carryVegetableEvent?.Invoke(Command.DROP, null);
                }
                if(carrier.IsCarrying() == false)
                {
                    currentState = PlayerState.EMPTY_HAND;
                }
            }
            /*
            if (success && carrier.IsCarrying() == false)
            {
                currentState = PlayerState.EMPTY_HAND;
            }
            */
            //TODO else condition
        }
        Debug.Log("Current state: " + currentState);
    }

    void Chopping()
    {
        Debug.Log("Current state: " + currentState);
        //if in contact with chopping stand and state is empty_hand start chopping
        if (currentStandInContact is ChoppingStand && currentState == PlayerState.EMPTY_HAND || currentState == PlayerState.CARRYING_VEGETABLE)
        {
            bool success = currentStandInContact.Execute(Command.CHOPPING);
            if (success)
            {
                ((ChoppingStand)currentStandInContact).AddChoppingDoneEvent(OnChoppingDone);
                currentState = PlayerState.CHOPPING;
            }
        }
    }

    public void OnChoppingDone()
    {
        ((ChoppingStand)currentStandInContact).RemoveChoppingDoneEvent(OnChoppingDone);
        if (carrier.IsCarrying())
        {
            currentState = PlayerState.CARRYING_VEGETABLE;
        }
        else
        {
            currentState = PlayerState.EMPTY_HAND;
        }
    }

    void CollectReward(Reward reward)
    {
        Debug.Log("Collect Reward: " + reward.Type);
        switch (reward.Type)
        {
            case RewardType.SCORE:
                updateScoreEvent.Invoke((int)reward.RewardAmount);
                break;

            case RewardType.TIME:
                updateTimeEvent.Invoke((int)reward.RewardAmount);
                break;

            case RewardType.SPEED:
                speedBoost = reward.RewardAmount;
                StartCoroutine(DisableSpeedBoost());
                break;
        }
        Destroy(reward.gameObject, 0.1f);
    }

    IEnumerator DisableSpeedBoost()
    {
        yield return new WaitForSeconds(speedBoostDuration);
        speedBoost = 0;
    }


    public void AddScoreUpdateEvent(Action<int> pEvent)
    {
        updateScoreEvent += pEvent;
    }

    public void RemoveScoreUpdateEvent(Action<int> pEvent)
    {
        if(updateScoreEvent != null)
        {
            updateScoreEvent -= pEvent;
        }
    }

    public void AddUpdateTimeEvent(Action<int> pEvent)
    {
        updateTimeEvent += pEvent;
    }

    public void RemoveUpdateTimeEvent(Action<int> pEvent)
    {
        if(updateTimeEvent != null)
        {
            updateTimeEvent -= pEvent;
        }
    }

    public void AddCarryVegetableEvent(Action<Command, Sprite> pEvent)
    {
        carryVegetableEvent += pEvent;
    }

    public void RemoveCarryVegetableEvent(Action<Command, Sprite> pEvent)
    {
        if(carryVegetableEvent != null)
        {
            carryVegetableEvent -= pEvent;
        }
    }
}
