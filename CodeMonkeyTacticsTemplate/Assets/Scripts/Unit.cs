using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;

    private void Update()
    {
        //Check how close the player is to the target position
        float distanceFromTarget = (targetPosition - transform.position).magnitude;
        float distanceThreshhold = 0.1f;
        if(distanceFromTarget > distanceThreshhold)
        {
            Debug.Log("Reached the Target Position");
            //Move toward target position
            //calcualte the direction
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }

        //Simple Testing 
        if (Input.GetKeyDown(KeyCode.T))
            Move(new Vector3(4, 0, 4));
    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
