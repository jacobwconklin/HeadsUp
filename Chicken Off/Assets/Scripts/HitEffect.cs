using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{

    [SerializeField] private GameObject hitParticleEffect;
    // private int count = 0;

    private void Update()
    {
        /* Just for testing the look:
        if (++count % 60 == 0)
        {
            CreateHitEffect();
        }
        */
    }

    public void CreateHitEffect()
    {
        Instantiate(hitParticleEffect, transform.position, Quaternion.identity);
    }
}
