using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;

    private void Awake()
    {
        //Target position should be set based on the distance to the ground
        // if the level designer places the unit high then for now the code should bring it to the ground
        targetPosition = transform.position;
    }


    private void Update()
    {
        //Check how close the player is to the target position
        float distanceFromTarget = (targetPosition - transform.position).magnitude;
        float distanceThreshhold = 0.2f;
        if(distanceFromTarget > distanceThreshhold)
        {
            Debug.Log("Reached the Target Position");
            //Move toward target position
            //calcualte the direction
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
    }
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
