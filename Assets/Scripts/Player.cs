using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6.0f;
    [SerializeField] float rotateSpeed = 10.0f;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>() ?? gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        characterController.Move(dir * moveSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, dir, rotateSpeed * Time.deltaTime);
        CheckForKitchenStands();
    }

    void CheckForKitchenStands()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);
    }
}
