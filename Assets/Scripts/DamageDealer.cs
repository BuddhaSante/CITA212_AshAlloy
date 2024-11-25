using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10; // damage value

    public int GetDamage() // getter for damage
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
