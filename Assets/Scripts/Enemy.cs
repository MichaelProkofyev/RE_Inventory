using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase, IDamagable {

    [SerializeField] private int health;
    
    public void Damage(int amount)
    {
        health -= amount;
        foreach (var shake in objectShakers)
        {
            shake.ShakeObject();
        }
        if (health <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    void Update()
    {

    }
}
