using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 100f;

    public void ObjectHitDamage(float amount)
    {
        objectHealth -= amount;
        if(objectHealth <= 0f)
        {
            objectHealth = 100f;
           // Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
