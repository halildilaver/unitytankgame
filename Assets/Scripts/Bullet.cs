using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthBehaviour healthBehaviour = collision.gameObject.GetComponent<HealthBehaviour>();

        if(healthBehaviour != null)
        {
            healthBehaviour.TakeDamage(20);
            Destroy(gameObject,0.1f);
        }
        Destroy(gameObject, 1);
    }
}
