using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Vector3 targetPosition;
    //private float movespeed; 

    private void Awake()
    {
        //Target position should be set based on the distance to the ground
        targetPosition = transform.position;
    }

    private void Update()
    {
        //Check how close the player is to the target position
        float distanceFromTarget = (targetPosition - transform.position).magnitude;
        float distanceThreshhold = 0.2f;
        if (distanceFromTarget > distanceThreshhold) // calculate the minimum distance the unit can be from the target
        {
            //calcualte the direction
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float tempMoveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * tempMoveSpeed;
            //Rotate Towards
            float rotateTowards = 15f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection * rotateTowards, Time.deltaTime);
            //Animators
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);
    }

    #region Setter Functions
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
    #endregion
}
