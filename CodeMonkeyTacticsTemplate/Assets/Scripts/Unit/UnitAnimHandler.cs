using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class UnitAnimHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootSpawnPoint; 
 
    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
            shootAction.OnShooting += ShootAction_OnShoot;
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");
        Transform bulletPrefabTransform =
            Instantiate(bulletPrefab, shootSpawnPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletPrefabTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAction = e.targetUnit.GetWorldPosition();
        targetUnitShootAction.y = shootSpawnPoint.position.y;
        bulletProjectile.Setup(targetUnitShootAction);
    }
}
