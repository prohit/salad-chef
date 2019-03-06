using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6.0f;
    [SerializeField] float rotateSpeed = 10.0f;
    [SerializeField] LayerMask kitchenStandLayer;
    [SerializeField] float contactDistance = 2f;
    CharacterController characterController;
    KitchenStand currentStandInContact = null;
    PlayerState currentState;
    PlayerCarrier carrier;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>() ?? gameObject.AddComponent<CharacterController>();
        carrier = GetComponent<PlayerCarrier>() ?? gameObject.AddComponent<PlayerCarrier>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        characterController.Move(dir * moveSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, dir, rotateSpeed * Time.deltaTime);
        ProcessCommands();
    }

    private void FixedUpdate()
    {
        CheckForKitchenStands();
    }

    //do the physics calculation to check user is in contact with any kitchen stand
    void CheckForKitchenStands()
    {
        RaycastHit hit;
        var result = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, contactDistance, kitchenStandLayer);
        Debug.DrawRay(transform.position, result ? hit.transform.position : transform.forward, result ? Color.red : Color.green);    
        if(result)
        {
            var kitchenStand = hit.collider.GetComponent<KitchenStand>();
            if (kitchenStand != null)
            {
                //Debug.Log("Kitchen Stand: " + hit.collider.name);
                kitchenStand.SetPlayerContactEnable(true);
                if(!(currentStandInContact == null || currentStandInContact == kitchenStand))
                {
                    currentStandInContact.SetPlayerContactEnable(false);
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

    void ProcessCommands()
    {
        if(currentStandInContact != null)
        {
            if (currentState == PlayerState.CHOPPING) return;

            if(Input.GetKey(KeyCode.E)) 
            {
                //if player hand is empty, it pickup from kitchen stand
                //if carrying anything it will drop on kitchen stand
                currentStandInContact.Execute((currentState == PlayerState.EMPTY_HAND) ? Command.PICKUP : Command.DROP, carrier);
            }
            else if(Input.GetKey(KeyCode.R))
            {
                //if in contact with chopping stand and state is empty_hand start chopping
                if(currentState == PlayerState.EMPTY_HAND && currentStandInContact is ChoppingStand)
                {
                    currentStandInContact.Execute(Command.CHOPPING);
                }
            }
        }
    }
}
