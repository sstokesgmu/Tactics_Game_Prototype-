using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        // Detach from parent before destroying
        transform.parent = null;
        StartCoroutine(DestroyAfterParticles());
    }

    IEnumerator DestroyAfterParticles()
    {
        // Wait for the particle system to finish playing
        yield return new WaitForSeconds(particleSystem.main.duration);

     

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
