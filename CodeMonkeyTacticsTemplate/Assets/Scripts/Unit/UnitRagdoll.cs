using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    private void Update()
    {
        Destroy(gameObject, 5f);
    }
    public void Setup(Transform originRootBone)
    {
        MatchAllChildTransforms(originRootBone, ragdollRootBone);
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position, 10f);

    }
    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(clone.name);
            if(cloneChild != null)
            {
                cloneChild.position = clone.position;
                cloneChild.rotation = clone.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }
    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPos, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPos, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPos, explosionRange);
        }
    }

    
}
