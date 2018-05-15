using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun : Item {
    
    [SerializeField] private Vector2Int firePoint;
    [SerializeField] private int fireRange;
    [SerializeField] private int damage;

    private Vector2Int originalFirePoint;

    public override ItemRect UseRange
    {
        get
        {
            int properlyFlippedFireRange = flippedX ? -fireRange : fireRange;
            return new ItemRect(Position + firePoint, properlyFlippedFireRange, 1);
        }
    }

    public override void Use()
    {
        int properlyFlippedFireRange = flippedX ? -fireRange : fireRange;
        var possibleTargets = Inventory.Instance.AtRect<EntityBase>(Position + firePoint, properlyFlippedFireRange, 1, Inventory.ScanDirection.NONE);
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


    public override void Flip(bool flipX, bool flipY)
    {
        base.Flip(flipX, flipY);

        if (flipX)
        {
            firePoint.x = size.x - 1 - originalFirePoint.x;
        }
        else
        {
            firePoint.x = originalFirePoint.x;
        }

        if (flipY)
        {
            firePoint.y = size.y - 1 - originalFirePoint.y;
        }
        else
        {
            firePoint.y = originalFirePoint.y;
        }

        print(firePoint);
    }

    protected override void Start()
    {
        base.Start();
        originalFirePoint = firePoint;
    }
}