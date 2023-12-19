using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform unitRagdollPrefab;
    [SerializeField] private Transform originalRootBone;
    [SerializeField] private Transform bloodPrefab;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = 
            Instantiate(unitRagdollPrefab, transform.position, transform.rotation);
        Instantiate(bloodPrefab, new Vector3 (transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }

    private void OnDestroy()
    {
        
    }
}
