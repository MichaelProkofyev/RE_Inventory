using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : Item {

    [SerializeField] private int damage;

    public override void Use()
    {
        var possibleTargets = Inventory.Instance.AtRect<EntityBase>(UseRange);
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

    public override void SetState(ItemState state)
    {
        base.SetState(state);

        switch (state)
        {
            case ItemState.IDLE:
                break;
            case ItemState.HOVERED_OVER:
                break;
            case ItemState.PICKED:
                break;
            default:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
