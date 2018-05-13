using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : Item {
    
    [SerializeField] private Vector2Int firePoint;
    [SerializeField] private int fireRange;
    [SerializeField] private int damage;

    public override void Use()
    {
        var possibleTargets = Inventory.Instance.AtRect<EntityBase>(Position + firePoint, fireRange, 1, Inventory.ScanDirection.NONE);
        var damagableTargets = possibleTargets.OfType<IDamagable>();
        if (damagableTargets.Any())
        {
            damagableTargets.First().Damage(damage);
        }
        //foreach (var target in possibleTargets)
        //{
        //    print(target.gameObject.name);
        //}
    }
}