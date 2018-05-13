using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase, IDamagable {

    private int health;
    
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    private void Start()
    {
        foreach (var sprite in sprites)
        {
            originalColor = sprite.color;
            orignalScale = sprite.transform.localScale;
        }
    }
}
