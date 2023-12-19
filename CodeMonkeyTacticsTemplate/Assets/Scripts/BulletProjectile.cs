using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private TrailRenderer trailRender;
    [SerializeField] private Transform bulletVFXPrefab;
    private Vector3 targetPosition;
    
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }


    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        //used to prevent overshoot the target
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            //Unparent the trail render
            trailRender.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletVFXPrefab, targetPosition, Quaternion.identity);
        }
    }
}
