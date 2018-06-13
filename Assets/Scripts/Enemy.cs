using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase, IDamagable, AIEntity {

    [SerializeField] private int health;

    public void Act()
    {
        //print($"{gameObject.name} acting!");

        System.Func<MoveDirection, bool> tryMoving = (direction) =>
        {
            if (Inventory.Instance.CanItemBeMoved(this, direction, true))
            {
                Move(direction);
                return true;
            }
            return false;
        };

        bool moved = false;
        int numberOfTries = 0;
        while (moved == false)
        {
            float rand = Random.value;
            if (rand < .25f)
            {
                moved = tryMoving(MoveDirection.UP);
            }
            else if (rand < .5f)
            {
                moved = tryMoving(MoveDirection.DOWN);
            }
            else if (rand < .75f)
            {
                moved = tryMoving(MoveDirection.RIGHT);
            }
            else if (rand < 1f)
            {
                moved = tryMoving(MoveDirection.LEFT);
            }
            numberOfTries++;
            if (numberOfTries > 4)
            {
                break;
            }
        }
    }

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
